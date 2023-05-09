using System;

namespace HS
{
    /// <summary>
    /// 时间相关工具类
    /// </summary>
    public class DateUtil
    {
        /// <summary>
        /// 获取当前时间戳(ms)
        /// </summary>
        public static long Now
        {
            get
            {
                return (long)(DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1)).TotalMilliseconds;
            }
        }
        /**格式化日期 xx天xx时xx分xx秒 */
        public static string FormatDate(double time)
        {
            var str = "";
            if (time <= 0) { }
            else if (time > 0 && time <= 60)
            {
                str = Math.Floor(time / 60) + '秒' + str;
            }
            else if (time > 60 && time <= 60 * 60)
            {
                str = Math.Floor(time / (60 * 60)) + '分' + str;
            }
            else if (time > 60 * 60 && time <= 60 * 60 * 24)
            {
                str = Math.Floor(time / (60 * 60 * 24)) + '时' + str;
            }
            else if (time > 60 * 60 * 24)
            {
                str = Math.Floor(time / (60 * 60 * 24)) + '天' + str;
            }
            return str;
        }
        /// <summary>
        /// 秒转换为00:00:00格式的格式字符串
        /// </summary>
        /// <param name="sec"></param>
        /// <returns></returns>
        public static string FormatHMS(int duration)
        {
            return new TimeSpan(0,0,duration).ToString(@"hh\:mm\:ss");
        }
    }
}
