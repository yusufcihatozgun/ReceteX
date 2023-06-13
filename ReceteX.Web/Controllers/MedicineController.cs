using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReceteX.Repository.Shared.Abstract;
using ReceteX.Repository.Shared.Concrete;
using ReceteX.Models;


namespace ReceteX.Web.Controllers
{
    [Authorize]
    public class MedicineController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public MedicineController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult GetAll()
        {
            return Json(unitOfWork.Medicines.GetAll());
        }
    }
}
