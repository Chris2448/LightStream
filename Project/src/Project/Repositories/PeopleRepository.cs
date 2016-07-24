using Microsoft.Extensions.OptionsModel;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using Project.Models;

namespace Project.Repositories
{
    public class PeopleRepository : IPeopleRepository
    {
        private readonly DbSettings _config;

        private readonly MongoDatabase _database;

        public PeopleRepository(IOptions<DbSettings> config)
        {
            _config = config.Value;
            _database = Connect();
        }

        public PeopleRepository(string conn, string db)
        {
            _config = new DbSettings
            {
                connectionstring = conn,
                database = db,
                collections = new collections
                {
                    People = "People"
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

        public MongoCursor<People> GetAllPeople()
        {
            var collection = _database.GetCollection<People>(_config.collections.People);
            return collection.FindAll();
        }

        public People GetPersonById(int id)
        {
            var query = Query<People>.EQ(e => e._id, id);
            return _database.GetCollection<People>(_config.collections.People).FindOne(query);
        }

        public People GetPersonByName(string name)
        {
            var query = Query<People>.EQ(e => e.Name, name);
            return _database.GetCollection<People>(_config.collections.People).FindOne(query);
        }

        public void AddPerson(People person)
        {
            _database.GetCollection<People>(_config.collections.People).Save(person);
        }

        public void UpdatePerson(People person)
        {
            var query = Query<People>.EQ(e => e._id, person._id);
            var update = Update<People>.Replace(person); // update modifiers
            _database.GetCollection<People>(_config.collections.People).Update(query, update);
        }

        public bool RemovePerson(int id)
        {
            var query = Query<People>.EQ(e => e._id, id);
            _database.GetCollection<People>(_config.collections.People).Remove(query);

            return GetPersonById(id) == null;
        }
    }
}
