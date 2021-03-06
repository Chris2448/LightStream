﻿using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Extensions.OptionsModel;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using Newtonsoft.Json;
using Project.Models;

namespace Project.Repositories
{
    public class ProgramsRepository : IProgramsRepository
    {
        private readonly DbSettings _config;

        private readonly MongoDatabase _database;

        public ProgramsRepository(IOptions<DbSettings> config)
        {
            _config = config.Value;
            _database = Connect();
        }

        public ProgramsRepository(string conn, string db)
        {
            _config = new DbSettings
            {
                connectionstring = conn,
                database = db,
                collections = new collections
                {
                    Programs = "Programs"
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

        public IEnumerable<Programs> GetAllPrograms()
        {
            var collection = _database.GetCollection(_config.collections.Programs).FindAll().SetSortOrder(SortBy.Ascending("Title")).ToJson();
            return JsonConvert.DeserializeObject<IEnumerable<Programs>>(collection);
        }

        public Programs GetProgramById(int id)
        {
            var query = Query<Programs>.EQ(e => e._id, id);
            var cursor = _database.GetCollection(_config.collections.Programs).FindOne(query);

            return JsonConvert.DeserializeObject<Programs>(cursor.ToJson());
        }

        public IEnumerable<Programs> GetProgramByIds(IEnumerable<int> ids)
        {
            var query = Query.In("_id", new BsonArray(ids));
            var cursor = _database.GetCollection(_config.collections.Programs).Find(query);

            return JsonConvert.DeserializeObject<IEnumerable<Programs>>(cursor.ToJson());
        }

        public IEnumerable<Programs> GetProgramBySearch(string title)
        {
            var query = Query.Matches("Title", new BsonRegularExpression(new Regex("/" + title) + "/i"));
            var cursor = _database.GetCollection(_config.collections.Programs).Find(query);

            return JsonConvert.DeserializeObject<IEnumerable<Programs>>(cursor.ToJson());
        }

        public IEnumerable<Programs> GetSuggestedPrograms(int id)
        {
            var query = Query<Programs>.EQ(e => e._id, id);
            var cursor = _database.GetCollection(_config.collections.Suggestions).FindOne(query);

            return JsonConvert.DeserializeObject<IEnumerable<Programs>>(cursor.ToJson());
        }

        public void AddProgram(Programs program)
        {
            _database.GetCollection<Programs>(_config.collections.Programs).Save(program);
        }

        public void UpdateProgram(Programs program)
        {
            var query = Query<Programs>.EQ(e => e._id, program._id);
            var update = Update<Programs>.Replace(program); // update modifiers
            _database.GetCollection<Programs>(_config.collections.Programs).Update(query, update);
        }

        public bool RemoveProgram(int id)
        {
            var query = Query<Programs>.EQ(e => e._id, id);
            _database.GetCollection<Programs>(_config.collections.Programs).Remove(query);

            return GetProgramById(id) == null;
        }
    }
}
