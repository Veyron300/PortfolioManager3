using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioManager3.InstrumentDataFactory
{
    interface ISourceInstrumentQuoteParser<T>
    {
        IEnumerable<T> ParseSourceQuotes();
    }
}
