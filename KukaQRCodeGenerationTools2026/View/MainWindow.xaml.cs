using KukaQRCodeGenerationTools2026.Common;
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
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel()
            {
                ErrorMessageEvent = msg => MessageBoxHelper.Error(msg),
                InfoMessageEvent = msg => MessageBoxHelper.Info(msg),
                WarningMessageEvent = msg => MessageBoxHelper.Warning(msg),
            };
        }

        /// <summary>
        /// 设置按钮 点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSetting_Click(object sender, RoutedEventArgs e)
        {
            SettingWindow win = new()
            {
                Title = $"{Title} - 设置",
                Top = Top + 30,
                Left = Left + 30,
            };
            win.ShowDialog();
            Show();
        }

        /// <summary>
        /// 地图选择按钮 点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnChooseMap_Click(object sender, RoutedEventArgs e)
        {
            MapSelectWindow win = new()
            {
                Title = $"{Title} - 地图选择",
                Top = Top + 30,
                Left = Left + 30,
                SelectedMapEvent = mapItem =>
                {
                    try
                    {
                        if (DataContext is not MainWindowViewModel vm) return;

                        vm.CurrentMapName = mapItem.MapName;
                        vm.CurrentFloorNumber = mapItem.FloorNumber;
                        vm.CurrentMapCode = mapItem.MapCode;
                        vm.CurrentBussinessMapPath = mapItem.BussinessMapPath;
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Error(ex);
                        MessageBoxHelper.Error(ex.Message);
                    }
                },
            };
            win.ShowDialog();
            Show();
        }
    }
}
