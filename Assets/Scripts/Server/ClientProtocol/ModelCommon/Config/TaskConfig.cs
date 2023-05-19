using System;
using System.Collections.Generic;
using System.Text;

namespace DuiChongServerCommon.ClientProtocol
{
    //[CodeAnnotation("通行证奖励")]
    public class TongXiangZhengAward : IKHSerializable
    {
        //[CodeAnnotation("普通通行证奖励")]
        public Award Low { get; set; }
        //[CodeAnnotation("高级通行证奖励")]
        public OptionalAwards Hight { get; set; }
        //[CodeAnnotation("积分要求")]
        public int Score { get; set; }
   
   
   
   
   
   
   
        #region AutoProtocol
        public virtual void Deserialize(BufferReader reader)
        {
            reader.Read(out Award _Low);
            this.Low= _Low;
            reader.Read(out OptionalAwards _Hight);
            this.Hight= _Hight;
            reader.Read(out Int32 _Score);
            this.Score= _Score;
        }
        public virtual void Serialize(BufferWriter sender)
        {
            sender.Write(Low);
            sender.Write(Hight);
            sender.Write(Score);
        }
        #endregion

    }
    public class TaskConfig : ConfigBase<TaskConfig>
    {
        public TaskModel[] DayTasks { get; private set; }
        public TaskModel[] WeekTasks { get; private set; }
        public TaskModel[] ChengJiuTasks { get; private set; }
        private TaskModel[] BuyTasks_1 { get; set; }
        private TaskModel[] BuyTasks_2 { get; set; }
        public TaskModel[] DateTasks { get; set; }
        public TaskModel[] GetBuyTasks(int buyIndex)
        {
            return buyIndex % 2 == 0 ? BuyTasks_1 : BuyTasks_2;
        }
        protected override void DeserializedInit()
        {
            var lis = new List<TaskModel>();
            var lis2 = new List<TaskModel>();
            var lis3 = new List<TaskModel>();
            var lis4_1 = new List<TaskModel>();
            var lis4_2 = new List<TaskModel>();
            var list5 = new List<TaskModel>();
            foreach (var item in Tasks.Values)
            {
                switch (item.TaskType)
                {
                    case TaskType.每日任务:
                        lis.Add(item);
                        break;
                    case TaskType.每周任务:
                        lis2.Add(item);
                        break;
                    case TaskType.成就任务:
                        lis3.Add(item);
                        break;
                    case TaskType.购买任务:
                        if (item.Name.Contains("招募"))
                        {
                            lis4_2.Add(item);
                        }
                        else
                        {
                            lis4_1.Add(item);
                        }
                        break;
                    case TaskType.限时任务:
                        list5.Add(item);
                        break;
                    default:
                        break;
                }
            }
            DayTasks = lis.ToArray();
            WeekTasks = lis2.ToArray();
            ChengJiuTasks = lis3.ToArray();
            this.BuyTasks_1 = lis4_1.ToArray();
            this.BuyTasks_2 = lis4_2.ToArray();
            this.DateTasks = list5.ToArray();
            base.DeserializedInit();
        }
        //[CodeAnnotation("任务数据")]
        public SDictionary<ushort, TaskModel> Tasks { get; set; }
        //[CodeAnnotation("每日任务奖励")]
        public SList<Award> DayAwards { get; set; }
        //[CodeAnnotation("每周任务奖励")]
        public SList<Award> WeekAwards { get; set; }
        //[CodeAnnotation("签到奖励")]
        public SList<Award> SginAwards { get; set; }
        //[CodeAnnotation("购买任务奖励")]
        public SDictionary<ushort, BuyExtraAward> BuyAwards { get; set; }
        //[CodeAnnotation("限时任务奖励")]
        public SDictionary<ushort, BuyExtraAward> DateTaskAward { get; set; }
        //[CodeAnnotation("通行证奖励")]
        public SList<TongXiangZhengAward> TongxingzhenAward { get; set; }
        public int 每日任务数量 { get; set; }
        public int 每周任务数量 { get; set; }
        public Currency 高级通行证价格 { get; set; }
        /// <summary>
        /// 自定义活动数据
        /// </summary>
        public SDictionary<CustomActivity, CustomActivityModel> CustomActivityModels { get;  set; }
   
   
   
   
   
   
   
        #region AutoProtocol
        public override void Deserialize(BufferReader reader)
        {
            reader.Read(out SDictionary<UInt16,TaskModel> _Tasks);
            this.Tasks= _Tasks;
            reader.Read(out SList<Award> _DayAwards);
            this.DayAwards= _DayAwards;
            reader.Read(out SList<Award> _WeekAwards);
            this.WeekAwards= _WeekAwards;
            reader.Read(out SList<Award> _SginAwards);
            this.SginAwards= _SginAwards;
            reader.Read(out SDictionary<UInt16,BuyExtraAward> _BuyAwards);
            this.BuyAwards= _BuyAwards;
            reader.Read(out SDictionary<UInt16,BuyExtraAward> _DateTaskAward);
            this.DateTaskAward= _DateTaskAward;
            reader.Read(out SList<TongXiangZhengAward> _TongxingzhenAward);
            this.TongxingzhenAward= _TongxingzhenAward;
            reader.Read(out Int32 _每日任务数量);
            this.每日任务数量= _每日任务数量;
            reader.Read(out Int32 _每周任务数量);
            this.每周任务数量= _每周任务数量;
            reader.Read(out Currency _高级通行证价格);
            this.高级通行证价格= _高级通行证价格;
            reader.Read(out SDictionary<CustomActivity,CustomActivityModel> _CustomActivityModels);
            this.CustomActivityModels= _CustomActivityModels;
            base.Deserialize(reader);
        }
        public override void Serialize(BufferWriter sender)
        {
            sender.Write(Tasks);
            sender.Write(DayAwards);
            sender.Write(WeekAwards);
            sender.Write(SginAwards);
            sender.Write(BuyAwards);
            sender.Write(DateTaskAward);
            sender.Write(TongxingzhenAward);
            sender.Write(每日任务数量);
            sender.Write(每周任务数量);
            sender.Write(高级通行证价格);
            sender.Write(CustomActivityModels);
            base.Serialize(sender);
        }
        #endregion

    }
}
