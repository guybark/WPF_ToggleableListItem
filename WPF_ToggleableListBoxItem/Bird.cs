using System.ComponentModel;

namespace WPF_ToggleableListItem
{
    public class Bird : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public string Habitat { get; set; }
        public string Voice { get; set; }

        private bool isCheckable;
        public bool IsCheckable { get => isCheckable; set => isCheckable = value; }

        private bool birdItemIsChecked;
        public bool BirdItemIsChecked
        {
            get
            {
                return birdItemIsChecked;
            }
            set
            {
                birdItemIsChecked = value;

                OnPropertyChanged("BirdItemIsChecked");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
