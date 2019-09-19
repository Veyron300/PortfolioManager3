using System;
using PortfolioManager3.WPFUtility;

namespace PortfolioManager3
{
    /// <summary>
    /// Position class
    /// </summary>
    public class Position : ViewModelBase
    {
        public string PortfolioName { get; set; }

        public string Ticker { get; set; }

        public string Name { get; set; }

        public string InstrumentRepositoryPath { get; set; }

        public string InstrumentFileType { get; set; }

        public DateTime? EntryDate { get; set; }

        public decimal EntryPrice { get; set; }

        public decimal Units { get; set; }

        private decimal _profit;
        public decimal Profit
        {
            get { return _profit; }
            set
            {
                if (value != _profit)
                {
                    _profit = value;
                    OnPropertyChanged("Profit");
                }
            }
        }


        private decimal _returnrate;
        public decimal ReturnRate
        {
            get { return _returnrate; }
            set
            {
                if(value != _returnrate)
                {
                    _returnrate = value;
                    OnPropertyChanged("ReturnRate");
                }
            }
        }


        private decimal _currentPrice;
        public decimal CurrentPrice
        {
            get { return _currentPrice; }
            set
            {
                if (value != _currentPrice)
                {
                    _currentPrice = value;
                    OnPropertyChanged("CurrentPrice");
                }
            }
        }

        private decimal _currentOpenPositionValue;
        public decimal CurrentOpenPositionValue
        {
            get { return _currentOpenPositionValue; }
            set
            {
                if(value != _currentOpenPositionValue)
                {
                    _currentOpenPositionValue = value;
                    OnPropertyChanged("CurrentOpenPositionValue");
                }
            }
        }


    }
}
