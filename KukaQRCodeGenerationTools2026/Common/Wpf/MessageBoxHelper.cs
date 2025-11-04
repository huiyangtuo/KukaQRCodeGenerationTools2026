using System.Windows;

namespace KukaQRCodeGenerationTools2026.Common.Wpf
{
    public class MessageBoxHelper
    {
        public static MessageBoxResult Error(string message, string caption = "异常提示")
        {
            return MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public static MessageBoxResult Info(string message, string caption = "提示")
        {
            return MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Information);
        }
        public static MessageBoxResult Warning(string message, string caption = "提示")
        {
            return MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        public static MessageBoxResult Confirm(string message, string caption = "提示")
        {
            return MessageBox.Show(message, caption, MessageBoxButton.YesNo, MessageBoxImage.Information);
        }
    }
}
