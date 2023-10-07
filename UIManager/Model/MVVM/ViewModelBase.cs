using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace UIManager.Model.MVVM
{
    public class ViewModelBase : INotifyPropertyChanged, INotifyPropertyChanging
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangingEventHandler PropertyChanging;

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs E)
            => this.PropertyChanged?.Invoke(this, E);
        protected virtual void OnPropertyChanged([CallerMemberName] string Name = null)
            => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(Name));

        protected virtual void OnPropertyChanging(PropertyChangingEventArgs E)
            => this.PropertyChanging?.Invoke(this, E);
        protected virtual void OnPropertyChanging([CallerMemberName] string Name = null)
            => this.PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(Name));

        protected void SetProperty<T>(ref T Property, ref T Value, [CallerMemberName] string Name = null)
        {
            OnPropertyChanging(Name);
            Property = Value;
            OnPropertyChanged(Name);
        }
    }
}
