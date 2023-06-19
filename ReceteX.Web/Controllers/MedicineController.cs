using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReceteX.Repository.Shared.Abstract;
using ReceteX.Repository.Shared.Concrete;
using ReceteX.Models;
using System.Xml;
using ReceteX.Utility;
using Microsoft.EntityFrameworkCore;

namespace ReceteX.Web.Controllers
{
    [Authorize]
    public class MedicineController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly XmlRetriever xmlRetriever;

        public MedicineController(IUnitOfWork unitOfWork, XmlRetriever xmlRetriever)
        {
            this.unitOfWork = unitOfWork;
            this.xmlRetriever = xmlRetriever;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ParseAndSaveFromXml(string xmlContent)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlContent);
            XmlNodeList medicinesFromXml = xmlDoc.SelectNodes("/ilaclar/ilac");

            IQueryable<Medicine> medicinesFromDbDeleted = unitOfWork.Medicines.GetAllDeleted().AsNoTracking().OrderBy(m => m.Name).ToList().AsQueryable<Medicine>();

            IQueryable<Medicine> medicinesFromDb = unitOfWork.Medicines.GetAll().AsNoTracking().OrderBy(m => m.Name).ToList().AsQueryable<Medicine>();

            foreach (XmlNode medicine in medicinesFromXml)
            {
                string barcode = medicine.SelectSingleNode("barkod").InnerText;

                if (!medicinesFromDb.Any(m => m.Barcode == barcode))
                {
                    Medicine med = new Medicine();
                    med.Name = medicine.SelectSingleNode("ad").InnerText;
                    med.Barcode = medicine.SelectSingleNode("barkod").InnerText;
                    unitOfWork.Medicines.Add(med);
                }
                else if (medicinesFromDbDeleted.Any(x => x.Barcode == barcode))
                {
                    Medicine medSilinmis = medicinesFromDbDeleted.FirstOrDefault(x => x.Barcode == barcode);

                    if (medSilinmis != null)
                    {
                        medSilinmis.isDeleted = false;
                        unitOfWork.Medicines.Update(medSilinmis);
                    }
                }
            }
            unitOfWork.Save();


            IEnumerable<XmlNode> medicinesFromXmlEnumerable = xmlDoc.SelectNodes("/ilaclar/ilac").Cast<XmlNode>();
            foreach (var medicine in medicinesFromDb)
            {
                if (!medicinesFromXmlEnumerable.Any(x => x.SelectSingleNode("barkod").InnerText == medicine.Barcode))
                {
                    medicine.isDeleted = true;
                    unitOfWork.Medicines.Update(medicine);
                }


            }
            unitOfWork.Save();
            return Ok();
        }

        public async Task<IActionResult> UpdateMedicinesList()
        {
            string content = await xmlRetriever.GetXmlContent("http://www.ibys.com.tr/exe/ilaclar.xml");
            await ParseAndSaveFromXml(content);

            return RedirectToAction("Index");
        }

        public IActionResult GetAll()
        {
            return Json(new { data = unitOfWork.Medicines.GetAll() });
        }

        

    }
}
