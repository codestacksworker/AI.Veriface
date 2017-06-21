using System.Collections.Generic;
using System.ComponentModel;

namespace TR_MODULES.Models
{
    public class TrackInfoModel : INotifyPropertyChanged
    {
        private string _tcChannelID;
        private string _name;
        private int _channel_type;
        private string _typestr;
        private string _channel_address;
        private string _longitude;
        private string _latitude;
        private List<CapCount> _capimg;
        private int Id;

        public int ID
        {
            get { return Id; }
            set
            {
                this.Id = value;
                RaisePropertyChanged("ID");
            }
        }

        public string TcChannelID
        {
            get
            {
                return _tcChannelID;
            }
            set
            {
                this._tcChannelID = value;
                RaisePropertyChanged("TcChannelID");
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                this._name = value;
                RaisePropertyChanged("Name");
            }
        }

        public int Channel_type
        {
            get
            {
                return _channel_type;
            }
            set
            {
                this._channel_type = value;
                RaisePropertyChanged("Channel_type");
            }
        }

        public string Typestr
        {
            get
            {
                return _typestr;
            }
            set
            {
                this._typestr = value;
                RaisePropertyChanged("Typestr");
            }
        }

        public string Channel_address
        {
            get
            {
                return _channel_address;
            }
            set
            {
                this._channel_address = value;
                RaisePropertyChanged("Channel_address");
            }
        }

        public string Longitude
        {
            get
            {
                return _longitude;
            }
            set
            {
                this._longitude = value;
                RaisePropertyChanged("Longitude");
            }
        }

        public string Latitude
        {
            get
            {
                return _latitude;
            }
            set
            {
                this._latitude = value;
                RaisePropertyChanged("Latitude");
            }
        }

        public List<CapCount> Capimg
        {
            get
            {
                return _capimg;
            }
            set
            {
                this._capimg = value;
                RaisePropertyChanged("Capimg");
            }
        }

        private byte[] currentcapimg;
        public byte[] CurrentCapimg
        {
            get
            {
                return currentcapimg;
            }
            set
            {
                this.currentcapimg = value;
                RaisePropertyChanged("CurrentCapimg");
            }
        }

        private string _time;
        public string Time
        {
            get
            {
                return _time;
            }
            set
            {
                this._time = value;
                RaisePropertyChanged("Time");
            }
        }

        private string _installDate;
        public string InstallDate
        {
            get
            {
                return _installDate;
            }
            set
            {
                this._installDate = value;
                RaisePropertyChanged("Time");
            }
        }

        #region  PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}
