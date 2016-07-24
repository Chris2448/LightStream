using MongoDB.Driver;
using Project.Models;

namespace Project.Repositories
{
    public interface ISuggestionsRepository
    {
        //MongoCursor<People> GetAllPeople();

        Suggestions GetSuggestionByProgramId(int id);

        void AddSuggestion(Suggestions suggestion);

        void UpdatePerson(Suggestions suggestion);
    }
}
