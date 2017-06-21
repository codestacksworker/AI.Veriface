using log4net;

namespace DATA.UTILITIES.Log4Net
{
    public class Logger<T> where T : class
    {
        public static ILog Log
        {
            get { return LogManager.GetLogger(typeof(T)); }
        }

        private Logger() { }

        public static void InitLogger()
        {
            log4net.Config.XmlConfigurator.Configure();
        }
    }
}
