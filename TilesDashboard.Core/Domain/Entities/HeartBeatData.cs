using System;
using TilesDashboard.Core.Type;
using TilesDashboard.Core.Type.TileData;

namespace TilesDashboard.Core.Domain.Entities
{
    public class HeartBeatData : ITileData
    {
        public HeartBeatData(int responseTime, string appVersion, string additionalInfo, DateTimeOffset addedOn)
           : this(responseTime, appVersion, addedOn)
        {
            AdditionalInfo = additionalInfo;
        }

        public HeartBeatData(int responseTime, string appVersion, DateTimeOffset addedOn)
            : this(responseTime, addedOn)
        {
            AppVersion = appVersion;
        }

        public HeartBeatData(int responseTime, DateTimeOffset addedOn)
        {
            ResponseTimeInMs = responseTime;
            AddedOn = addedOn;
        }

        /// <summary>
        /// Application response time in ms. -1 means that application is unavailable.
        /// </summary>
        public int ResponseTimeInMs { get; private set; }

        public string AppVersion { get; private set; }

        public string AdditionalInfo { get; private set; }

        public DateTimeOffset AddedOn { get; }

        public static HeartBeatData Unavailable(DateTimeOffset addedOn) => new HeartBeatData(-1, addedOn);

        public ValueObject GetData()
        {
            throw new NotImplementedException();
        }
    }
}
