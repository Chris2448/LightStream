using System.Collections.Generic;
using Microsoft.AspNet.Mvc;
using Project.Models;
using Project.Repositories;

namespace Project.Controllers
{
    [Route("Programs")]
    [Produces("application/json")]
    public class ProgramsController : Controller
    {
        private readonly IProgramsRepository _programsRepository;

        public ProgramsController(IProgramsRepository programsRepository)
        {
            _programsRepository = programsRepository;
        }

        [HttpGet]
        public IEnumerable<Programs> GetAllPrograms()
        {
            return _programsRepository.GetAllPrograms();
        }

        [HttpGet("{id}")]
        public Programs GetProgramById(int id)
        {
            return _programsRepository.GetProgramById(id);
        }
    }
}
