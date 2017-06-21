using System.Collections.Generic;

namespace DATA.MODELS.SensingModels
{
    public class ConfigRegion
    {
        /// <summary>
        /// region type
        /// or region number
        /// </summary>
        public int RegionNO { get; set; }
        /// <summary>
        /// region name
        /// </summary>
        public string RegionName { get; set; }
        /// <summary>
        /// publish to ip address
        /// </summary>
        public List<string> Hosts { get; set; }
        /// <summary>
        /// alarm number
        /// </summary>
        public int AlarmNO { get; set; }
        /// <summary>
        /// threashold
        /// </summary>
        public int Threshold { get; set; }
    }
}
