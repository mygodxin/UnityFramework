
using System;
namespace DuiChongServerCommon.ClientProtocol
{
    public abstract class CustomActivityModel : IKHSerializable
    {
   
   
   
   
   
   
   
        #region AutoProtocol
        public virtual void Deserialize(BufferReader reader)
        {
        }
        public virtual void Serialize(BufferWriter sender)
        {
        }
        #endregion

    }
    public abstract class CustomActivityData : IKHSerializable
    {
   
   
   
   
   
   
   
        #region AutoProtocol
        public virtual void Deserialize(BufferReader reader)
        {
        }
        public virtual void Serialize(BufferWriter sender)
        {
        }
        #endregion

    }
    //[CodeAnnotation("为题")]
    public class Question : IKHSerializable
    {
        //[CodeAnnotation("题目")]
        public string QuestionStr { get; set; }
        //[CodeAnnotation("答案选项")]
        public SList<string> Answers { get; set; }
        //[CodeAnnotation("正确答案的下标(考虑到可能是多选 所有使用集合)")]
        public SList<int> RightAnswerIndex { get; set; }
        //[CodeAnnotation("答题的时间限制")]
        public int TimeLimit { get; set; }
   
   
   
   
   
   
   
        #region AutoProtocol
        public virtual void Deserialize(BufferReader reader)
        {
            reader.Read(out String _QuestionStr);
            this.QuestionStr= _QuestionStr;
            reader.Read(out SList<String> _Answers);
            this.Answers= _Answers;
            reader.Read(out SList<Int32> _RightAnswerIndex);
            this.RightAnswerIndex= _RightAnswerIndex;
            reader.Read(out Int32 _TimeLimit);
            this.TimeLimit= _TimeLimit;
        }
        public virtual void Serialize(BufferWriter sender)
        {
            sender.Write(QuestionStr);
            sender.Write(Answers);
            sender.Write(RightAnswerIndex);
            sender.Write(TimeLimit);
        }
        #endregion

    }
    public class YuanXiaoModelList : CustomActivityModel
    {
        public SList<YuanXiaoModel> Models { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
   
   
   
   
   
   
   
        #region AutoProtocol
        public override void Deserialize(BufferReader reader)
        {
            reader.Read(out SList<YuanXiaoModel> _Models);
            this.Models= _Models;
            reader.Read(out DateTime _StartDate);
            this.StartDate= _StartDate;
            reader.Read(out DateTime _EndDate);
            this.EndDate= _EndDate;
            base.Deserialize(reader);
        }
        public override void Serialize(BufferWriter sender)
        {
            sender.Write(Models);
            sender.Write(StartDate);
            sender.Write(EndDate);
            base.Serialize(sender);
        }
        #endregion

    }
    //[CodeAnnotation("元宵节数据")]
    public class YuanXiaoModel:IKHSerializable
    {
        //[CodeAnnotation("本难度所有问题")]
        public SList<Question> Questions { get; set; }
        //[CodeAnnotation("可选奖励")]
        public BuyExtraAward Award { get; set; }
        //[CodeAnnotation("购买价格")]
        public Currency Price { get; set; }
   
   
   
   
   
   
   
        #region AutoProtocol
        public virtual void Deserialize(BufferReader reader)
        {
            reader.Read(out SList<Question> _Questions);
            this.Questions= _Questions;
            reader.Read(out BuyExtraAward _Award);
            this.Award= _Award;
            reader.Read(out Currency _Price);
            this.Price= _Price;
        }
        public virtual void Serialize(BufferWriter sender)
        {
            sender.Write(Questions);
            sender.Write(Award);
            sender.Write(Price);
        }
        #endregion

    }
    //[CodeAnnotation("人物的元宵节数据")]
    public class YuanXiaoData: CustomActivityData
    {
        //[CodeAnnotation("答题的状态")]
        public AnswerState CurrnetAnswerState { get; set; }
        //[CodeAnnotation("当前答题数据的Index")]
        public byte CurrentIndex { get; set; }
        public void Pass()
        {
            if (this.CurrentIndex<(TaskConfig.Instance.CustomActivityModels[CustomActivity.元宵节] as YuanXiaoModelList).Models.Count)
            {
                this.CurrentIndex++;
                this.CurrnetAnswerState= AnswerState.None;
            }
        }
        public void Wrong()
        {
            this.CurrnetAnswerState= AnswerState.AnswerWrong;
        }
   
   
   
   
   
   
   
        #region AutoProtocol
        public override void Deserialize(BufferReader reader)
        {
            reader.Read(out byte _CurrnetAnswerState);
            this.CurrnetAnswerState= (AnswerState)_CurrnetAnswerState;
            reader.Read(out Byte _CurrentIndex);
            this.CurrentIndex= _CurrentIndex;
            base.Deserialize(reader);
        }
        public override void Serialize(BufferWriter sender)
        {
            sender.Write((byte)CurrnetAnswerState);
            sender.Write(CurrentIndex);
            base.Serialize(sender);
        }
        #endregion

    }
}
