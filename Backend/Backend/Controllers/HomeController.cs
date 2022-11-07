using Backend.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    public class HomeController : Controller
    {
        private IUsersRepository _usersRepository;

        public HomeController(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        [HttpGet]
        public string GetUserData(string name)
        {
            string data = _usersRepository.GetUserData(name);
            return data;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
