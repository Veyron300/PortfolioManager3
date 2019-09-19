using PortfolioManager3.WPFUtility;
using IMA.API.LocalData;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Ookii.Dialogs.Wpf;
using PortfolioManager3.InstrumentDataFactory;
using System.IO;

namespace PortfolioManager3
{/// <summary>
/// Viewmodel for the instrument selection dialog
/// </summary>
    public class InstrumentSelectionViewModel : ViewModelBase
    {
        IDataManager dataManager = DataManagerFactory.GetDataManager();

        public string InstrumentFileType { get; set; }

        private string _csvFilePath = string.Empty;
        public string CSVFilePath
        {
            get { return _csvFilePath; }
            set { SetProperty(ref _csvFilePath, value, () => CSVFilePath); }
        }

        private bool? _dialogResult = null;
        public bool? DialogResult
        {
            get { return _dialogResult; }
            set { SetProperty(ref _dialogResult, value, () => DialogResult); }
        }

        private string _sourceFolder = string.Empty;
        public string SourceFolder
        {
            get { return _sourceFolder; }
            set
            {
                SetProperty(ref _sourceFolder, value, () => SourceFolder);
             
            }
        }

        private SourceInstrumentMetadata _currentInstrument;
        public SourceInstrumentMetadata CurrentInstrument
        {
            get { return _currentInstrument; }
            set { SetProperty(ref _currentInstrument, value, () => CurrentInstrument); }
        }

        private SecurityMetadata _currentMSLocalInstrument;
        public SecurityMetadata CurrentMSLocalInstrument
        {
            get { return _currentMSLocalInstrument; }
            set { SetProperty(ref _currentMSLocalInstrument, value, () => CurrentMSLocalInstrument); }
        }

        private ITextBoxView _csvFilePathTextBoxView = null;
        public ITextBoxView CSVFilePathTextBoxView
        {
            get { return _csvFilePathTextBoxView; }
            set { SetProperty(ref _csvFilePathTextBoxView, value, () => CSVFilePathTextBoxView); }
        }
 

        private ObservableCollection<SecurityMetadata> _metastockMetadata = new ObservableCollection<SecurityMetadata>();
        public ObservableCollection<SecurityMetadata> MetastockMetadata
        {
            get { return _metastockMetadata; }
            set { SetProperty(ref _metastockMetadata, value, () => MetastockMetadata); }
        }

        public InstrumentSelectionViewModel()
        {
            OnSelectedInstrumentFileTypeChanged = new RelayCommand(SelectedInstrumentFileTypeChangedExecute);
            BrowseForSourceFolderCommand = new RelayCommand(BrowseForSourceFolder,BrowseForSourceFolderCanExecute);
            AddInstrumentCommand = new RelayCommand(AddInstrumentExecute,AddInstrumentCanExecute);
            OnSelectedInstrumentChanged = new RelayCommand(SelectedMSLocalInstrumentChanged);
        }

        public ICommand OnSelectedInstrumentFileTypeChanged
        {
            get;
            private set;
        }

        public ICommand OnSelectedInstrumentChanged
        {
            get;
            private set;
        }

        public ICommand BrowseForSourceFolderCommand
        {
            get;
            private set;
        }

        public ICommand AddInstrumentCommand
        {
            get;
            private set;
        }

        private void BrowseForSourceFolder(object parameter)
        {
            switch (InstrumentFileType)
            {
                case "CSV":
                    ShowOpenFileDialog();
                    break;

                case "MSLocal":
                    ShowVistaFolderBrowserDialog();
                    break;
            }
            
        }

        private bool BrowseForSourceFolderCanExecute(object parameter)
        {
            return InstrumentFileType != null;
        }

        private void LoadMetadataFromSourceFolder()
        {
            RepositoryId repoId = new RepositoryId(SourceFolder);
            var allMetadata = dataManager.GetAllMetadataAsync(repoId).Result;
            MetastockMetadata = new ObservableCollection<SecurityMetadata>(allMetadata);
        }

        private void AddInstrumentExecute(object parameter)
        {
            InitializeCurrentInstrument();
            Mediator.Instance.OnInstrumentAdded(this, CurrentInstrument);
            DialogResult = true;
        }

        private void SelectedInstrumentFileTypeChangedExecute(object paramater)
        {
            InstrumentFileType = (string)paramater;
        }

        private bool AddInstrumentCanExecute(object parameter)
        {
            return CurrentMSLocalInstrument != null || CSVFilePath != null;
        }

        private void ShowOpenFileDialog()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".csv";
            bool? result = dlg.ShowDialog();

            if (result.HasValue && result.Value)
            {
                string filename = dlg.FileName;
                CSVFilePath = filename;
            }
        }

        private void ShowVistaFolderBrowserDialog()
        {
            VistaFolderBrowserDialog folderBrowserDialog = new VistaFolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() ?? false)
            {
                SourceFolder = folderBrowserDialog.SelectedPath;

            }

            LoadMetadataFromSourceFolder();
        }

        private void SelectedMSLocalInstrumentChanged(object parameter)
        {

        }

        private void InitializeCurrentInstrument()
        {
            CurrentInstrument = new SourceInstrumentMetadata();

            switch (InstrumentFileType)
            {
                case "MSLocal":
                    CurrentInstrument.RepositoryPath = CurrentMSLocalInstrument.FolderPath;
                    CurrentInstrument.Symbol = CurrentMSLocalInstrument.Symbol;
                    CurrentInstrument.Range = 60;
                    CurrentInstrument.FileType = "MSLocal";
                    break;

                case "CSV":
                    CurrentInstrument.RepositoryPath = CSVFilePath;
                    CurrentInstrument.Symbol = Path.GetFileNameWithoutExtension(CSVFilePath);
                    CurrentInstrument.Range = 60;
                    CurrentInstrument.FileType = "CSV";
                    break;
            }
            
        }
    }
}
