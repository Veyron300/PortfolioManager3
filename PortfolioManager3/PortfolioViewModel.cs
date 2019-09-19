using System;
using PortfolioManager3.WPFUtility;
using System.Collections.ObjectModel;
using System.Windows.Input;
using PortfolioManager3.XmlRepository;

namespace PortfolioManager3
{/// <summary>
/// Viewmodel for Portfolios view user control
/// </summary>
    public class PortfolioViewModel : ViewModelBase
    {
        

        private string _portfolioName;
        public string PortfolioName
        {
            get { return _portfolioName; }
            set
            {
                SetProperty(ref _portfolioName, value, ()=> PortfolioName);
            }
        }

        private ObservableCollection<Portfolio> _portfolios = new ObservableCollection<Portfolio>();
        public ObservableCollection<Portfolio> Portfolios
        {
            get { return _portfolios; }
            set { SetProperty(ref _portfolios, value, () => Portfolios); }
        }

        public ICommand AddNewPortfolioCommand
        {
            get;
            private set;
        }

        public RelayCommand DeletePortfolioCommand
        {
            get;
            private set;
        }

        public ICommand OnPortfolioChanged
        {
            get;
            private set;
        }

        private XmlRepositoryWriter _repositoryWriter = new XmlRepositoryWriter();
        public XmlRepositoryWriter RepositoryWriter
        {
            get { return _repositoryWriter; }
            set { SetProperty(ref _repositoryWriter, value, () => RepositoryWriter); }
        }

        private XmlRepositoryReader _repositoryReader = new XmlRepositoryReader();
        public XmlRepositoryReader RepositoryReader
        {
            get { return _repositoryReader; }
            set { SetProperty(ref _repositoryReader, value, ()=> RepositoryReader); }
        }

        private Portfolio _selectedPortfolio = null;
        public Portfolio SelectedPortfolio
        {
            get { return _selectedPortfolio; }
            set
            {
                SetProperty(ref _selectedPortfolio, value, () => SelectedPortfolio);
                DeletePortfolioCommand.RaiseCanExecuteChanged();
            }

        }

        public PortfolioViewModel()
        {
            AddNewPortfolioCommand = new RelayCommand(AddNewPortfolio);
            OnPortfolioChanged = new RelayCommand(PortfolioChanged);
            DeletePortfolioCommand = new RelayCommand(DeletePortfolio, CanDeletePortfolio);
            Portfolios = RepositoryReader.LoadPortfolios();
        }

        private bool CanDeletePortfolio(object obj)
        {
            return SelectedPortfolio != null;
        }

        private void DeletePortfolio(object obj)
        {
            RepositoryWriter.RemovePortfolio(SelectedPortfolio);
            Portfolios.Remove(SelectedPortfolio);
        }

        private void PortfolioChanged(object obj)
        {
            if(SelectedPortfolio != null)
            {
                
                
                Mediator.Instance.OnPortfolioChanged(this, SelectedPortfolio);
            }

        }

        private void AddNewPortfolio(object parameter)
        {
            Portfolio NewPortfolio = new Portfolio();
            NewPortfolioViewModel NPVM = new NewPortfolioViewModel();
            NewPortfolioWindow NPW = new NewPortfolioWindow();

            NPW.DataContext = NPVM;
            NPW.ShowDialog();
            if (NPW.DialogResult.HasValue && NPW.DialogResult.Value)
                NewPortfolio.PortfolioName = NPW.PortfolioNameTextBox.Text;
            NewPortfolio.BenchmarkInstrument = NPW.BenchmarkInstrumentTextBox.Text;
            NewPortfolio.InceptionDate = DateTime.Today;
            Portfolios.Add(NewPortfolio);
            RepositoryWriter.WritePortfolio(NewPortfolio.PortfolioName, NewPortfolio.BenchmarkInstrument, NewPortfolio.InceptionDate);
        }

        
       
    }
}
