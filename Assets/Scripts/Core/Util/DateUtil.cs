//using System.Text.RegularExpressions;
//using System;
//using Unity.VisualScripting;
//using UnityEngine;
//using UnityEngine.UIElements;

///// <summary>
///// 时间相关工具类
///// </summary>
//public class DateUtil
//{

///**格式化日期 xx天xx时xx分xx秒 */
//public static FormatDate1(DateTime timestamp)
//{
//        Date d = new Date();
//    var expireTime = timestamp;
//    var expireDay = 0;
//    var str = '';
//    if (expireTime <= 0)
//    {
//        // 已经过期
//    }
//    else if (expireTime > 0 && expireTime <= 60)
//    {
//        // 秒
//        str = Math.floor(expireTime / 60) + '秒' + str;
//    }
//    else if (expireTime > 60 && expireTime <= 60 * 60)
//    {
//        // 分
//        str = Math.floor(expireTime / (60 * 60)) + '分' + str;
//    }
//    else if (expireTime > 60 * 60 && expireTime <= 60 * 60 * 24)
//    {
//        // 时 
//        str = Math.floor(expireTime / (60 * 60 * 24)) + '时' + str;
//    }
//    else if (expireTime > 60 * 60 * 24)
//    {
//        // 天   
//        str = Math.floor(expireTime / (60 * 60 * 24)) + '天' + str;
//    }
//    return str;
//}
///**
// * 将整数秒转换为00:00:00格式的格式字符串
// * @param sec 秒数（整数秒）
// * @param showHour 是否显示“小时”位，x:x:x/x:x
// * @param pad 小于10时是否使用数字0补位
// * @returns 
// */
//public static getTimeStr(sec: number, showHour: boolean, pad: boolean): string
//{
//    if (sec == null || isNaN(sec) || sec < 0)
//    {
//        sec = 0;
//    }
//    sec = Math.floor(sec);
//    const snds: number = sec % 60;
//    const tm: number = Math.floor(sec / 60);
//    const minutes = tm % 60;
//    const hour: number = Math.floor(tm / 60);
//    const arr = showHour?[hour, minutes, snds] : [minutes, snds] ;
//    if (pad)
//    {
//        arr.forEach((v, idx) =>
//        {
//            if (v < 10)
//            {
//                arr[idx] = Util.padLeft(v, 2, "0");
//            }
//        })

//        }
//    const str = arr.join(':');
//    return str;
//}
//}
