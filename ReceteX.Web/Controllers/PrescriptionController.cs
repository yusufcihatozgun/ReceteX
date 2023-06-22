using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReceteX.Models;
using ReceteX.Repository.Shared.Abstract;
using ReceteX.Utility;
using System.Security.Claims;

namespace ReceteX.Web.Controllers
{
    [Authorize]
    public class PrescriptionController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly MyXmlWriter myXmlWriter;

        public PrescriptionController(IUnitOfWork unitOfWork, MyXmlWriter myXmlWriter)
        {
            this.unitOfWork = unitOfWork;
            this.myXmlWriter = myXmlWriter;
        }


        public IActionResult Index(Guid? prescriptionId = null)
        {
            if (prescriptionId == null)
            {
                Prescription prescription = new Prescription();
                prescription.StatusId = unitOfWork.Statuses.GetFirstOrDefault(s => s.Name == "Taslak").Id;
                prescription.AppUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                unitOfWork.Prescriptions.Add(prescription);
                unitOfWork.Save();
                return View(prescription);
            }
            else
            {
                Prescription prescription = unitOfWork.Prescriptions.GetFirstOrDefault(p => p.Id == prescriptionId);
                return View(prescription);
            }
        }
        public IActionResult ReceteArsivi()
        {
            return View();
        }

        public IActionResult GetAllWithStatuses(Guid? statusId = null)
        {
            int pageSize = 10;
            int currentPage = 2;

            List<Prescription> prescriptions = null;

            if (statusId == null || statusId == Guid.Parse("fb899913-c32d-4f1e-9f16-73ecf891def1"))
            {
                prescriptions = unitOfWork.Prescriptions
                    .GetAll(p => p.StatusId == Guid.Parse("fb899913-c32d-4f1e-9f16-73ecf891def1"))
                    .Include(p => p.Status)
                    .OrderByDescending(p => p.DateCreated)
                                        .ToList();
            }
            else if (statusId == Guid.Parse("dd4edb5c-4cac-4fa4-b52a-cd08817c3c0c"))
            {
                prescriptions = unitOfWork.Prescriptions
                    .GetAll(p => p.StatusId == statusId)
                    .Include(p => p.Status)
                    .OrderByDescending(p => p.DateCreated)
                                        .ToList();
            }
            else if (statusId == Guid.Parse("69416e64-c670-4d1c-a792-eabc0510f5fd"))
            {
                prescriptions = unitOfWork.Prescriptions
                    .GetAll(p => p.StatusId == statusId)
                    .Include(p => p.Status)
                    .OrderByDescending(p => p.DateCreated)
                    .ToList();
            }

            return Ok(prescriptions);
        }


        [HttpPost]
        public IActionResult AddDiagnosis(Guid prescriptionId, Guid diagnosisId)
        {
            Prescription asil = unitOfWork.Prescriptions.GetAll(p => p.Id == prescriptionId).Include(p => p.Diagnoses).First();
            Diagnosis asilDiagnosis = unitOfWork.Diagnoses.GetFirstOrDefault(d => d.Id == diagnosisId);
            asil.Diagnoses.Add(asilDiagnosis);
            unitOfWork.Prescriptions.Update(asil);
            unitOfWork.Save();
            return Ok();
        }


        [HttpPost]
        public IActionResult GetDiagnoses(Guid prescriptionId)
        {
            return Json(unitOfWork.Prescriptions.GetAll(p => p.Id == prescriptionId).Include(p => p.Diagnoses));
        }

        [HttpPost]
        public IActionResult GetPatient(Guid prescriptionId)
        {
            return Json(unitOfWork.Prescriptions.GetFirstOrDefault(p => p.Id == prescriptionId));
        }


        [HttpPost]
        public IActionResult GetMedicines(Guid prescriptionId)
        {
            return Json(unitOfWork.Prescriptions.GetAll(p => p.Id == prescriptionId).Include(p => p.PrescriptionMedicines).ThenInclude(m => m.Medicine).Include(p => p.PrescriptionMedicines).ThenInclude(p => p.MedicineUsagePeriod).Include(p => p.PrescriptionMedicines).ThenInclude(p => p.MedicineUsageType).First());

        }


        [HttpPost]
        public IActionResult AddDescription(Description description)
        {
            unitOfWork.Descriptions.Add(description);
            unitOfWork.Save();
            return Ok();
        }


        [HttpPost]
        public JsonResult GetDescriptions(Guid prescriptionId)
        {
            return Json(unitOfWork.Descriptions.GetAll(d => d.PrescriptionId == prescriptionId).Include(d => d.DescriptionType));
        }


        [HttpPost]
        public IActionResult AddMedicine(Guid prescriptionId, PrescriptionMedicine prescriptionMedicine)
        {
            Prescription asil = unitOfWork.Prescriptions.GetAll(p => p.Id == prescriptionId).Include(p => p.PrescriptionMedicines).First();
            asil.PrescriptionMedicines.Add(prescriptionMedicine);
            unitOfWork.Prescriptions.Update(asil);
            unitOfWork.Save();
            return Ok();
        }


        [HttpPost]
        public IActionResult RemoveMedicine(Guid prescriptionId, Guid prescriptionMedicineId)
        {
            Prescription asil = unitOfWork.Prescriptions.GetAll(p => p.Id == prescriptionId).Include(p => p.PrescriptionMedicines).First();
            asil.PrescriptionMedicines.Remove(asil.PrescriptionMedicines.FirstOrDefault(pm => pm.Id == prescriptionMedicineId));
            unitOfWork.Prescriptions.Update(asil);
            unitOfWork.Save();
            return Ok();
        }


        [HttpPost]
        public IActionResult RemoveDescription(Guid descriptionId)
        {
            unitOfWork.Descriptions.Remove(descriptionId);
            unitOfWork.Save();
            return Ok();
        }


        [HttpPost]
        public IActionResult RemoveDiagnosis(Guid prescriptionId, Guid diagnosisId)
        {
            Prescription asil = unitOfWork.Prescriptions.GetAll(p => p.Id == prescriptionId).Include(p => p.Diagnoses).First();
            asil.Diagnoses.Remove(asil.Diagnoses.FirstOrDefault(d => d.Id == diagnosisId));
            unitOfWork.Prescriptions.Update(asil);
            unitOfWork.Save();
            return Ok();
        }

        public async Task<IActionResult> PrintXmlDocument(Guid prescriptionId)
        {
            Guid asilPrescriptionId = unitOfWork.Prescriptions.GetFirstOrDefault(p => p.Id == prescriptionId).Id;
            await myXmlWriter.WriteXml(asilPrescriptionId);
            return Ok();
        }
        [HttpPost]
        public IActionResult AddPatient(Guid prescriptionId, string patientTCK, string patientGSM)
        {
            Prescription asil = unitOfWork.Prescriptions.GetAll(p => p.Id == prescriptionId).First();
            asil.TCKN = patientTCK;
            asil.PatientGsm = patientGSM;
            unitOfWork.Prescriptions.Update(asil);
            unitOfWork.Save();
            return Ok();
        }

    }
}
