using Microsoft.AspNetCore.Mvc;

namespace Projeto_Unifeob.Controllers {
    public class HomeController : Controller {
        public IActionResult Index() {
            return View();
        }
    }
}
