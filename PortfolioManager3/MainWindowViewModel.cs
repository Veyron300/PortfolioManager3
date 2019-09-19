using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PortfolioManager3.WPFUtility;
using PortfolioManager3.XmlRepository;
using PortfolioManager3.InstrumentDataFactory;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Xml.Linq;
using System.Xml.XPath;

namespace PortfolioManager3
{
    public class MainWindowViewModel : ViewModelBase
    {
        
        public MainWindowViewModel()
        {
            
            PositionsDataView = new PositionViewModel();
            PortfoliosDataView = new PortfolioViewModel();
            AnalyticsDataView = new AnalyticsViewModel();
            
        }

        

        private PositionViewModel _positionsDataView;
        public PositionViewModel PositionsDataView
        {
            get { return _positionsDataView; }
            set { SetProperty(ref _positionsDataView, value, () => PositionsDataView); }
        }

        private PortfolioViewModel _portfoliosDataView;
        public PortfolioViewModel PortfoliosDataView
        {
            get { return _portfoliosDataView; }
            set { SetProperty(ref _portfoliosDataView, value, () => PortfoliosDataView); }
        }

        private ViewModelBase _currentViewModel;
        private ViewModelBase CurrentViewModel
        {
            get { return _currentViewModel; }
            set { SetProperty(ref _currentViewModel, value, () => CurrentViewModel); }
        }

        private AnalyticsViewModel _analyticsDataView;
        public AnalyticsViewModel AnalyticsDataView
        {
            get { return _analyticsDataView; }
            set { SetProperty(ref _analyticsDataView, value, () => AnalyticsDataView); }
        }


        


    }
}
