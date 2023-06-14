using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReceteX.Models;
using ReceteX.Repository.Shared.Abstract;
using ReceteX.Repository.Shared.Concrete;
using ReceteX.Utility;
using System.Data;
using System.Linq;
using System.Xml;

namespace ReceteX.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DiagnosisController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly XmlRetriever xmlRetriever;

        public DiagnosisController(IUnitOfWork unitOfWork, XmlRetriever xmlRetriever)
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
            XmlNodeList diagnoses = xmlDoc.SelectNodes("/tanilar/tani");

            foreach (XmlNode diagnosis in diagnoses)
            {
                Diagnosis dia = new Diagnosis();

                dia.Code = diagnosis.SelectSingleNode("kod").InnerText;
                dia.Name = diagnosis.SelectSingleNode("ad").InnerText;
                unitOfWork.Diagnoses.Add(dia);
            }
            unitOfWork.Save();
            return Ok();
        }
        public async Task<IActionResult> UpdateDiagnosesList()
        {
            string content = await xmlRetriever.GetXmlContent("http://www.ibys.com.tr/exe/tanilar.xml");
            await ParseAndSaveFromXml(content);

            return RedirectToAction("Index");
        }

        public IActionResult GetAll()
        {
            return Json(new { data = unitOfWork.Diagnoses.GetAll() });
        }

    }
}
