using KukaQRCodeGenerationTools2026.Common.Wpf;
using KukaQRCodeGenerationTools2026.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace KukaQRCodeGenerationTools2026.View
{
    /// <summary>
    /// SettingWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SettingWindow : Window
    {
        public SettingWindow()
        {
            InitializeComponent();

            DataContext = new SettingWindowViewModel()
            {
                ErrorMessageEvent = msg => MessageBoxHelper.Error(msg),
                InfoMessageEvent = msg => MessageBoxHelper.Info(msg),
                WarningMessageEvent = msg => MessageBoxHelper.Warning(msg),
            };
        }
    }
}
