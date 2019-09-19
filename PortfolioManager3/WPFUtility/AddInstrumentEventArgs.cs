using IMA.API.LocalData;
using System;
using System.Collections.Generic;
using PortfolioManager3.InstrumentDataFactory;

namespace PortfolioManager3.WPFUtility
{
    /// <summary>
    /// EventArgs for adding a new instrument
    /// </summary>
    public class AddInstrumentEventArgs : EventArgs
    {
        public SourceInstrumentMetadata Instrument { get; set; }
        
    }
}