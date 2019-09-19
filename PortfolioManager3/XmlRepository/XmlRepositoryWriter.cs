using System;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using System.IO;

namespace PortfolioManager3.XmlRepository
{
    /// <summary>
    /// Writes portfolio and position data to XML file
    /// </summary>
    public class XmlRepositoryWriter
    {

        internal XDocument PortfoliosXDocument { get; set; }

        internal XElement PortfolioRepository { get; set; } 

        private XElement root;

        private XElement parent;

        private XElement child;

        public XmlRepositoryWriter()
        {

            Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Portfolios"));

            if (!File.Exists(XmlRepositoryConfig.XmlFile))
            {
                CreatePortfolioXMLFile();
            }


            PortfolioRepository = XElement.Load(XmlRepositoryConfig.XmlFile);
        }

        public void CreatePortfolioXMLFile()
        {
            
            PortfoliosXDocument = new XDocument(new XDeclaration("1.0", "utf-8", "yes"),
                        new XComment("Portfolios"),
                        new XElement("Portfolios"));
            root = PortfoliosXDocument.XPathSelectElement("//Portfolios");
            PortfoliosXDocument.Save(XmlRepositoryConfig.XmlFile);
        }
        
        public void WritePortfolio(string portfolioname, string benchmarkinstrument, DateTime? inceptiondate)
        {
            parent = new XElement("Portfolio",
                    new XElement("PortfolioName", portfolioname),
                    new XElement("BenchmarkInstrument", benchmarkinstrument),
                    new XElement("InceptionDate", inceptiondate));
            PortfolioRepository.Add(parent);
            PortfolioRepository.Save(XmlRepositoryConfig.XmlFile);
        }

        public void RemovePortfolio(Portfolio selectedportfolio)
        {
            XElement port = (from p in PortfolioRepository.Descendants("Portfolio")
                             where p.Element("PortfolioName").Value == selectedportfolio.PortfolioName
                             select p).SingleOrDefault();
            if (port != null)
            {
                port.Remove();
                PortfolioRepository.Save(XmlRepositoryConfig.XmlFile);
            }
        }

        public void WritePosition(Portfolio selectedPortfolio, string ticker, decimal units, DateTime? entryDate, decimal entryPrice, string repositoryPath, string fileType)
        {
            PortfolioRepository = XElement.Load(XmlRepositoryConfig.XmlFile);

            child = new XElement("Position",
                new XElement("Ticker", ticker),
                new XElement("Units", units),
                new XElement("EntryDate", entryDate),
                new XElement("EntryPrice", entryPrice),
                new XElement("InstrumentRepositoryPath", repositoryPath),
                new XElement("InstrumentFileType", fileType));


            XElement port = (from p in PortfolioRepository.Descendants("Portfolio")
                        where p.Element("PortfolioName").Value == selectedPortfolio.PortfolioName
                        select p).SingleOrDefault();
            port.Add(child);
            PortfolioRepository.Save(XmlRepositoryConfig.XmlFile);
        }

    }
}
