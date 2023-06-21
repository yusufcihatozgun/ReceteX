using Microsoft.EntityFrameworkCore;
using ReceteX.Models;
using ReceteX.Repository.Shared.Abstract;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace ReceteX.Utility
{
	public class MyXmlWriter
	{
		private readonly IUnitOfWork unitOfWork;

		public MyXmlWriter(IUnitOfWork unitOfWork)
		{
			this.unitOfWork = unitOfWork;
		}


        public async Task<XmlDocument> WriteXml(Guid prescriptionId)
		{
			Prescription prescription = unitOfWork.Prescriptions.GetAll(p => p.Id == prescriptionId).Include(p => p.Diagnoses).Include(p => p.AppUser).Include(p => p.PrescriptionMedicines).ThenInclude(pm=>pm.MedicineUsagePeriod).Include(p => p.PrescriptionMedicines).ThenInclude(pm=>pm.Medicine).Include(p => p.PrescriptionMedicines).ThenInclude(pm=>pm.MedicineUsageType).First();
			List<PrescriptionMedicine> medicines = prescription.PrescriptionMedicines.ToList<PrescriptionMedicine>();
			List<Diagnosis> diagnoses = prescription.Diagnoses.ToList<Diagnosis>();


			XmlDocument xmlDoc = new XmlDocument();

            // XmlDeclaration oluşturulması
            XmlDeclaration xmlDeclaration = xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", "yes");
            xmlDoc.AppendChild(xmlDeclaration);

            XmlNode ereceteBilgisiNode = xmlDoc.CreateElement("ereceteBilgisi");
			xmlDoc.AppendChild(ereceteBilgisiNode);

			XmlNode doktorBransKoduNode = xmlDoc.CreateElement("doktorBransKodu");
			doktorBransKoduNode.InnerText = "9999";
			ereceteBilgisiNode.AppendChild(doktorBransKoduNode);

			XmlNode doktorTcKimlikNoNode = xmlDoc.CreateElement("doktorTcKimlikNo");
			doktorTcKimlikNoNode.InnerText = prescription.AppUser.TCKN.ToString();
			ereceteBilgisiNode.AppendChild(doktorTcKimlikNoNode);

			XmlNode doktorSertifikaKoduNode = xmlDoc.CreateElement("doktorSertifikaKodu");
			doktorSertifikaKoduNode.InnerText = "İH-96854";
			ereceteBilgisiNode.AppendChild(doktorSertifikaKoduNode);

			XmlNode protokolNoNode = xmlDoc.CreateElement("protokolNo");
			protokolNoNode.InnerText = "20.5.1982 00:00:00";
			ereceteBilgisiNode.AppendChild(protokolNoNode);

			XmlNode provizyonTipiNode = xmlDoc.CreateElement("provizyonTipi");
			provizyonTipiNode.InnerText = "28018";
			ereceteBilgisiNode.AppendChild(provizyonTipiNode);

			XmlNode receteAltTuruNode = xmlDoc.CreateElement("receteAltTuru");
			receteAltTuruNode.InnerText = "1";
			ereceteBilgisiNode.AppendChild(receteAltTuruNode);

			XmlNode receteTarihiNode = xmlDoc.CreateElement("receteTarihi");
			receteTarihiNode.InnerText = DateOnly.FromDateTime(DateTime.Now).ToString();
			ereceteBilgisiNode.AppendChild(receteTarihiNode);

			XmlNode receteTuruNode = xmlDoc.CreateElement("receteTuru");
			receteTuruNode.InnerText = "1";
			ereceteBilgisiNode.AppendChild(receteTuruNode);

			XmlNode takipNoNode = xmlDoc.CreateElement("takipNo");
			takipNoNode.InnerText = "0";
			ereceteBilgisiNode.AppendChild(takipNoNode);


			//hasta tc no
			XmlNode tcKimlikNoNode = xmlDoc.CreateElement("tcKimlikNo");
			tcKimlikNoNode.InnerText = prescription.AppUser.TCKN.ToString();
			ereceteBilgisiNode.AppendChild(tcKimlikNoNode);

			XmlNode tesisKoduNode = xmlDoc.CreateElement("tesisKodu");
			tesisKoduNode.InnerText = "11349903";
			ereceteBilgisiNode.AppendChild(tesisKoduNode);

			XmlNode doktorAdiNode = xmlDoc.CreateElement("doktorAdi");
			doktorAdiNode.InnerText = prescription.AppUser.Name;
			ereceteBilgisiNode.AppendChild(doktorAdiNode);

			XmlNode doktorSoyadiNode = xmlDoc.CreateElement("doktorSoyadi");
			doktorSoyadiNode.InnerText = prescription.AppUser.Surname;
			ereceteBilgisiNode.AppendChild(doktorSoyadiNode);

			XmlNode ereceteNoNode = xmlDoc.CreateElement("ereceteNo");
			ereceteNoNode.InnerText = "0";
			ereceteBilgisiNode.AppendChild(ereceteNoNode);

			// İlaç bilgilerini içeren düğümler
			XmlNode ereceteIlacBilgisiNode = xmlDoc.CreateElement("ereceteIlacBilgisi");
			ereceteBilgisiNode.AppendChild(ereceteIlacBilgisiNode);

			// İlaç bilgilerini döngü ile ekleyelim
			for (int i = 0; i < medicines.Count; i++)
			{

				XmlNode adetNode = xmlDoc.CreateElement("adet");
				adetNode.InnerText = medicines[i].Quantity.ToString();
				ereceteIlacBilgisiNode.AppendChild(adetNode);

				XmlNode barkodNode = xmlDoc.CreateElement("barkod");
				barkodNode.InnerText = medicines[i].Medicine.Barcode.ToString();
				ereceteIlacBilgisiNode.AppendChild(barkodNode);

				XmlNode kullanimDoz1Node = xmlDoc.CreateElement("kullanimDoz1");
				kullanimDoz1Node.InnerText = medicines[i].Dose1.ToString();
				ereceteIlacBilgisiNode.AppendChild(kullanimDoz1Node);

				XmlNode kullanimDoz2Node = xmlDoc.CreateElement("kullanimDoz2");
				kullanimDoz2Node.InnerText = medicines[i].Dose2.ToString();
				ereceteIlacBilgisiNode.AppendChild(kullanimDoz2Node);

				XmlNode kullanimPeriyotNode = xmlDoc.CreateElement("kullanimPeriyot");
				kullanimPeriyotNode.InnerText = medicines[i].MedicineUsagePeriod.ToString();
				ereceteIlacBilgisiNode.AppendChild(kullanimPeriyotNode);

				XmlNode kullanimPeriyotBirimiNode = xmlDoc.CreateElement("kullanimPeriyotBirimi");
				kullanimPeriyotBirimiNode.InnerText = medicines[i].Period.ToString();
				ereceteIlacBilgisiNode.AppendChild(kullanimPeriyotBirimiNode);

				XmlNode kullanimSekliNode = xmlDoc.CreateElement("kullanimSekli");
				kullanimSekliNode.InnerText = medicines[i].MedicineUsageType.ToString();
				ereceteIlacBilgisiNode.AppendChild(kullanimSekliNode);

			}

			// Tanı bilgilerini içeren düğümler
			XmlNode ereceteTaniBilgisiNode = xmlDoc.CreateElement("ereceteTaniBilgisi");
			ereceteBilgisiNode.AppendChild(ereceteTaniBilgisiNode);

			// Tanı bilgilerini döngü ile ekleyelim
			for (int i = 0; i < diagnoses.Count; i++)
			{

				XmlNode taniKoduNode = xmlDoc.CreateElement("taniKodu");
				taniKoduNode.InnerText = diagnoses[i].Code.ToString();
				ereceteBilgisiNode.AppendChild(taniKoduNode);
			}

			XmlNode kisiDVONode = xmlDoc.CreateElement("kisiDVO");
			ereceteBilgisiNode.AppendChild(kisiDVONode);

			// kisiDVO düğümünü doldur
			XmlNode adiNode = xmlDoc.CreateElement("adi");
			adiNode.InnerText = prescription.PatientFirstName;
			kisiDVONode.AppendChild(adiNode);

			XmlNode soyadiNode = xmlDoc.CreateElement("soyadi");
			soyadiNode.InnerText = prescription.PatientLastName;
			kisiDVONode.AppendChild(soyadiNode);

			XmlNode cinsiyetiNode = xmlDoc.CreateElement("cinsiyeti");
			cinsiyetiNode.InnerText = prescription.Gender.ToString();
			kisiDVONode.AppendChild(cinsiyetiNode);

			XmlNode dogumTarihiNode = xmlDoc.CreateElement("dogumTarihi");
			dogumTarihiNode.InnerText = prescription.BirthDate.ToString();
			kisiDVONode.AppendChild(dogumTarihiNode);
			 
			XmlNode hastaTcKimlikNoNode = xmlDoc.CreateElement("tcKimlikNo");
			hastaTcKimlikNoNode.InnerText = prescription.TCKN.ToString();
			kisiDVONode.AppendChild(hastaTcKimlikNoNode);

			//if (!Directory.Exists(Directory.GetCurrentDirectory() + "/dosyalar/ImzalanacakRecete/"))
			//{
			//	Directory.CreateDirectory(Directory.GetCurrentDirectory() + "/dosyalar/ImzalanacakRecete/");
			//}

			//         string dosyaAdi = "recete_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xml";
			//string dosyaYolu = Directory.GetCurrentDirectory+"/dosyalar/İmzalanacakRecete/" + dosyaAdi;
			//xmlDoc.Save(dosyaYolu);

			string xmlString = 


			prescription.XmlToSign = xmlDoc.InnerXml.ToString();
			unitOfWork.Prescriptions.Update(prescription);
			unitOfWork.Save();

			byte[] bytes = Encoding.UTF8.GetBytes(xmlString);
            string base64String = Convert.ToBase64String(bytes);

            return xmlDoc;

        }
	}
}
