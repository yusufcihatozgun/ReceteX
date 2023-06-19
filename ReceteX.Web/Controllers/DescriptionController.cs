using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReceteX.Models;
using ReceteX.Repository.Shared.Abstract;
using ReceteX.Repository.Shared.Concrete;

namespace ReceteX.Web.Controllers
{
	public class DescriptionController : Controller
	{
		private readonly IUnitOfWork unitOfWork;

		public DescriptionController(IUnitOfWork unitOfWork = null)
		{
			this.unitOfWork = unitOfWork;
		}

		[HttpPost]
		public IActionResult AddDescription(Guid prescriptionId, Description description)
		{
			Prescription asil = unitOfWork.Prescriptions.GetAll(p => p.Id == prescriptionId).Include(p => p.Descriptions).First();
			
			asil.Descriptions.Add(description);

			unitOfWork.Prescriptions.Update(asil);
			unitOfWork.Save();

			return Ok();
		}

        [HttpPost]
        public IActionResult AddDescription(Description description)
        {
            
            unitOfWork.Descriptions.Update(description);
            unitOfWork.Save();

            return Ok();
        }

        [HttpPost]
        public IActionResult GetDescriptions(Guid prescriptionId)
        {
			return Json(unitOfWork.Descriptions)
        }
}
