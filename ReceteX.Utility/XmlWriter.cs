using System.Xml;

namespace ReceteX.Utility
{
    public class XmlWriter
    {
        public void WriteXml()
        {
            XmlDocument xmlDoc = new XmlDocument();

            XmlNode ereceteBilgisiNode = xmlDoc.CreateElement("ereceteBilgisi");
            xmlDoc.AppendChild(ereceteBilgisiNode);

            XmlNode doktorBransKoduNode = xmlDoc.CreateElement("doktorBransKodu");
            doktorBransKoduNode.InnerText = "";
            ereceteBilgisiNode.AppendChild(doktorBransKoduNode);

            XmlNode doktorTcKimlikNoNode = xmlDoc.CreateElement("doktorTcKimlikNo");
            doktorTcKimlikNoNode.InnerText = "";
            ereceteBilgisiNode.AppendChild(doktorTcKimlikNoNode);

            XmlNode doktorSertifikaKoduNode = xmlDoc.CreateElement("doktorSertifikaKodu");
            doktorSertifikaKoduNode.InnerText = "";
            ereceteBilgisiNode.AppendChild(doktorSertifikaKoduNode);

            XmlNode protokolNoNode = xmlDoc.CreateElement("protokolNo");
            protokolNoNode.InnerText = "";
            ereceteBilgisiNode.AppendChild(protokolNoNode);

            XmlNode provizyonTipiNode = xmlDoc.CreateElement("provizyonTipi");
            provizyonTipiNode.InnerText = "";
            ereceteBilgisiNode.AppendChild(provizyonTipiNode);

            XmlNode receteAltTuruNode = xmlDoc.CreateElement("receteAltTuru");
            receteAltTuruNode.InnerText = "";
            ereceteBilgisiNode.AppendChild(receteAltTuruNode);

            XmlNode receteTarihiNode = xmlDoc.CreateElement("receteTarihi");
            receteTarihiNode.InnerText = "";
            ereceteBilgisiNode.AppendChild(receteTarihiNode);

            XmlNode receteTuruNode = xmlDoc.CreateElement("receteTuru");
            receteTuruNode.InnerText = "";
            ereceteBilgisiNode.AppendChild(receteTuruNode);

            XmlNode takipNoNode = xmlDoc.CreateElement("takipNo");
            takipNoNode.InnerText = "";
            ereceteBilgisiNode.AppendChild(takipNoNode);

            XmlNode tcKimlikNoNode = xmlDoc.CreateElement("tcKimlikNo");
            tcKimlikNoNode.InnerText = "";
            ereceteBilgisiNode.AppendChild(tcKimlikNoNode);

            XmlNode tesisKoduNode = xmlDoc.CreateElement("tesisKodu");
            tesisKoduNode.InnerText = "";
            ereceteBilgisiNode.AppendChild(tesisKoduNode);

            XmlNode doktorAdiNode = xmlDoc.CreateElement("doktorAdi");
            doktorAdiNode.InnerText = "";
            ereceteBilgisiNode.AppendChild(doktorAdiNode);

            XmlNode doktorSoyadiNode = xmlDoc.CreateElement("doktorSoyadi");
            doktorSoyadiNode.InnerText = "";
            ereceteBilgisiNode.AppendChild(doktorSoyadiNode);

            XmlNode ereceteNoNode = xmlDoc.CreateElement("ereceteNo");
            ereceteNoNode.InnerText = "";
            ereceteBilgisiNode.AppendChild(ereceteNoNode);

            // İlaç bilgilerini içeren düğümler
            XmlNode ereceteIlacBilgisiNode = xmlDoc.CreateElement("ereceteIlacBilgisi");
            ereceteBilgisiNode.AppendChild(ereceteIlacBilgisiNode);

            // İlaç bilgilerini döngü ile ekleyelim
            for (int i = 0; i < ilacBilgileri.Count; i++)
            {
                XmlNode ilacNode = xmlDoc.CreateElement("ilac");

                XmlNode adetNode = xmlDoc.CreateElement("adet");
                adetNode.InnerText = ilacBilgileri[i].Adet;
                ilacNode.AppendChild(adetNode);

                XmlNode barkodNode = xmlDoc.CreateElement("barkod");
                barkodNode.InnerText = ilacBilgileri[i].Barkod;
                ilacNode.AppendChild(barkodNode);

                XmlNode kullanimDoz1Node = xmlDoc.CreateElement("kullanimDoz1");
                kullanimDoz1Node.InnerText = ilacBilgileri[i].KullanimDoz1;
                ilacNode.AppendChild(kullanimDoz1Node);

                XmlNode kullanimDoz2Node = xmlDoc.CreateElement("kullanimDoz2");
                kullanimDoz2Node.InnerText = ilacBilgileri[i].KullanimDoz2;
                ilacNode.AppendChild(kullanimDoz2Node);

                XmlNode kullanimPeriyotNode = xmlDoc.CreateElement("kullanimPeriyot");
                kullanimPeriyotNode.InnerText = ilacBilgileri[i].KullanimPeriyot;
                ilacNode.AppendChild(kullanimPeriyotNode);

                XmlNode kullanimPeriyotBirimiNode = xmlDoc.CreateElement("kullanimPeriyotBirimi");
                kullanimPeriyotBirimiNode.InnerText = ilacBilgileri[i].KullanimPeriyotBirimi;
                ilacNode.AppendChild(kullanimPeriyotBirimiNode);

                ereceteIlacBilgisiNode.AppendChild(ilacNode);
            }

            // Tanı bilgilerini içeren düğümler
            XmlNode tanıBilgisiNode = xmlDoc.CreateElement("tanıBilgisi");
            ereceteBilgisiNode.AppendChild(tanıBilgisiNode);

            // Tanı bilgilerini döngü ile ekleyelim
            for (int i = 0; i < tanıBilgileri.Count; i++)
            {
                XmlNode tanıNode = xmlDoc.CreateElement("tanı");

                XmlNode tanıKoduNode = xmlDoc.CreateElement("tanıKodu");
                tanıKoduNode.InnerText = tanıBilgileri[i].TanıKodu;
                tanıNode.AppendChild(tanıKoduNode);
            }

            XmlNode kisiDVONode = xmlDoc.CreateElement("kisiDVO");
            ereceteBilgisiNode.AppendChild(kisiDVONode);

            // kisiDVO düğümünü doldur
            XmlNode adNode = xmlDoc.CreateElement("ad");
            adNode.InnerText = kisiDVO.Ad;
            kisiDVONode.AppendChild(adNode);

            XmlNode soyadNode = xmlDoc.CreateElement("soyad");
            soyadNode.InnerText = kisiDVO.Soyad;
            kisiDVONode.AppendChild(soyadNode);

            XmlNode tcKimlikNoNode = xmlDoc.CreateElement("tcKimlikNo");
            tcKimlikNoNode.InnerText = kisiDVO.TcKimlikNo;
            kisiDVONode.AppendChild(tcKimlikNoNode);

            // İlaç bilgilerini içeren düğümler
            XmlNode ereceteIlacBilgisiNode = xmlDoc.CreateElement("ereceteIlacBilgisi");
            ereceteBilgisiNode.AppendChild(ereceteIlacBilgisiNode);
            // XML dosyasını kaydet
            xmlDoc.Save(@"recete.xml");
        }
    }
}
