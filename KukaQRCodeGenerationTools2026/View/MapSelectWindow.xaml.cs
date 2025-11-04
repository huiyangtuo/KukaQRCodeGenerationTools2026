using KukaQRCodeGenerationTools2026.Common.Wpf;
using KukaQRCodeGenerationTools2026.Model;
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
    /// MapSelectWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MapSelectWindow : Window
    {
        public Action<MapItemModel> SelectedMapEvent;

        public MapSelectWindow()
        {
            InitializeComponent();

            DataContext = new MapSelectWindowViewModel()
            {
                ErrorMessageEvent = msg => MessageBoxHelper.Error(msg),
                InfoMessageEvent = msg => MessageBoxHelper.Info(msg),
                WarningMessageEvent = msg => MessageBoxHelper.Warning(msg),
                CloseWindowEvent = () => Close(),
                SelectedMapEvent = mapItem => SelectedMapEvent?.Invoke(mapItem),    // 选中的地图传递到调用窗口中
            };
        }
    }
}
