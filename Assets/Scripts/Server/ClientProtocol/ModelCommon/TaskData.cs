
using KHCore.Utils;
using System;
using UnityEngine;

namespace DuiChongServerCommon.ClientProtocol
{
    public class Range : IKHSerializable
    {
        public static Range CreatByMiddle(decimal middle, decimal bili)
        {
            if (bili > 1)
            {
                throw new Exception("比例不可以>1");
            }
            var offset = middle * bili;
            var min = middle - offset;
            var max = middle + offset;
            return new Range((double)min, (double)max);
        }
        public double Min { get; private set; }
        public double Max { get; private set; }
        public Range(double min, double max)
        {
            this.Min = Math.Min(min, max);
            this.Max = Math.Max(min, max);
        }
        public Range()
        {
        }
        public static Range Plus(Range data1, Range data2)
        {
            if (data1 == null || data2 == null)
            {
                return null;
            }
            var middle1 = (data1.Max + data1.Min) / 2;
            var middle2 = (data2.Max + data2.Min) / 2;
            var newMiddle = middle1 + middle2;
            var fenliXishu1 = (middle1 - data1.Min) / middle1;
            var fenliXishu2 = (middle2 - data2.Min) / middle2;
            var newXishu = (fenliXishu1 + fenliXishu2) / 2;
            var newMin = newMiddle - (newMiddle * newXishu);
            var newMax = newMiddle + (newMiddle * newXishu);
            return new Range(newMin, newMax);
        }

        public double Middle => (this.Max + this.Min) / 2;
        public static Range Parse(string str)
        {
            var range = new Range();
            if (double.TryParse(str, out var value))
            {
                range.Min = range.Max = Math.Round(value, 0);
                return range;
            }
            var strs = str.Split('-');
            var v1 = double.Parse(strs[0]);
            var v2 = double.Parse(strs[1]);
            range.Min = Math.Min(v1, v2);
            range.Max = Math.Max(v1, v2);
            return range;
        }
        public bool Include(double value)
        {
            return value >= Min && value <= Max;
        }
        public bool Between(double value)
        {
            return value > Min && value < Max;
        }
        public double Random()
        {
            return 1;
           // return MathTool.Random(Min, Max);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="coefficient">0-1</param>
        /// <returns></returns>
        public double GetByCoefficient(double coefficient)
        {
            if (coefficient == 0)
                return this.Min;
            else
                return this.Min + (this.Max - this.Min) * coefficient;
        }
        public override string ToString()
        {
            return Min == Max ? Min.ToString() : $"{Min}-{Max}";
        }
   
   
   
   
   
   
   
        #region AutoProtocol
        public virtual void Deserialize(BufferReader reader)
        {
            reader.Read(out Double _Min);
            this.Min= _Min;
            reader.Read(out Double _Max);
            this.Max= _Max;
        }
        public virtual void Serialize(BufferWriter sender)
        {
            sender.Write(Min);
            sender.Write(Max);
        }
        #endregion

    }
    public class TaskModel : AMoldeDataBase
    {
        //[CodeAnnotation("任务类型")]
        public TaskType TaskType { get; set; }
        //[CodeAnnotation("完成次数")]
        public uint CompalteCount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        //[CodeAnnotation("任务版本")]
        public string Version { get; set; }
        public GameTask CreatTask()
        {
            var task = new GameTask()
            {
                Pid = this.Pid,
                GetAward = false,
                CommplateCount = 0,
                Version= this.Version,
            };
            switch (TaskType)
            {
                case TaskType.每日任务:
                    task.AwardIndex = (byte)TaskConfig.Instance.DayAwards.RandomIndex();
                    break;
                case TaskType.每周任务:
                    task.AwardIndex = (byte)TaskConfig.Instance.WeekAwards.RandomIndex();
                    break;
                case TaskType.购买任务:
                    task.AwardIndex = 0;
                    break;
                case TaskType.限时任务:
                    task.AwardIndex = 0;
                    break;
                default:
                    break;
            }
            return task;
        }
   
   
   
   
   
   
   
        #region AutoProtocol
        public override void Deserialize(BufferReader reader)
        {
            reader.Read(out byte _TaskType);
            this.TaskType= (TaskType)_TaskType;
            reader.Read(out UInt32 _CompalteCount);
            this.CompalteCount= _CompalteCount;
            reader.Read(out DateTime _StartDate);
            this.StartDate= _StartDate;
            reader.Read(out DateTime _EndDate);
            this.EndDate= _EndDate;
            reader.Read(out String _Version);
            this.Version= _Version;
            base.Deserialize(reader);
        }
        public override void Serialize(BufferWriter sender)
        {
            sender.Write((byte)TaskType);
            sender.Write(CompalteCount);
            sender.Write(StartDate);
            sender.Write(EndDate);
            sender.Write(Version);
            base.Serialize(sender);
        }
        #endregion

    }

    //[CodeAnnotation("玩家的任务数据")]
    public class TaskData : IKHSerializable
    {
        public static TaskData CreatNew()
        {
            return new TaskData()
            {
                DayTask = new SList<GameTask>(),
                Signed = false,
                SignValue = 0,
                WeekTask = new SList<GameTask>(),
                ChengJiuTask = new SList<GameTask>(),
                BuyTask = new SList<GameTask>(),
            };
        }
        //[CodeAnnotation("日常")]
        public SList<GameTask> DayTask { get; set; }
        //[CodeAnnotation("周常")]
        public SList<GameTask> WeekTask { get; set; }
        //[CodeAnnotation("成就")]
        public SList<GameTask> ChengJiuTask { get; set; }
        //[CodeAnnotation("购买")]
        public SList<GameTask> BuyTask { get; set; }
        //[CodeAnnotation("限时")]
        public SList<GameTask> DateTask { get; set; }
        //[CodeAnnotation("签到次数")]
        public ushort SignValue { get; set; }
        //[CodeAnnotation("当天是否已签到")]
        public bool Signed { get; set; }
        //[CodeAnnotation("高级通行证是否购买")]
        public bool TXZVip { get; set; } = false;
        public ushort TongXingZhengAwardLevel { get; set; }
        public ushort VipTongXingZhengAwardLevel { get; set; }

