
using KHCore.Utils;
using System;
using System.Collections.Generic;
namespace DuiChongServerCommon.ClientProtocol
{
    //[CodeAnnotation("货币")]
    public class Currency : IKHSerializable
    {
        public Currency()
        {
            CurrencyType = default;
            Value = default;
        }
        public Currency(CurrencyType type, double value)
        {
            CurrencyType = type;
            Value = value;
        }
        public CurrencyType CurrencyType { get; private set; }
        public double Value { get; private set; }
        /// <summary>
        /// 读取货币 格式 货币1=100|货币2=100|....
        /// </summary>
        /// <param name="content"></param>
        public static Currency Parse(string str)
        {
            if (str == "0")
            {
                return new Currency();
            }
            var strs = str.Trim().Split("=");
            var type = Enum.Parse<CurrencyType>(strs[0]);
            var value = double.Parse(strs[1]);
            return new Currency() { CurrencyType = type, Value = value };
        }
        /// <summary>
        /// 读取货币数组 格式 货币1=100|货币2=100|....
        /// </summary>
        /// <param name="content"></param>
        public static List<Currency> ParseList(string content)
        {
            content = content.Trim();
            var lis = new List<Currency>();
            if (!string.IsNullOrEmpty(content))
            {
                var strs = content.Split('|');
                foreach (var item in strs)
                {
                    lis.Add(Parse(item));
                }
            }
            return lis;
        }

        public void SetValue(CurrencyType type, double value)
        {
            this.Value = value;
            this.CurrencyType = type;
        }
        public override string ToString()
        {
            return Value == 0 ? "0" : $"{CurrencyType} = {Value}";
        }
   
   
   
   
   
   
   
        #region AutoProtocol
        public virtual void Deserialize(BufferReader reader)
        {
            reader.Read(out byte _CurrencyType);
            this.CurrencyType= (CurrencyType)_CurrencyType;
            reader.Read(out Double _Value);
            this.Value= _Value;
        }
        public virtual void Serialize(BufferWriter sender)
        {
            sender.Write((byte)CurrencyType);
            sender.Write(Value);
        }
        #endregion

    }
}
