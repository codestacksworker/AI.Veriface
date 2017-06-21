using System;
using System.Windows.Media;

namespace PeopleModel
{
    public partial class Camera
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public object Coordinate { get; set; }
        public string Brand { get; set; }
        public DateTime InstallDate { get; set; }
        public decimal SnapPersonCount { get; set; }
        public string SnapPersonCountStr { get; set; }
        public ImageSource SnapPhoto { get; set; }
        public byte[] SnapPhotoByteArray { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
    }
}
