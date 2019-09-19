using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioManager3.InstrumentDataFactory
{/// <summary>
/// Class for parsing CSV data
/// </summary>
    public class SourceInstrumentCSVParser : ISourceInstrumentQuoteParser<SourceInstrumentQuote>
    {
        private ISourceInstrumentMetadataLoader _sourcemetadataloader;

        public SourceInstrumentCSVParser(ISourceInstrumentMetadataLoader sourcemetadataloader)
        {
            _sourcemetadataloader = sourcemetadataloader;
        }

        public IEnumerable<SourceInstrumentQuote> ParseSourceQuotes()
        {
            var query=
                from line in File.ReadAllLines(_sourcemetadataloader.LoadSourceInstrumentMetadata().RepositoryPath).Skip(1)
                let data = line.Split(',')
                where data[0].Length > 0
                select new SourceInstrumentQuote()
                {

                    Date = DateTime.Parse(data[0]),
                    Close = decimal.Parse(data[4]),

                };

            return query.ToList();
        }
    }
}
