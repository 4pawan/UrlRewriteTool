using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;

namespace DigitasLbi.RedirectTool.ViewModel
{
    public class UtilityMainViewModel : BaseViewModel
    {
        private string _excelSourcePath;
        private string _excelDestinationPath;
        private string _message;
        private Constant.Constant.MesasgeColor _statusFlag;
        public ICommand ShowDialogToSelectExcel { get; set; }
        public ICommand ShowSaveDialog { get; set; }
        public ICommand UrlRewriteUtilityCommand { get; set; }

        public Constant.Constant.MesasgeColor StatusFlag
        {
            get
            {
                return _statusFlag;
            }
            set
            {
                _statusFlag = value;
                OnPropertyChanged();
            }
        }

        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                OnPropertyChanged();
            }
        }


        public string ExcelSourcePath
        {
            get { return _excelSourcePath; }
            set
            {
                _excelSourcePath = value;
                OnPropertyChanged();
            }
        }

        public string ExcelDestinationPath
        {
            get { return _excelDestinationPath; }
            set
            {
                _excelDestinationPath = value;
                OnPropertyChanged();
            }
        }

        public UtilityMainViewModel()
        {
            ShowDialogToSelectExcel = new RelayCommand(() =>
            {
                ExcelSourcePath = "";
                OpenFileDialog dlg = new OpenFileDialog
                {
                    DefaultExt = ".xlsx",
                    Filter = "xlsx Files (*.xlsx)|*.xlsx|xls Files (*.xls)|*.xls"
                };
                bool? result = dlg.ShowDialog();
                if (result == true)
                {
                    ExcelSourcePath = dlg.FileName;
                }
            }, () => true);

            ShowSaveDialog = new RelayCommand(() =>
            {
                ExcelDestinationPath = "";
                SaveFileDialog savedlg = new SaveFileDialog
                {
                    Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*",
                    FilterIndex = 1,
                    RestoreDirectory = true
                };
                bool? result = savedlg.ShowDialog();

                if (result == true)
                {
                    ExcelDestinationPath = savedlg.FileName;
                }

            }, () => true);

            UrlRewriteUtilityCommand = new RelayCommand(() =>
            {
                StatusFlag = Constant.Constant.MesasgeColor.InProcess;
                Message = Constant.Constant.Mesasge.InProgress.ToString();
                string msg = null;
                //Task.Run(() =>
                //{
                //  msg = Helper.Helper.CreateFile(ExcelDestinationPath, ExcelSourcePath);
                //});
                msg = Helper.Helper.CreateFile(ExcelDestinationPath, ExcelSourcePath);
                if (msg.Contains(Constant.Constant.Mesasge.Success.ToString()))
                {
                    StatusFlag = Constant.Constant.MesasgeColor.Green;
                    Message = Constant.Constant.Mesasge.Success.ToString();
                }
                if (msg.Contains(Constant.Constant.Mesasge.Fail.ToString()))
                {
                    StatusFlag = Constant.Constant.MesasgeColor.Red;
                    Message = msg;
                }

            }, () => true);

            StatusFlag = Constant.Constant.MesasgeColor.Default;
            Message = Constant.Constant.Mesasge.NotStarted.ToString();

        }
    }
}
