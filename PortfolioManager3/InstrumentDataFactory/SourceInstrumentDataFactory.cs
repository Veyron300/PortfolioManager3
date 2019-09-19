using System;
using System.Collections.Generic;
using System.Linq;
using IMA.API.LocalData;

namespace PortfolioManager3.InstrumentDataFactory
{
    /// <summary>
    /// Supplies instrument data to the PositionViewModel and AnalyticsViewmodel
    /// </summary>
    public class SourceInstrumentDataFactory : SourceInstrumentQuote
    {
        private static IEnumerable<SecurityDataAndMetadata> msLocalQuotes;
        private static IList<SourceInstrumentQuote> csvQuotes;
        private static IList<SourceInstrumentQuote> sourceInstrumentQuotes;
        private static ISourceInstrumentMetadataLoader msLocalMetaLoader;
        private static ISourceInstrumentMetadataLoader csvMetaLoader;

       

        /// <summary>
        /// Method for returning a collection of instrument data
        /// </summary>
        /// <param name="repoFolder"></param>
        /// <param name="ticker"></param>
        /// <param name="range"></param>
        /// <param name="sourceFileType"></param>
        /// <returns></returns>
        public IList<SourceInstrumentQuote> OutPutSourceInstrumentData(string repoFolder, string ticker, int range, string sourceFileType)
        {
            IList<SourceInstrumentQuote> finalOutPutInstrumentCollection = new List<SourceInstrumentQuote>();

            switch (sourceFileType)
            {
                case "CSV":
                    finalOutPutInstrumentCollection = OutPutCSVData(repoFolder, range);
                    break;
                case "MSLocal":
                    finalOutPutInstrumentCollection = OutPutMSLocalData(repoFolder, ticker, range);
                    break;   

            }

            return finalOutPutInstrumentCollection;
        }

        public IList<SourceInstrumentQuote> OutPutMSLocalData(string repoFolder, string ticker, int range)
        {
            sourceInstrumentQuotes = new List<SourceInstrumentQuote>();
            msLocalMetaLoader = new SourceInstrumentFileMetadataLoader(repoFolder, ticker, range);
            SourceInstrumentMSLocalParser msParser = new SourceInstrumentMSLocalParser(msLocalMetaLoader);
            msLocalQuotes = msParser.ParseSourceQuotes();
            IEnumerable<DataRecord> dataRecords = msLocalQuotes.Last().TableData;

            foreach (DataRecord record in dataRecords)
            {
                SourceInstrumentQuote quote = new SourceInstrumentQuote();
                quote.Ticker = msLocalQuotes.Last().Metadata.Symbol;
                quote.Date = record.Date;
                quote.Close = Convert.ToDecimal(record.Last);
                sourceInstrumentQuotes.Add(quote);
            }

            return sourceInstrumentQuotes.AsParallel().ToList();
        }


        public IList<SourceInstrumentQuote> OutPutCSVData(string repoFolder, int range)
        {
            sourceInstrumentQuotes = new List<SourceInstrumentQuote>();
            csvMetaLoader = new SourceInstrumentFileMetadataLoader(repoFolder);
            SourceInstrumentCSVParser csvParser = new SourceInstrumentCSVParser(csvMetaLoader);
            csvQuotes = csvParser.ParseSourceQuotes().ToList();
           

            for(int i = (csvQuotes.Count - range); i < csvQuotes.Count; i++ )
            {
                SourceInstrumentQuote quote = new SourceInstrumentQuote();
                quote.Date = csvQuotes[i].Date;
                quote.Close = csvQuotes[i].Close;
                sourceInstrumentQuotes.Add(quote);
            }

            return sourceInstrumentQuotes.AsParallel().ToList();
        }



    }  
}
