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

        public IActionResult Index()
        {
            return View();
        }
    }
}
