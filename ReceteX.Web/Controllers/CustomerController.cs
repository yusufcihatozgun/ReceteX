using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReceteX.Models;
using ReceteX.Repository.Shared.Abstract;

namespace ReceteX.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CustomerController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public CustomerController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }


        public IActionResult Index()
        {
            return View();
        }


        public IActionResult GetAll()
        {
            return Json(new { data = unitOfWork.Customers.GetAllWithUserCount()});
        }


        public IActionResult GetById(Guid Id)
        {
            return Json(unitOfWork.Customers.GetById(Id));
        }


        [HttpPost]
        public IActionResult Update(Customer customer)
        {
            Customer asil = unitOfWork.Customers.GetFirstOrDefault(a => a.Id == customer.Id);
            asil.Name = customer.Name;
            unitOfWork.Customers.Update(asil);
            unitOfWork.Save();
            return Ok();
        }


        [HttpPost]
        public IActionResult Delete(Guid id)
        {
            unitOfWork.Customers.Remove(id);
            unitOfWork.Save();
            return Ok();
        }


        [HttpPost]
        public IActionResult Create(Customer customer)
        {
            unitOfWork.Customers.Add(customer);
            unitOfWork.Save();
            return Ok();
        }
    }
}