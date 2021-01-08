using System;

namespace HandControl.Services
{
    public static class UnixTimestampConverter
    {
        /// <summary>
        ///     Выполняет преобразование DateTime в Unix timestamp.
        /// </summary>
        /// <returns>Unix time.</returns>
        public static long DateTimeToUnix(DateTime time)
        {
            var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var diff = time.ToUniversalTime() - origin;
            return (long)Math.Floor(diff.TotalSeconds);
        }

        /// <summary>
        ///     Выполняет установку последнего времени изменения из unix времени.
        /// </summary>
        public static DateTime DateTimeFromUnix(long dateTime)
        {
            var outer = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var time = outer.AddSeconds(dateTime).ToLocalTime();
            return time;
        }
    }
}
