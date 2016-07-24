using MongoDB.Driver;
using WorkerRole.Models;

namespace WorkerRole.Repositories
{
    public interface IPeopleRepository
    {
        MongoCursor<People> GetAllPeople();

        People GetPersonById(int id);

        People GetPersonByName(string name);

        void AddPerson(People person);

        void UpdatePerson(People person);

        bool RemovePerson(int id);
    }
}
