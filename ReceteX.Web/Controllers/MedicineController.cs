using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReceteX.Repository.Shared.Abstract;
using ReceteX.Repository.Shared.Concrete;
using ReceteX.Models;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ReceteX.Web.Controllers
{
    [Authorize]
    public class MedicineController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<MedicineController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public MedicineController(ILogger<MedicineController> logger, IWebHostEnvironment webHostEnvironment, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Update()
        {
            List<Medicine> allMedicines = _unitOfWork.Medicines.GetAll().ToList<Medicine>();
            _unitOfWork.Medicines.ClearRange(allMedicines);
            _unitOfWork.Save();

            List<Medicine> medicines = new List<Medicine>();

            string xmlUrl = "http://www.ibys.com.tr/exe/ilaclar.xml";

            using (HttpClient httpClient = new HttpClient())
            {
                try
                {
                    string xmlData = await httpClient.GetStringAsync(xmlUrl);

                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.LoadXml(xmlData);

                    foreach (XmlNode xml in xmlDocument.SelectNodes("/ilaclar/ilac"))
                    {
                        
                            medicines.Add(new Medicine
                            {
                                Barcode = xml["barkod"]?.InnerText,
                                Name = xml["ad"]?.InnerText
                            });                        
                    }
                    _unitOfWork.Medicines.AddRange(medicines);
                    _unitOfWork.Save();
                }
                catch (Exception ex)
                {
                    // Handle the exception appropriately
                    // You can log the exception or display an error message
                    _logger.LogError(ex, "Error occurred while loading the XML data from the URL.");
                }
            }

            return RedirectToAction("Index");
        }

        public IActionResult GetAll()
        {
            return Json(new { data = _unitOfWork.Medicines.GetAll() });
        }
    }
}
