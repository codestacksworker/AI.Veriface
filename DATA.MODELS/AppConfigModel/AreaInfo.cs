namespace AppConfigModel
{
    public class AreaInfo
    {
        public string Account { get; set; }
        public int Flag { get; set; }
        public string CameraName { get; set; }
        public IpAddress ReceivedClientIp { get; set; }

        #region 自动推送用
        public int AlarmNum { get; set; }
        public double Threshold { get; set; }
        #endregion
    }

    public class IpAddress
    {
        public string Ip { get; set; }
        public int Port { get; set; }
    }
}
