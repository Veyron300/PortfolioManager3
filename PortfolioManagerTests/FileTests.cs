using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PortfolioManager3.XmlRepository;

namespace PortfolioManagerTests
{
    [TestClass]
    public class FileTests
    {


        [TestMethod]
        public void PortfoliosXMLFileExists()
        {
            //Arrange
            string portfolioXMLfile = XmlRepositoryConfig.XmlFile;

            //Act
            bool portfolioFileExists = File.Exists(portfolioXMLfile);

            //Assert
            Assert.IsTrue(portfolioFileExists);
        }

       
    }
}
