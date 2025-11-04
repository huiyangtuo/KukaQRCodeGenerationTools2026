using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace KukaQRCodeGenerationTools2026.Common.Wpf
{
    public class NotifyBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void DoNotify([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = "")
        {
            storage = value;
            DoNotify(propertyName);
        }
    }
}
