using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioManager3.InstrumentDataFactory
{
    public class SourceInstrumentFileMetadataLoader : ISourceInstrumentMetadataLoader
    {
        private string _repoFolderPath;
        private string _symbol;
        private string _fileType;
        private int _range;
        public SourceInstrumentMetadata LoadedSourceInstrumentMetadata = new SourceInstrumentMetadata();

        public SourceInstrumentFileMetadataLoader(string repoFolderPath, string symbol, int range)
        {
            _repoFolderPath = repoFolderPath;
            _symbol = symbol;
            _range = range;
        }

        public SourceInstrumentFileMetadataLoader(string repoFolderPath)
        {
            _repoFolderPath = repoFolderPath;
        }

        public SourceInstrumentMetadata LoadSourceInstrumentMetadata()
        {
            LoadedSourceInstrumentMetadata.RepositoryPath = _repoFolderPath;
            LoadedSourceInstrumentMetadata.Symbol = _symbol;
            LoadedSourceInstrumentMetadata.Range = _range;

            return LoadedSourceInstrumentMetadata;
            
        }
    }
}
