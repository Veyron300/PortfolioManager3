using IMA.API.LocalData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioManager3.InstrumentDataFactory
{
    public class SourceInstrumentMSLocalParser : ISourceInstrumentQuoteParser<SecurityDataAndMetadata>
    {
        IDataManager dataManager = DataManagerFactory.GetDataManager();
        private readonly ISourceInstrumentMetadataLoader _sourceMetaLoader;

        public SourceInstrumentMSLocalParser(ISourceInstrumentMetadataLoader sourceMetaLoader)
        {
            _sourceMetaLoader = sourceMetaLoader;
        }


        public IEnumerable<SecurityDataAndMetadata> ParseSourceQuotes()
        {
            string repo = _sourceMetaLoader.LoadSourceInstrumentMetadata().RepositoryPath;
            string symbol = _sourceMetaLoader.LoadSourceInstrumentMetadata().Symbol;
            RepositoryId repoId = new RepositoryId(repo);
            SecurityId securityId = new SecurityId(symbol, Interval.Daily, repoId);
            int range = _sourceMetaLoader.LoadSourceInstrumentMetadata().Range;
            SecurityDataAndMetadata dataRecords = dataManager.GetDataAndMetadataAsync(securityId, RequestRange.LastNPoints(range)).Result;
            yield return dataRecords;
        }
    }
}
