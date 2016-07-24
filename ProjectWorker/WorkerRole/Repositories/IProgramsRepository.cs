using System.Collections.Generic;
using WorkerRole.Models;

namespace WorkerRole.Repositories
{
    public interface IProgramsRepository
    {
        IEnumerable<Programs> GetAllPrograms();

        Programs GetProgramById(int id);

        void AddProgram(Programs program);

        void UpdateProgram(Programs program);

        bool RemoveProgram(int id);
    }
}
