using System;

namespace PortfolioManager3.XmlRepository
{
    /// <summary>
    /// Sets the location of the Portfolios database XML file
    /// </summary>
    public static class XmlRepositoryConfig
    {
        
         public static string XmlFile { get; set; } = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\\Portfolios\\XMLFile.xml";  

    }
}
