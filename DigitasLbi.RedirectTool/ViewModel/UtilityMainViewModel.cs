using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
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
        public ICommand ConfigureRewriteRuleCommand { get; set; }

        public string ValidationTemplate { get; set; } = "verification started for Rule{0}\nExisting url : {1}\nNew url :{2}\nResult :InProgress";
        public string ValidationDoneTemplate { get; set; } = "verification done for Rule{0}\nExisting url : {1}\nNew url :{2}\nResult :{3}";


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
                    Message = "Rewrite output file kept at location:\n" + savedlg.FileName;
                }

            }, () => true);

            ValidateRewriteRuleCommand = new RelayCommand(async () =>
            {
                Message = "We are working on report...Please wait !\n";
                StatusFlag = Constant.Constant.MesasgeColor.InProcess;
                DataTable dt = Helper.Helper.GetDataTableFromXml(ExcelDestinationPath);
                string validationDoneTxt = "";

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string validationTxt = string.Format(ValidationTemplate, i, dt.Rows[i][0], dt.Rows[i][1]);

                    if (i > 0)
                    {
                        validationDoneTxt = string.Format(ValidationDoneTemplate, i - 1, dt.Rows[i - 1][0], dt.Rows[i - 1][1], dt.Rows[i - 1][2]);
                    }
                    Message += string.Format("{0}\n\n{1}", validationTxt, validationDoneTxt);
                    dt.Rows[i][2] = await Helper.Helper.ValidateRuleAsync(dt.Rows[i][0].ToString(), dt.Rows[i][1].ToString());
                }
                Message = "Validation done. Now, generating report.";
                string excelToBeSavedAtLocation = ExcelDestinationPath.Replace(".xml", ".xlsx");
                Helper.Helper.DataTableToExcel(excelToBeSavedAtLocation, dt);
                Message = "Report created and can be downloaded from location :\n" + excelToBeSavedAtLocation;
                StatusFlag = Constant.Constant.MesasgeColor.Green;
            }, () => true);

            StatusFlag = Constant.Constant.MesasgeColor.Default;
            Message = Constant.Constant.Mesasge.NotStarted.ToString();

        }
    }
}
