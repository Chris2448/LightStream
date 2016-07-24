using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using Newtonsoft.Json;
using WorkerRole.Models;
using WorkerRole.Repositories;

namespace WorkerRole
{
    public class TableFiller
    {
        private string DatabaseName { get; }

        private string ConnectionString { get; }

        private string Counters { get; }

        private string Configurations { get; }

        private MongoDatabase Database { get; }

        private Configurations Config { get; }


        public TableFiller(DbSettings configuration)
        {
            Configurations = configuration.collections.Configurations;
            Counters = configuration.collections.Counters;

            ConnectionString = configuration.connectionstring;
            DatabaseName = configuration.database;

            Database = new MongoClient(ConnectionString).GetServer().GetDatabase(DatabaseName);

            var configJson = Database.GetCollection(Configurations).FindOne().ToJson();
            Config = JsonConvert.DeserializeObject<Configurations>(configJson);
        }

        public void FillPeople()
        {
            var config = Database.GetCollection<Configurations>(Configurations).FindOne();

            if (!config.NeedUpdate["PeopleUpdate"])
                return;

            var programRepository = new ProgramsRepository(ConnectionString, DatabaseName);
            var programs = programRepository.GetAllPrograms();

            foreach (var program in programs)
            {
                if (program.PeopleAdded)
                    continue;

                var peopleRepository = new PeopleRepository(ConnectionString, DatabaseName);

                //Log
                Trace.TraceInformation("People: Started Processing Actors...");
                foreach (var actor in program.Actors)
                {
                    AddPerson(peopleRepository, actor, program, "Actor");
                }
                Trace.TraceInformation("People: Finished Processing Actors...");

                Trace.TraceInformation("People: Started Processing Directors...");
                foreach (var director in program.Directors)
                {
                    AddPerson(peopleRepository, director, program, "Director");
                }
                Trace.TraceInformation("People: Started Processing Directors...");

                Trace.TraceInformation("People: Started Processing Writers...");
                foreach (var writer in program.Writers)
                {
                    AddPerson(peopleRepository, writer, program, "Writer");
                }
                Trace.TraceInformation("People: Started Processing Writers...");

                // TODO: REMOVE COMMENT
                //var updatedProgram = (Programs)program.Clone();
                //updatedProgram.PeopleAdded = true;

                //programRepository.UpdateProgram(updatedProgram);
            }

            var updatedConfig = (Configurations) config.Clone();
            updatedConfig.NeedUpdate["PeopleUpdate"] = false;

            var query = Query<Configurations>.EQ(e => e._id, updatedConfig._id);
            var update = Update<Configurations>.Replace(updatedConfig); // update modifiers
            Database.GetCollection<Configurations>(Configurations).Update(query, update);
        }

        public void FillSuggestions()
        {
            var config = Database.GetCollection<Configurations>(Configurations).FindOne();

            if (!config.NeedUpdate["SuggestionsUpdate"])
                return;

            var programRepository = new ProgramsRepository(ConnectionString, DatabaseName);
            var suggestionsRepository = new SuggestionsRepository(ConnectionString, DatabaseName);

            var programs = programRepository.GetAllPrograms().ToList();

            foreach (var program in programs)
            {
                if (program.SuggestionsAdded)
                    continue;

                foreach (var candidate in programs.Where(candidate => program._id != candidate._id))
                {
                    bool addSuggested;
                    var oneGenre = false;
                    var writers = false;
                    var directors = false;
                    var twoActors = false;

                    if (program.Genres.SequenceEqual(candidate.Genres))
                        addSuggested = true;
                    else
                    {
                        if (program.Genres.Except(candidate.Genres).Count() < program.Genres.Count())
                            oneGenre = true;

                        if (program.Directors.SequenceEqual(candidate.Directors))
                            directors = true;

                        if (program.Writers.SequenceEqual(candidate.Writers))
                            writers = true;

                        if (program.Actors.Except(candidate.Actors).Count() <= program.Actors.Count() - 2)
                            twoActors = true;

                        addSuggested = (oneGenre && writers) || (oneGenre && directors) || (oneGenre && twoActors);
                    }

                    if (addSuggested)
                    {
                        AddSuggestion(suggestionsRepository, program, candidate);
                    }
                }
            }
        }

        public void AddPerson(PeopleRepository peopleRepository, string name, Programs program, string role)
        {
            var person = peopleRepository.GetPersonByName(name);

            // if "actor" exisit, then simply append current program to
            // its enrollment array
            if (person != null)
            {
                var updatedPerson = (People) person.Clone();
                updatedPerson.Enrollments.Add(new Enrollment
                {
                    ProgramId = program._id,
                    Role = role
                });

                peopleRepository.UpdatePerson(updatedPerson);
            }
            // else, add this person with this enrollment
            else
            {
                peopleRepository.AddPerson(new People
                {
                    _id = GetNextId("People"),
                    Name = name,
                    Enrollments = new List<Enrollment>
                                {
                                    new Enrollment
                                    {
                                        ProgramId = program._id,
                                        Role = role
                                    }
                                }
                });
            }
        }

        public void AddSuggestion(SuggestionsRepository suggestionsRepository, Programs program, Programs candidate)
        {
            var suggestion = suggestionsRepository.GetSuggestionByProgramId(program._id);

            if (suggestion != null)
            {
                var updatedSuggestion = (Suggestions) suggestion.Clone();
                updatedSuggestion.SuggestedPrograms.Add(candidate);

                suggestionsRepository.UpdatePerson(updatedSuggestion);
            }
            else
            {
                suggestionsRepository.AddSuggestion(new Suggestions
                {
                    _id = GetNextId("Suggestions"),
                    ProgramId = program._id,
                    SuggestedPrograms = new List<Programs>
                                        {
                                            candidate
                                        }
                });
            }
        }

        public int GetNextId(string id)
        {
            var query = Query.EQ("_id", id);

            var findAndModify = Database.GetCollection(Counters).FindAndModify(
                                    new FindAndModifyArgs
                                    {
                                        Query = query,
                                        Update = Update.Inc("incrementedId", 1),

                                    });

            var counter = findAndModify.GetModifiedDocumentAs<Counters>();

            return counter.incrementedId;
        }
    }
}
