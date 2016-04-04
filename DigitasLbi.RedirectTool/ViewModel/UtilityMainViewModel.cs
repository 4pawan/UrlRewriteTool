using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;
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
        private bool _isConfigXmlEnabled;
        private bool _isValidateXmlEnabled;
        private bool _isGenerateXmlEnabled;
        public ICommand ShowDialogToSelectExcel { get; set; }
        public ICommand ShowSaveDialog { get; set; }
        public ICommand UrlRewriteUtilityCommand { get; set; }
        public ICommand ValidateRewriteRuleCommand { get; set; }
        public ICommand ConfigureRewriteRuleCommand { get; set; }

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
                GenerateXmlEnabledEvent();
                IsConfigXmlEnabled = false;
                IsValidateXmlEnabled = false;
            }
        }

        public string ExcelDestinationPath
        {
            get { return _excelDestinationPath; }
            set
            {
                _excelDestinationPath = value;
                OnPropertyChanged();
                GenerateXmlEnabledEvent();
                IsConfigXmlEnabled = false;
                IsValidateXmlEnabled = false;
            }
        }

        public bool IsConfigXmlEnabled
        {
            get { return _isConfigXmlEnabled; }
            set
            {
                _isConfigXmlEnabled = value;
                OnPropertyChanged();
            }
        }

        public bool IsValidateXmlEnabled
        {
            get { return _isValidateXmlEnabled; }
            set
            {
                _isValidateXmlEnabled = value;
                OnPropertyChanged();
            }
        }

        public bool IsGenerateXmlEnabled
        {
            get { return _isGenerateXmlEnabled; }
            set
            {
                _isGenerateXmlEnabled = value;
                OnPropertyChanged();
            }
        }

        public UtilityMainViewModel()
        {
            IsGenerateXmlEnabled = false;
            IsConfigXmlEnabled = false;

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
                    IsConfigXmlEnabled = true;
                }
                if (msg.Contains(Constant.Constant.Mesasge.Fail.ToString()))
                {
                    StatusFlag = Constant.Constant.MesasgeColor.Red;
                    Message = msg;
                }
            }, () => true);

            ConfigureRewriteRuleCommand = new RelayCommand(() =>
            {
                SaveFileDialog savedlg = new SaveFileDialog
                {
                    Filter = "config files (*.config)|*.config|All files (*.*)|*.*",
                    FilterIndex = 1,
                    RestoreDirectory = true
                };

                if (savedlg.ShowDialog() == true)
                {
                    Helper.Helper.ConfigureRewriteRule(ExcelDestinationPath, savedlg.FileName);
                    Message = Constant.Constant.Utility.ConfigKeptAtLoc + "\n" + savedlg.FileName;
                    IsValidateXmlEnabled = true;
                }

            }, () => true);

            ValidateRewriteRuleCommand = new RelayCommand(async () =>
            {
                IsGenerateXmlEnabled = false;
                IsConfigXmlEnabled = false;
                IsValidateXmlEnabled = false;

                Message = Constant.Constant.Utility.WorkingOnReport + "\n\n";
                StatusFlag = Constant.Constant.MesasgeColor.InProcess;
                DataTable dt = Helper.Helper.GetDataTableFromXml(ExcelDestinationPath);
                string validationDoneTxt = "";

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string validationTxt = string.Format(Constant.Constant.Template.ValidationTemplate, i);

                    if (i > 0)
                    {
                        validationDoneTxt = string.Format(Constant.Constant.Template.ValidationDoneTemplate, i - 1, dt.Rows[i - 1][2]);
                    }
                    Message += $"{validationDoneTxt}{validationTxt}";
                    dt.Rows[i][2] = await Helper.Helper.ValidateRuleAsync(dt.Rows[i][0].ToString(), dt.Rows[i][1].ToString());
                }
                Message = Constant.Constant.Utility.GeneratingReport;
                string excelToBeSavedAtLocation = ExcelDestinationPath.Replace(".xml", ".xlsx");
                Helper.Helper.DataTableToExcel(excelToBeSavedAtLocation, dt);
                Message = Constant.Constant.Utility.ReportGenerationDone + "\n" + excelToBeSavedAtLocation;
                StatusFlag = Constant.Constant.MesasgeColor.Green;

                IsGenerateXmlEnabled = true;
                IsConfigXmlEnabled = true;
                IsValidateXmlEnabled = true;
            }, () => true);

            StatusFlag = Constant.Constant.MesasgeColor.Default;
            Message = Constant.Constant.Mesasge.NotStarted.ToString();

        }


        public void GenerateXmlEnabledEvent()
        {
            if (!string.IsNullOrEmpty(ExcelSourcePath) && !string.IsNullOrEmpty(ExcelDestinationPath))
            {
                IsGenerateXmlEnabled = true;
            }
            else
            {
                IsGenerateXmlEnabled = false;
            }

        }

    }
}
