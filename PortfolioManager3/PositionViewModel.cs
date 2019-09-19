using System.Collections.Generic;
using System.Linq;
using PortfolioManager3.WPFUtility;
using System.Collections.ObjectModel;
using System.Windows.Input;
using PortfolioManager3.XmlRepository;
using PortfolioManager3.InstrumentDataFactory;

namespace PortfolioManager3
{/// <summary>
/// Viewmodel for Positions view user control
/// </summary>
    public class PositionViewModel : ViewModelBase
    {
        public IList<SourceInstrumentQuote> InstrumentHistoricalData { get; set; }

        private Portfolio _currentPortfolio = null;
        public Portfolio CurrentPortfolio
        {
            get { return _currentPortfolio; }
            set
            {
                SetProperty(ref _currentPortfolio, value, () => CurrentPortfolio);
            }
        }

        private SourceInstrumentMetadata _currentInstrument = null;
        public SourceInstrumentMetadata CurrentInstrument
        {
            get { return _currentInstrument; }
            set { SetProperty(ref _currentInstrument, value, () => CurrentInstrument); }
        }

        private Position _newPosition = null;
        public Position NewPosition
        {
            get { return _newPosition; }
            set { SetProperty(ref _newPosition, value, () => NewPosition); }
        
        }

        private bool _isDirty = false;
        public bool IsDirty
        {
            get { return _isDirty; }
            set { SetProperty(ref _isDirty, value, () => IsDirty); }
        }

        public ICommand SaveCommand
        {
            get;
            private set;
        }

        public ICommand SelectInstrumentCommand
        {
            get;
            private set;
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

        private XmlRepositoryWriter RepositoryWriter { get; set; } = new XmlRepositoryWriter();
        
        public XmlRepositoryReader RepositoryReader { get; set; }
        
        public SourceInstrumentDataFactory Factory { get; set; }

        public IList<IList<Analytic>> CombinedAnalytic { get; set; }

        public PositionViewModel()
        {
            SaveCommand = new RelayCommand(SaveExecute);
            SelectInstrumentCommand = new RelayCommand(SelectInstrumentExecute);
            Factory = new SourceInstrumentDataFactory();

            Mediator.Instance.PortfolioChanged += (s, e) =>
            {
                UpdateCurrentPortfolio(e.Portfolio);

                RepositoryReader = new XmlRepositoryReader();
                Positions = RepositoryReader.LoadPositions(e.Portfolio);


                UpdateCurrentPortfolioPrices(e.Portfolio);
            };

            Mediator.Instance.AddInstrument += (s, e) =>
            {
                UpdateCurrentInstrument(e.Instrument);
                InitializeCurrentPosition();
            };

            
        }
  
        private Position CreatePosition()
        {
            
           
            NewPosition.EntryDate = Positions.Last().EntryDate;
            NewPosition.Units = Positions.Last().Units;
            NewPosition.EntryPrice = Positions.Last().EntryPrice;
            NewPosition.PortfolioName = CurrentPortfolio.PortfolioName; 
            UpdatePositionCurrentPrice(NewPosition);
            RepositoryWriter.WritePosition(CurrentPortfolio, NewPosition.Ticker, NewPosition.Units, NewPosition.EntryDate, NewPosition.EntryPrice, NewPosition.InstrumentRepositoryPath, NewPosition.InstrumentFileType);
            Mediator.Instance.OnPositionAdded(this, NewPosition);
            return NewPosition;
        }

        public void SaveExecute(object parameter)
        {
            CreatePosition();
        }

        public void SelectInstrumentExecute(object parameter)
        {
            InstrumentSelectionViewModel ISVM = new InstrumentSelectionViewModel();
            InstrumentSelectionWindow ISW = new InstrumentSelectionWindow();
            ISW.DataContext = ISVM;
            ISW.ShowDialog();
        }
        
        private Portfolio UpdateCurrentPortfolio(Portfolio portfolio)
        {
            return CurrentPortfolio = portfolio;
        }
        
        private SourceInstrumentMetadata UpdateCurrentInstrument(SourceInstrumentMetadata instrument)
        {
            return CurrentInstrument = instrument;
        }

        private void InitializeCurrentPosition()
        {
            IsDirty = true;
            NewPosition = new Position();
            NewPosition.Ticker = CurrentInstrument.Symbol;
            NewPosition.InstrumentRepositoryPath = CurrentInstrument.RepositoryPath;
            NewPosition.InstrumentFileType = CurrentInstrument.FileType;
            Positions.Add(NewPosition);
        }

        private Position UpdatePositionCurrentPrice(Position position)
        {
            InstrumentHistoricalData = Factory.OutPutSourceInstrumentData(position.InstrumentRepositoryPath, position.Ticker, 2, position.InstrumentFileType);
            position.CurrentPrice = InstrumentHistoricalData.Last().Close;
            position.CurrentOpenPositionValue = PortfolioAnalytics.CalculateCurrentOpenPositionValue(position);
            position.Profit = PortfolioAnalytics.CalculatePositionProfit(position);
            position.ReturnRate = PortfolioAnalytics.CalculatePositionReturn(position);
            return position;
        }
 
        private void UpdateCurrentPortfolioPrices(Portfolio portfolio)
        {
            if (Positions != null)
            {

                foreach (Position position in Positions)
                {
                    UpdatePositionCurrentPrice(position);
                    
                }


            }  
                
        }
    }

}
