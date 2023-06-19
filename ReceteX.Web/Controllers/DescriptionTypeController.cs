using Microsoft.AspNetCore.Mvc;
using ReceteX.Models;
using ReceteX.Repository.Shared.Abstract;
using ReceteX.Repository.Shared.Concrete;

namespace ReceteX.Web.Controllers
{
	public class DescriptionTypeController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;

		public DescriptionTypeController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public IActionResult GetAll()
		{
			return Json(_unitOfWork.DescriptionTypes.GetAll().OrderBy(m => m.RemoteId));
		}

    }
}
