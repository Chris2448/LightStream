using System.Collections.Generic;
using Project.Models;

namespace Project.Repositories
{
    public interface IProgramsRepository
    {
        IEnumerable<Programs> GetAllPrograms();

        Programs GetProgramById(int id);

        IEnumerable<Programs> GetProgramByIds(IEnumerable<int> ids);

        IEnumerable<Programs> GetProgramBySearch(string title);

        IEnumerable<Programs> GetSuggestedPrograms(int id);

        void AddProgram(Programs program);

        void UpdateProgram(Programs program);

        bool RemoveProgram(int id);
    }
}
