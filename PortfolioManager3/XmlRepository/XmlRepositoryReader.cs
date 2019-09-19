using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;

namespace PortfolioManager3.XmlRepository
{
    /// <summary>
    /// Loads portfolio and position data from XML file
    /// </summary>
    public class XmlRepositoryReader
    {
        public ObservableCollection<Portfolio> LoadPortfolios()
        {
            
            ObservableCollection<Portfolio> loadedPortfolios = new ObservableCollection<Portfolio>();
            XDocument xd = XDocument.Load(XmlRepositoryConfig.XmlFile);
            IList<XElement> xList =  xd.XPathSelectElements("//Portfolio").ToList();

            foreach (XElement xe in xList)
            {
                Portfolio loadedPortfolio = new Portfolio();
                loadedPortfolio.PortfolioName = xe.Element("PortfolioName").Value;
                loadedPortfolio.BenchmarkInstrument = xe.Element("BenchmarkInstrument").Value;
                loadedPortfolio.InceptionDate = DateTime.Parse(xe.Element("InceptionDate").Value); 
                loadedPortfolios.Add(loadedPortfolio);
            }

            return loadedPortfolios;
        }

        public ObservableCollection<Position> LoadPositions(Portfolio selectedPortfolio)
        {
            ObservableCollection<Position> loadedPositions = new ObservableCollection<Position>();
            XDocument PortfolioRepository = XDocument.Load(XmlRepositoryConfig.XmlFile);
            XElement selectedPort = (from p in PortfolioRepository.Descendants("Portfolio")
                                     where p.Element("PortfolioName").Value == selectedPortfolio.PortfolioName
                                     select p).SingleOrDefault();

            IList<XElement> positionsList = selectedPort.XPathSelectElements(".//Position").ToList();

            foreach (XElement xe in positionsList)
            {
                Position loadedPosition = new Position();
                loadedPosition.Ticker = xe.Element("Ticker").Value;
                loadedPosition.Units = decimal.Parse(xe.Element("Units").Value);
                loadedPosition.EntryDate = DateTime.Parse(xe.Element("EntryDate").Value);
                loadedPosition.EntryPrice = decimal.Parse(xe.Element("EntryPrice").Value);
                loadedPosition.InstrumentRepositoryPath = (xe.Element("InstrumentRepositoryPath").Value);
                loadedPosition.InstrumentFileType = (xe.Element("InstrumentFileType").Value);
                loadedPositions.Add(loadedPosition);
            }
            return loadedPositions;
          
        }
    }
}
