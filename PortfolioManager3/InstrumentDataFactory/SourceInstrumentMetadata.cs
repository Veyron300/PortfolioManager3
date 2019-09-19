using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioManager3.InstrumentDataFactory
{/// <summary>
/// Information about a source instrument file
/// </summary>
    public class SourceInstrumentMetadata
    {
        public string RepositoryPath { get; set; }
        public string Symbol { get; set; }
        public int Range { get; set; }
        public string FileType { get; set; }
    }
}
