using System;

public class StringUtil
{
    /// <summary>
    /// 获取字符串指定两个字符中间的内容
    /// </summary>
    /// <param name="source"></param>
    /// <param name="startTag"></param>
    /// <param name="endTag"></param>
    /// <returns></returns>
    public static string ContentBetween(string source, string startTag, string endTag)
    {
        int startIndex = source.IndexOf(startTag) + startTag.Length;
        if (startIndex == startTag.Length - 1) // 如果未找到起始标签，返回空字符串
        {
            return string.Empty;
        }

        int endIndex = source.IndexOf(endTag, startIndex);
        if (endIndex == -1) // 如果未找到结束标签，返回空字符串
        {
            return string.Empty;
        }

        return source.Substring(startIndex, endIndex - startIndex);
    }

    /// <summary>
    /// 获取字符串指定字符后的文本
    /// </summary>
    /// <param name="source"></param>
    /// <param name="startTag"></param>
    /// <returns></returns>
    public static string ContentLater(string source, string startTag, bool include = false)
    {
        int index = source.IndexOf(startTag);
        if (index == -1)
        {
            return string.Empty;
        }
        string result = source.Substring(index + (!include ? startTag.Length : 0));
        return result;
    }

    public static string ContentBefore(string source, string endTag)
    {
        int index = source.IndexOf(endTag);
        if (index == -1)
        {
            return string.Empty;
        }
        string result = source.Substring(0, index);
        return result;
    }
}

public class ChineseNumber
{

    private static readonly string[] chineseDigits = { "", "一", "二", "三", "四", "五", "六", "七", "八", "九" };
    private static readonly string[] unitDigits = { "", "十", "百", "千" };
    public static string Convert(int number)
    {
        if (number < 0 || number > 999999999)
        {
            throw new ArgumentException("输入的数字超出范围（0 到 9999）");
        }

        if (number == 0)
        {
            return "零";
        }
        int recordSrcNum = number;//记录下源数字

        string result = "";

        int unitIndex = 0;
        while (number > 0)
        {
            int digit = number % 10;
            if (digit > 0)
            {
                result = chineseDigits[digit] + unitDigits[unitIndex] + result;
            }
            else
            {
                // 处理连续的零
                if (result.Length > 0 && result[0] != '零')
                {
                    result = "零" + result;
                }
            }

            number /= 10;
            unitIndex++;
        }
        if (number >= 10 && number < 20) //单独处理10 - 19，否则会生成一十
        {
            return result.Substring(1);
        }
        return result;
    }
}