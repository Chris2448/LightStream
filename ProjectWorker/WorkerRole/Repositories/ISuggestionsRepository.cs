using MongoDB.Driver;
using WorkerRole.Models;

namespace WorkerRole.Repositories
{
    public interface ISuggestionsRepository
    {
        //MongoCursor<People> GetAllPeople();

        Suggestions GetSuggestionByProgramId(int id);

        void AddSuggestion(Suggestions suggestion);

        void UpdatePerson(Suggestions suggestion);
    }
}
