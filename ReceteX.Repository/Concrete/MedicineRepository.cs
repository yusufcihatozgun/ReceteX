using ReceteX.Data;
using ReceteX.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ReceteX.Repository.Concrete
{        //xml için. silinebilir.

    public class MedicineRepository
    {
        private readonly ApplicationDbContext _db;

        public MedicineRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task ParseAndSaveMedicinesFromXml(string xmlContent)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlContent);

            XmlNodeList ilacNodes = xmlDoc.SelectNodes("/ilaclar/ilac");

            foreach (XmlNode ilacNode in ilacNodes)
            {
                Medicine medicine = new Medicine();

                XmlNode barkodNode = ilacNode.SelectSingleNode("barkod");
                if (barkodNode != null)
                    medicine.Barcode = barkodNode.InnerText;

                XmlNode adNode = ilacNode.SelectSingleNode("ad");
                if (adNode != null)
                    medicine.Name = adNode.InnerText;

                _db.Medicines.Add(medicine);
            }

            await _db.SaveChangesAsync();
        }
    }
}
}
