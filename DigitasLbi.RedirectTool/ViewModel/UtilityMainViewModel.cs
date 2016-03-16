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
        public ICommand ValidateRewriteRuleCommand { get; set; }



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
                StatusFlag = Constant.Constant.MesasgeColor.Default;
                Message = Constant.Constant.Mesasge.NotStarted.ToString();

                OpenFileDialog dlg = new OpenFileDialog
                {
                    DefaultExt = ".xlsx",
                    Filter = "xlsx Files (*.xlsx)|*.xlsx|xls Files (*.xls)|*.xls"
                };
                if (dlg.ShowDialog() == true)
                {
                    ExcelSourcePath = dlg.FileName;
                }
            }, () => true);

            ShowSaveDialog = new RelayCommand(() =>
            {
                ExcelDestinationPath = "";
                StatusFlag = Constant.Constant.MesasgeColor.Default;
                Message = Constant.Constant.Mesasge.NotStarted.ToString();

                SaveFileDialog savedlg = new SaveFileDialog
                {
                    Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*",
                    FilterIndex = 1,
                    RestoreDirectory = true
                };

                if (savedlg.ShowDialog() == true)
                {
                    ExcelDestinationPath = savedlg.FileName;
                }

            }, () => true);


            UrlRewriteUtilityCommand = new RelayCommand(async () =>
            {
                StatusFlag = Constant.Constant.MesasgeColor.InProcess;
                Message = Constant.Constant.Mesasge.InProgress.ToString();
                string msg = null;
                await Task.Run(() =>
                {
                    msg = Helper.Helper.CreateFile(ExcelDestinationPath, ExcelSourcePath);
                });
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

            ValidateRewriteRuleCommand = new RelayCommand(async () =>
            {
                Message = "We are working on report...Please wait !";
                await Helper.Helper.ValidateRewriteRulesAsync(ExcelDestinationPath);
                Message = "Report created.";
            }, () => true);

            StatusFlag = Constant.Constant.MesasgeColor.Default;
            Message = Constant.Constant.Mesasge.NotStarted.ToString();

        }
    }
}
