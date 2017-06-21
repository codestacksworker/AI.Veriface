namespace DATA.UTILITIES.FileHandler
{
    public abstract class FileName
    {
        /// <summary>
        /// CONFIG.json
        /// </summary>
        public static string fileName
        {
            get { return "CONFIG.json"; }
        }

        /// <summary>
        /// JSON
        /// </summary>
        public static string dirName
        {
            get { return "JSON"; }
        }

        /// <summary>
        /// \\
        /// </summary>
        public static string backslash
        {
            get { return @"\\"; }
        }

        public static string backslashSingle
        {
            get { return @"\"; }
        }
    }
}
