using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MVVM
{
    public class NotifiableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private Dictionary<string, object> _properties = new Dictionary<string, object>();

        protected bool CheckChanged<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (!EqualityComparer<T>.Default.Equals(field, value))
            {
                field = value;
                OnPropertyChanged(propertyName);
                return true;
            }
            return false;
        }

        protected T Get<T>([CallerMemberName] string name = null)
        {
            if (_properties.TryGetValue(name, out var value))
                return value == null ? default(T) : (T)value;

            return default(T);
        }

        protected bool Set<T>(T value, [CallerMemberName] string name = null)
        {
            if (EqualityComparer<T>.Default.Equals(value, Get<T>(name)))
            {
                return false;
            }

            _properties[name] = value;
            OnPropertyChanged(name);
            return true;
        }
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
