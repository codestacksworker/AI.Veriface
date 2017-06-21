namespace AppConfigModel
{
    public class ConfigContent
    {
        public ConfigText AreaJsonText { get; set; }
        public ConfigText RealtimeCaptureJsonText { get; set; }
        public ConfigText PortJsonText { get; set; }
    }

    public class ConfigText
    {
        public string JsonConfigText { get; set; }
    }
}
