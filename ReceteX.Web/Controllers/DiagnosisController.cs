using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace ReceteX.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DiagnosisController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
