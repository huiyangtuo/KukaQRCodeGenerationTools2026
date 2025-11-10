using KukaQRCodeGenerationTools2026.Common;
using KukaQRCodeGenerationTools2026.Common.Wpf;
using KukaQRCodeGenerationTools2026.Model.QRCode;
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

        private void Window_Closed(object sender, EventArgs e)
        {
            if (DataContext is not MainWindowViewModel vm)
                return;

            vm.SaveSetting();   // 保存设置
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is not MainWindowViewModel vm)
                return;

            vm.ReadSetting();   // 读取页面设置
        }


        #region 渲染地图
        /// <summary>
        /// 刷新地图 按钮 点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnRefreshMap_Click(object sender, RoutedEventArgs e)
        {

        }

        int maxX;
        int minX;
        int maxY;
        int minY;
        double codeWidth = 20;
        double canvasMarginLeft = 40;
        double canvasMarginTop = 40;
        double canvasMarginRight = 80;
        double canvasMarginBottom = 80;
        int scale = 10;
        /// <summary>
        /// 生成二维码地图
        /// </summary>
        /// <param name="l_GroundCodeList"></param>
        /// <param name="selectedCode"></param>
        private void SetQrCodeMap(List<GroundCodeInfoModel> l_GroundCodeList, GroundCodeInfoModel? selectedCode = null)
        {
            try
            {
                canQrCodeMap.Children.RemoveRange(0, canQrCodeMap.Children.Count);
                // 原先直接打开文件，现在直接传入二维码列表进行初始化
                // 导入二维码列表
                //string groundCodeConfigPath = $"{Environment.CurrentDirectory}\\Data\\CodeList\\MainWindow\\{fileName}.json";
                //string strGroundCodeList = FileHelper.Read(groundCodeConfigPath);
                //List<GroundCodeInfo> l_GroundCodeList = JsonConvert.DeserializeObject<List<GroundCodeInfo>>(strGroundCodeList);


                // 找到X最小，Y最大的码作为基准码
                maxX = l_GroundCodeList.Max(code => code.X);
                minX = l_GroundCodeList.Min(code => code.X);
                maxY = l_GroundCodeList.Max(code => code.Y);
                minY = l_GroundCodeList.Min(code => code.Y);

                double canvasWidth = (maxX - minX) / scale + canvasMarginLeft + canvasMarginRight;
                double canvasHeight = (maxY - minY) / scale + canvasMarginTop + canvasMarginBottom;

                canQrCodeMap.Width = canvasWidth;
                canQrCodeMap.Height = canvasHeight;

                // 绘制二维码地图
                foreach (GroundCodeInfoModel code in l_GroundCodeList)
                {
                    bool isCodeSelected = selectedCode != null && (selectedCode.X == code.X && selectedCode.Y == code.Y);

                    StackPanel stackpanel = GenerateQrCodeInCanvas(code, isCodeSelected);
                    canQrCodeMap.Children.Add(stackpanel);
                }
            }
            catch (Exception ex)
            {
                MessageBoxHelper.Error(ex.Message);
            }
        }

        /// <summary>
        /// 在 Canvas 中生成 QRCode
        /// </summary>
        /// <param name="code"></param>
        /// <param name="isSelected"></param>
        /// <returns></returns>
        private StackPanel GenerateQrCodeInCanvas(GroundCodeInfoModel code, bool isSelected = false)
        {
            double canvasX = (code.X - minX) / scale + canvasMarginLeft;
            double canvasY = (maxY - code.Y) / scale + canvasMarginTop;

            StackPanel stackpanel = new StackPanel();
            Canvas.SetLeft(stackpanel, canvasX);
            Canvas.SetTop(stackpanel, canvasY);

            TextBlock text = new TextBlock()
            {
                Text = $"{code.X},{code.Y}",
                FontSize = 10,
                HorizontalAlignment = HorizontalAlignment.Center,
            };
            stackpanel.Children.Add(text);
            Rectangle rectangle = new Rectangle()
            {
                Width = codeWidth,
                Height = codeWidth,
                Fill = isSelected ? new SolidColorBrush(Colors.Red) : new SolidColorBrush(Colors.Green),
                Tag = $"{code.X}aa{code.Y}",
            };
            rectangle.MouseUp += (sender, e) =>
            {
                try
                {
                    InitCanvasQrCodeFill();

                    Rectangle? selectedCode = sender as Rectangle;
                    if (selectedCode == null) return;

                    selectedCode.Fill = new SolidColorBrush(Colors.Red);
                    string? name = selectedCode.Tag as string;
                    if (string.IsNullOrEmpty(name)) return;

                    string[] codeCoordinates = name.Split(new char[] { 'c', 'a' }, StringSplitOptions.RemoveEmptyEntries);
                    string codeX = codeCoordinates[0];
                    string codeY = codeCoordinates[1];
                    //(DataContext as QrCodeMapWindowViewModel).SetSelectedGroundCode(codeX, codeY);
                }
                catch (Exception ex)
                {
                    LogHelper.Error(ex);
                    MessageBoxHelper.Error(ex.Message);
                }
            };
            stackpanel.Children.Add(rectangle);

            return stackpanel;
        }

        /// <summary>
        /// 初始化 Canvas 中的 QRCode 颜色
        /// </summary>
        private void InitCanvasQrCodeFill()
        {
            foreach (var childStackPanel in canQrCodeMap.Children.OfType<StackPanel>())
            {
                foreach (var panelChild in childStackPanel.Children)
                {
                    if (panelChild is Rectangle l_rectangle)
                    {
                        l_rectangle.Fill = new SolidColorBrush(Colors.Green);
                    }
                }
            }
        }

        #endregion
    }
}
