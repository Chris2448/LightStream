using MongoDB.Driver;
using MongoDB.Driver.Builders;
using WorkerRole.Models;

namespace WorkerRole.Repositories
{
    public class SuggestionsRepository : ISuggestionsRepository
    {
        private readonly DbSettings _config;

        private readonly MongoDatabase _database;

        public SuggestionsRepository(DbSettings config)
        {
            _config = config;
            _database = Connect();
        }

        public SuggestionsRepository(string conn, string db)
        {
            _config = new DbSettings
            {
                connectionstring = conn,
                database = db,
                collections = new collections
                {
                    Suggestions = "Suggestions"
                }
            };

            _database = Connect();
        }

        private MongoDatabase Connect()
        {
            var client = new MongoClient(_config.connectionstring);
            var server = client.GetServer();
            var database = server.GetDatabase(_config.database);

            return database;
        }

        public Suggestions GetSuggestionByProgramId(int id)
        {
            var query = Query<Suggestions>.EQ(e => e.ProgramId, id);
            return _database.GetCollection<Suggestions>(_config.collections.Suggestions).FindOne(query);
        }

        public void AddSuggestion(Suggestions suggestion)
        {
            _database.GetCollection<Suggestions>(_config.collections.Suggestions).Save(suggestion);
        }

        public void UpdatePerson(Suggestions suggestion)
        {
            var query = Query<Suggestions>.EQ(e => e._id, suggestion._id);
            var update = Update<Suggestions>.Replace(suggestion); // update modifiers
            _database.GetCollection<Suggestions>(_config.collections.Suggestions).Update(query, update);
        }
    }
}
