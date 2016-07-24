using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.OptionsModel;
using Project.Models;
using Project.Repositories;

namespace Project.Controllers
{
    public class HomeController : Controller
    {
        private IOptions<DbSettings> Config { get; } 

        private ProgramsRepository ProgramRepo { get; }

        private PeopleRepository PeopleRepo { get; }

        private SuggestionsRepository SuggestionRepo { get; }

        public HomeController(IOptions<DbSettings> config)
        {
            Config = config;
            ProgramRepo = new ProgramsRepository(Config);
            PeopleRepo = new PeopleRepository(Config);
            SuggestionRepo = new SuggestionsRepository(Config);
        }

        public IActionResult Index()
        {
            return View(ProgramRepo.GetAllPrograms().ToList());
        }

        public IActionResult ProgramInfo(int id)
        {
            var myModel = new ProgramSuggested
            {
                Program = ProgramRepo.GetProgramById(id),
                Suggestions = SuggestionRepo.GetSuggestionByProgramId(id) ?? new Suggestions { SuggestedPrograms = new List<Programs>()}
            };

            return View(myModel);
        }

        public IActionResult PeopleInfo(string name)
        {
            var person = PeopleRepo.GetPersonByName(name);
            var ids = person.Enrollments.Select(enrollment => enrollment.ProgramId).ToList();

            dynamic myModel = new ExpandoObject();
            myModel.Person = person;
            myModel.Programs = ProgramRepo.GetProgramByIds(ids);

            return View(myModel);
        }

        public IActionResult Search(string q)
        {
            dynamic model = new ExpandoObject();
            model.Person = new People();
            model.Programs = ProgramRepo.GetProgramBySearch(q);
            return View(model);
        }
    }
}
