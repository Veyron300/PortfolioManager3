using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PortfolioManager3.WPFUtility;
using System.Windows.Input;

namespace PortfolioManager3
{
    public class NewPortfolioViewModel : ViewModelBase
    {
        public ICommand OkCommand
        {
            get;
            private set;
        }
        private bool? _dialogResult = null;
        public bool? DialogResult
        {
            get { return _dialogResult; }
            set { SetProperty(ref _dialogResult, value, () => DialogResult); }
        }

        private string _portfolioName = string.Empty;
        public string PortfolioName
        {
            get { return _portfolioName; }
            set { SetProperty(ref _portfolioName, value, () => PortfolioName); }
        }

        private string _benchmarkInstrument = string.Empty;
        public string BenchmarkInstrument
        {
            get { return _benchmarkInstrument; }
            set { SetProperty(ref _benchmarkInstrument, value, () => BenchmarkInstrument); }
        }

        private ITextBoxView _portfolioNameTextBoxView = null;
        public ITextBoxView PortfolioNameTextBoxView
        {
            get { return _portfolioNameTextBoxView; }
            set { SetProperty(ref _portfolioNameTextBoxView, value, () => PortfolioNameTextBoxView); }
        }

        private ITextBoxView _benchmarkInstrumentTextBoxView = null;
        public ITextBoxView BenchmarkInstrumentTextBoxView
        {
            get { return _benchmarkInstrumentTextBoxView; }
            set { SetProperty(ref _benchmarkInstrumentTextBoxView, value, () => BenchmarkInstrumentTextBoxView); }
        }

        public NewPortfolioViewModel()
        {
            OkCommand = new RelayCommand(OkCommandExecute, OkCommandCanExecute);
        }

        private void OkCommandExecute(object parameter)
        {
            DialogResult = true;  
        }

        private bool OkCommandCanExecute(object parameter)
        {
            return !string.IsNullOrWhiteSpace(PortfolioName) && !string.IsNullOrWhiteSpace(BenchmarkInstrument);
        }

    }
}
