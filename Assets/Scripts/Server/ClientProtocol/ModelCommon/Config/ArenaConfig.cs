using System;
using System.Linq;

namespace DuiChongServerCommon.ClientProtocol
{
    public class ArenaData : IKHSerializable
    {
        //[CodeAnnotation("分数区间")]
        public Range Range { get; set; }
        //[CodeAnnotation("对应段位")]
        public 竞技段位 Level { get; set; }
        //[CodeAnnotation("失败惩罚")]
        public uint ScoreFailed { get; set; }
        //[CodeAnnotation("积分奖励")]
        public uint JiFen { get; set; }
        public Range RobotPower { get; set; }
   
   
   
   
   
   
   
        #region AutoProtocol
        public virtual void Deserialize(BufferReader reader)
        {
            reader.Read(out Range _Range);
            this.Range= _Range;
            reader.Read(out byte _Level);
            this.Level= (竞技段位)_Level;
            reader.Read(out UInt32 _ScoreFailed);
            this.ScoreFailed= _ScoreFailed;
            reader.Read(out UInt32 _JiFen);
            this.JiFen= _JiFen;
            reader.Read(out Range _RobotPower);
            this.RobotPower= _RobotPower;
        }
        public virtual void Serialize(BufferWriter sender)
        {
            sender.Write(Range);
            sender.Write((byte)Level);
            sender.Write(ScoreFailed);
            sender.Write(JiFen);
            sender.Write(RobotPower);
        }
        #endregion

    }

    public class ArenaConfig : ConfigBase<ArenaConfig>
    {
        protected override void DeserializedInit()
        {
            var lis = ArenaDatas.Values.ToList();
            lis.Sort((a, b) => { return (int)(a.Range.Max - b.Range.Max); });
            ArenaDatasArr = lis.ToArray();
            Awards.Sort((a, b) => { return (int)(a.ScoreLimit - b.ScoreLimit); });
        }

        ArenaData[] ArenaDatasArr;
        //[CodeAnnotation("竞技等级对应数据")]
        public SDictionary<竞技段位, ArenaData> ArenaDatas { get; set; }
        public uint 每日积分奖励次数 { get; set; }
        public uint 胜利分数增加 { get; set; }
        //[CodeAnnotation("分段奖励")]
        public SList<ScoreAward> Awards { get; set; }
        public 竞技段位 GetLevelByScore(int score)
        {
            if (score == 0)
            {
                return 竞技段位.青铜五阶;
            }
            for (int i = 0; i < ArenaDatasArr.Length; i++)
            {
                var data = ArenaDatasArr[i];
                if (data.Range.Include(score))
                {
                    return data.Level;
                }
            }
            return 竞技段位.王者;
        }
        public ArenaData GetArenaDataByScore(int score)
        {
            return ArenaDatas[GetLevelByScore(score)];
        }
   
   
   
   
   
   
   
   
        #region AutoProtocol
        public override void Deserialize(BufferReader reader)
        {
            reader.Read(out SDictionary<竞技段位,ArenaData> _ArenaDatas);
            this.ArenaDatas= _ArenaDatas;
            reader.Read(out UInt32 _每日积分奖励次数);
            this.每日积分奖励次数= _每日积分奖励次数;
            reader.Read(out UInt32 _胜利分数增加);
            this.胜利分数增加= _胜利分数增加;
            reader.Read(out SList<ScoreAward> _Awards);
            this.Awards= _Awards;
            base.Deserialize(reader);
        }
        public override void Serialize(BufferWriter sender)
        {
            sender.Write(ArenaDatas);
            sender.Write(每日积分奖励次数);
            sender.Write(胜利分数增加);
            sender.Write(Awards);
            base.Serialize(sender);
        }
        #endregion

    }
}
