using System.Windows.Media;

namespace FaceSysByMvvm.Model
{
    public class CompOfRecordTemplate
    {
        public string TemplateGuid { get; set; }
        public string UserName { get; set; }
        public string TemplateType { get; set; }
        public string LikeScore { get; set; }
        public byte[] TemplateImageBuffer { get; set; }
        public ImageSource TemplateImage { get; set; }
    }
}
