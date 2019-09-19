using System;
using System.Collections.Generic;

namespace PortfolioManager3.WPFUtility
{
    /// <summary>
    /// Provides the EventArgs for Portfolio selection
    /// </summary>
    public class PortfolioChangedEventArgs : EventArgs
    {
        public Portfolio Portfolio { get; set; }
        public IEnumerable<Analytic> AnalyticList { get; set; }
    }
}