using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace DigitasLbi.RedirectTool.Common
{
    public class StringToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            SolidColorBrush color = null;

            if (value.ToString() == Constant.Constant.MesasgeColor.Red.ToString())
            {
                color = new SolidColorBrush(Colors.Red);
            }
            else if (value.ToString() == Constant.Constant.MesasgeColor.Green.ToString())
            {
                color = new SolidColorBrush(Colors.Green);
            }

            else if (value.ToString() == Constant.Constant.MesasgeColor.Default.ToString())
            {
                color = new SolidColorBrush(Colors.White);
            }

            else if (value.ToString() == Constant.Constant.MesasgeColor.InProcess.ToString())
            {
                color = new SolidColorBrush(Colors.Gray);
            }


            return color;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
