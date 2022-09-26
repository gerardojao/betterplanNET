using System;
using System.Linq;
using ApiBase.Models;
using Microsoft.Extensions.Configuration;

namespace ApiBase.Data
{
    public class NormalizedDateTime
    {

        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        public NormalizedDateTime(AppDbContext context, IConfiguration configuration)
        {
            _configuration = configuration;
            _context = context;
        }

        public DateTime DT()
        {
            TimeZoneInfo setTimeZoneInfo;
            DateTime currentDateTime;
            setTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(_configuration["ServerSettings:GMTLocal"]);

            //Get date and time in US Mountain Standard Time
            currentDateTime = TimeZoneInfo.ConvertTime(DateTime.Now, setTimeZoneInfo);
            return currentDateTime;
        }

        public DateTime Today()
        {
            TimeZoneInfo setTimeZoneInfo;
            DateTime currentDateTime;
            setTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(_configuration["ServerSettings:GMTLocal"]);
            currentDateTime = TimeZoneInfo.ConvertTime(DateTime.Now, setTimeZoneInfo);
            return currentDateTime.Date;
        }
        public DateTime Tomorrow()
        {
            TimeZoneInfo setTimeZoneInfo;
            DateTime currentDateTime;
            var today = DateTime.Now;
            var tomorrow = today.AddDays(1);
            setTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(_configuration["ServerSettings:GMTLocal"]);
            currentDateTime = TimeZoneInfo.ConvertTime(tomorrow, setTimeZoneInfo);
            return currentDateTime.Date;
        }


        public DateTime UtcToServerTime(DateTime time)
        {
            TimeZoneInfo localZone = TimeZoneInfo.Local;
            TimeSpan currentOffset = localZone.GetUtcOffset( time );

            if ( Convert.ToInt32(_configuration["ServerSettings:GMTLocal"]) == currentOffset.Hours) return time;
        
            return time.AddHours(Convert.ToDouble(_configuration["ServerSettings:GMTLocal"]));
        }

        public static long UTCDateTimeToUnix(DateTime time)
        {

            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            TimeSpan span = (time-epoch);
            return (long) span.TotalSeconds;
        }

        public static DateTime UnixToUTCDateTime(long time)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(time);
        }        

           
    }
}