        //[CodeAnnotation("通行证到期时间")]
        public DateTime TXZDate { get; set; }

        //[CodeAnnotation("购买任务到期时间")]
        public DateTime BuyTaskDate { get; set; }

        public int BuyTaskIndex { get; set; }
        public void RefeshSeason()
        {
            VipTongXingZhengAwardLevel = 0;
            TongXingZhengAwardLevel = 0;
        }
   
   
   
   
   
   
   
        #region AutoProtocol
        public virtual void Deserialize(BufferReader reader)
        {
            reader.Read(out SList<GameTask> _DayTask);
            this.DayTask= _DayTask;
            reader.Read(out SList<GameTask> _WeekTask);
            this.WeekTask= _WeekTask;
            reader.Read(out SList<GameTask> _ChengJiuTask);
            this.ChengJiuTask= _ChengJiuTask;
            reader.Read(out SList<GameTask> _BuyTask);
            this.BuyTask= _BuyTask;
            reader.Read(out SList<GameTask> _DateTask);
            this.DateTask= _DateTask;
            reader.Read(out UInt16 _SignValue);
            this.SignValue= _SignValue;
            reader.Read(out Boolean _Signed);
            this.Signed= _Signed;
            reader.Read(out Boolean _TXZVip);
            this.TXZVip= _TXZVip;
            reader.Read(out UInt16 _TongXingZhengAwardLevel);
            this.TongXingZhengAwardLevel= _TongXingZhengAwardLevel;
            reader.Read(out UInt16 _VipTongXingZhengAwardLevel);
            this.VipTongXingZhengAwardLevel= _VipTongXingZhengAwardLevel;
            reader.Read(out DateTime _TXZDate);
            this.TXZDate= _TXZDate;
            reader.Read(out DateTime _BuyTaskDate);
            this.BuyTaskDate= _BuyTaskDate;
        }
        public virtual void Serialize(BufferWriter sender)
        {
            sender.Write(DayTask);
            sender.Write(WeekTask);
            sender.Write(ChengJiuTask);
            sender.Write(BuyTask);
            sender.Write(DateTask);
            sender.Write(SignValue);
            sender.Write(Signed);
            sender.Write(TXZVip);
            sender.Write(TongXingZhengAwardLevel);
            sender.Write(VipTongXingZhengAwardLevel);
            sender.Write(TXZDate);
            sender.Write(BuyTaskDate);
        }
        #endregion

    }

    public class GameTask : IKHSerializable
    {
        public ushort Pid { get; set; }
        public uint CommplateCount { get; set; }
        public bool GetAward { get; set; }
        public byte AwardIndex { get; set; }

        public string Version { get; set; }

        public decimal BaiFenBi
        {
            get
            {
                var modle = TaskConfig.Instance.Tasks[Pid];
                return Math.Clamp(CommplateCount / (decimal)modle.CompalteCount, 0, 1);
            }
        }
        public bool Complated()
        {
            var modle = TaskConfig.Instance.Tasks[Pid];
            if (CommplateCount > modle.CompalteCount)
            {
                CommplateCount = (modle.CompalteCount);
            }
            return CommplateCount >= modle.CompalteCount;
        }
        public Award Award()
        {
            var modle = TaskConfig.Instance.Tasks[Pid];
            switch (modle.TaskType)
            {
                case TaskType.每日任务:
                    if (this.AwardIndex >= TaskConfig.Instance.DayAwards.Count)
                        this.AwardIndex = (byte)TaskConfig.Instance.DayAwards.RandomIndex();
                    return TaskConfig.Instance.DayAwards[AwardIndex];
                case TaskType.每周任务:
                    if (this.AwardIndex >= TaskConfig.Instance.WeekAwards.Count)
                        this.AwardIndex = (byte)TaskConfig.Instance.WeekAwards.RandomIndex();
                    return TaskConfig.Instance.WeekAwards[AwardIndex];
            }
            return default;
        }
   
   
   
   
   
   
   
        #region AutoProtocol
        public virtual void Deserialize(BufferReader reader)
        {
            reader.Read(out UInt16 _Pid);
            this.Pid= _Pid;
            reader.Read(out UInt32 _CommplateCount);
            this.CommplateCount= _CommplateCount;
            reader.Read(out Boolean _GetAward);
            this.GetAward= _GetAward;
            reader.Read(out Byte _AwardIndex);
            this.AwardIndex= _AwardIndex;
        }
        public virtual void Serialize(BufferWriter sender)
        {
            sender.Write(Pid);
            sender.Write(CommplateCount);
            sender.Write(GetAward);
            sender.Write(AwardIndex);
        }
        #endregion

    }
    //[CodeAnnotation("积分奖励")]
    public class ScoreAward : Award
    {
        //[CodeAnnotation("积分限制")]
        public uint ScoreLimit { get; set; }
   
   
   
   
   
   
   
        #region AutoProtocol
        public override void Deserialize(BufferReader reader)
        {
            reader.Read(out UInt32 _ScoreLimit);
            this.ScoreLimit= _ScoreLimit;
            base.Deserialize(reader);
        }
        public override void Serialize(BufferWriter sender)
        {
            sender.Write(ScoreLimit);
            base.Serialize(sender);
        }
        #endregion

    }
}
