using System;
using PortfolioManager3.WPFUtility;

namespace PortfolioManager3
{/// <summary>
/// Portfolio class
/// </summary>
    public class Portfolio : ViewModelBase
    {
       
        public string PortfolioName { get; set; }

        public string BenchmarkInstrument { get; set; } 

        private decimal portfolioReturn;
        public decimal PortfolioReturn
        {
            get { return portfolioReturn; }
            set
            {
                if (value != portfolioReturn)
                {
                    portfolioReturn = value;
                    OnPropertyChanged("PortfolioReturn");
                }
            }
        }

        private decimal portfolioProfit;
        public decimal PortfolioProfit
        {
            get { return portfolioProfit; }
            set
            {
                if (value != portfolioProfit)
                {
                    portfolioProfit = value;
                    OnPropertyChanged("PortfolioProfit");
                }
            }

        }

        public DateTime? InceptionDate { get; set; }
    }
}
