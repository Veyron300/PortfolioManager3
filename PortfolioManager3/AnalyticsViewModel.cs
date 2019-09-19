using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using PortfolioManager3.WPFUtility;
using System.Collections.ObjectModel;
using PortfolioManager3.XmlRepository;
using PortfolioManager3.InstrumentDataFactory;
using IMA.API.LocalData;

namespace PortfolioManager3
{
    /// <summary>
    /// Viewmodel for Analytics user control
    /// </summary>
    public class AnalyticsViewModel : ViewModelBase
    {
        

        private decimal _portfolioProfit;
        public decimal PortfolioProfit
        {
            get { return _portfolioProfit; }
            set
            {
                if (value != _portfolioProfit)
                {
                    _portfolioProfit = value;
                    OnPropertyChanged("PortfolioProfit");
                }
            }
        }
        

        private decimal _portfolioReturn;
        public decimal PortfolioReturn
        {
            get { return _portfolioReturn; }
            set
            {
                SetProperty(ref _portfolioReturn, value, () => PortfolioReturn);
            }
        }

        public IList<IList<Analytic>> CombinedAnalytic { get; set; } = new List<IList<Analytic>>();

        public IList<Analytic> HistoricalReturnSeries { get; set; } 

        public IList<SourceInstrumentQuote> InstrumentHistoricalData { get; set; }


        private XmlRepositoryReader _repositoryReader = new XmlRepositoryReader();
        public XmlRepositoryReader RepositoryReader
        {
            get { return _repositoryReader; }
            set { SetProperty(ref _repositoryReader, value, () => RepositoryReader); }
        }


        private Portfolio _currentPortfolio = null;
        public Portfolio CurrentPortfolio
        {
            get { return _currentPortfolio; }
            set
            {
                SetProperty(ref _currentPortfolio, value, () => CurrentPortfolio);
            }
        }

        private ObservableCollection<Position> _positions = new ObservableCollection<Position>();
        public ObservableCollection<Position> Positions
        {
            get { return _positions; }
            set
            {
                SetProperty(ref _positions, value, () => Positions);
            }
        }

        private ObservableCollection<SecurityDataAndMetadata> _historicalInstrumentData = new ObservableCollection<SecurityDataAndMetadata>();
        public ObservableCollection<SecurityDataAndMetadata> HistoricalInstrumentData
        {
            get { return _historicalInstrumentData; }
            set
            {
                SetProperty(ref _historicalInstrumentData, value, () => HistoricalInstrumentData);
            }
        }

        private PointCollection _portfolioReturnCollection;
        public PointCollection PortfolioReturnCollection
        {
            get { return _portfolioReturnCollection; }
            set
            {
                SetProperty(ref _portfolioReturnCollection, value, () => PortfolioReturnCollection);
            }
        }

        public SourceInstrumentDataFactory Factory { get; set; }

        public AnalyticsViewModel()
        {
            Factory = new SourceInstrumentDataFactory();

            Mediator.Instance.PortfolioChanged += (s, e) =>
            {
                
                UpdateCurrentPortfolio(e.Portfolio);
                Positions = RepositoryReader.LoadPositions(e.Portfolio);
                UpdateCurrentPortfolioPrices(e.Portfolio);
                PortfolioProfit = PortfolioAnalytics.CalculatePortFolioProfit(Positions);
                PortfolioReturn = PortfolioAnalytics.CalculatePortfolioReturn(Positions);
                s = this;
                e.AnalyticList = HistoricalReturnSeries;
                
                
            };
           

            
            Mediator.Instance.AddPosition += (s, e) =>
            {
                Positions.Add(e.Position);
                PortfolioProfit = PortfolioAnalytics.CalculatePortFolioProfit(Positions);
                PortfolioReturn = PortfolioAnalytics.CalculatePortfolioReturn(Positions);
                s = this;
                e.AnalyticList = HistoricalReturnSeries;
            };
        }

        private Portfolio UpdateCurrentPortfolio(Portfolio portfolio)
        {
            return CurrentPortfolio = portfolio;
        }

        private Position UpdatePositionInstrumentData(Position position)
        {
            InstrumentHistoricalData = new List<SourceInstrumentQuote>();
            decimal positionDuration = PortfolioAnalytics.CalculatePositionDuration(position.EntryDate);
            InstrumentHistoricalData = Factory.OutPutSourceInstrumentData(position.InstrumentRepositoryPath, position.Ticker, Convert.ToInt32(positionDuration),position.InstrumentFileType);
            position.CurrentPrice = InstrumentHistoricalData.Last().Close;
            position.CurrentOpenPositionValue = PortfolioAnalytics.CalculateCurrentOpenPositionValue(position);
            position.Profit = PortfolioAnalytics.CalculatePositionProfit(position);
            position.ReturnRate = PortfolioAnalytics.CalculatePositionReturn(position);
            return position;
        }

        private IEnumerable<Analytic> CalculateHistoricalReturn(Position position)
        {
            CombinedAnalytic.Clear(); 
            IList<Analytic> historicalReturn = new List<Analytic>();  
            historicalReturn   =  position.CalculateHistoricalAnalytic(InstrumentHistoricalData,PortfolioAnalytics.CalculateDailyPositionReturn);
            CombinedAnalytic.Add(historicalReturn);

            return historicalReturn;
        }

        /// <summary>
        /// Updates the instrument data for the portfolio and populates a collection of daily returns
        /// </summary>
        /// <param name="portfolio"></param>
        private void UpdateCurrentPortfolioPrices(Portfolio portfolio)
        {
            HistoricalReturnSeries = new List<Analytic>();
            if (Positions.Count() > 0)
            {

                foreach (Position position in Positions)
                {
                    UpdatePositionInstrumentData(position);
                    CalculateHistoricalReturn(position);
                }

                
                HistoricalReturnSeries = PortfolioAnalytics.CombineAnalyticsInParallel(CombinedAnalytic).ToList();
            }

            else
            {
                HistoricalReturnSeries.Clear();
            }
        }

        

    }


}
