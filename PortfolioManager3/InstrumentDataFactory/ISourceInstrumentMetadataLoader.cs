﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioManager3.InstrumentDataFactory
{
    public interface ISourceInstrumentMetadataLoader
    {
        SourceInstrumentMetadata LoadSourceInstrumentMetadata();
    }
}
