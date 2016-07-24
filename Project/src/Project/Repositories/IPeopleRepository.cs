using MongoDB.Driver;
using Project.Models;

namespace Project.Repositories
{
    public interface IPeopleRepository
    {
        MongoCursor<People> GetAllPeople();

        People GetPersonById(int id);

        People GetPersonByName(string name);

        void AddPerson(People program);

        void UpdatePerson(People program);

        bool RemovePerson(int id);
    }
}
