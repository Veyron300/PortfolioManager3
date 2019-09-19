using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PortfolioManager3.WPFUtility;

namespace PortfolioManager3.InstrumentDataFactory
{
    public class SourceInstrumentQuote : ViewModelBase
    {
        public string Name { get; set; }

        private string _ticker;
        public string Ticker
        {
            get { return _ticker; }
            set
            {
                if(value != _ticker)
                {
                    _ticker = value;
                    OnPropertyChanged("Ticker");
                }
            }
        }

        public DateTime Date { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
    }
}
