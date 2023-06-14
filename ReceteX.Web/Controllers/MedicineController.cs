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
using System.Xml.Linq;
using ReceteX.Utility;

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
            XmlNodeList medicines = xmlDoc.SelectNodes("/ilaclar/ilac");

            foreach (XmlNode medicine in medicines)
            {
                Medicine med = new Medicine();

                med.Name = medicine.SelectSingleNode("ad").InnerText;
                med.Barcode = medicine.SelectSingleNode("barkod").InnerText;
                unitOfWork.Medicines.Add(med);
            }
            unitOfWork.Save();
            return Ok();



            //List<Medicine> allMedicines = unitOfWork.Medicines.GetAll().ToList<Medicine>();
            //unitOfWork.Medicines.ClearRange(allMedicines);
            //unitOfWork.Save();
            //List<Medicine> medicines = new List<Medicine>();
            //string xmlUrl = "http://www.ibys.com.tr/exe/ilaclar.xml";
            //using (HttpClient httpClient = new HttpClient())
            //{
            //    string xmlData = await httpClient.GetStringAsync(xmlUrl);
            //    XmlDocument xmlDocument = new XmlDocument();
            //    xmlDocument.LoadXml(xmlData);

            //    foreach (XmlNode xml in xmlDocument.SelectNodes("/ilaclar/ilac"))
            //    {

            //        medicines.Add(new Medicine
            //        {
            //            Barcode = xml["barkod"]?.InnerText,
            //            Name = xml["ad"]?.InnerText
            //        });
            //    }
            //    unitOfWork.Medicines.AddRange(medicines);
            //    unitOfWork.Save();
            //}
            //return RedirectToAction("Index");
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
