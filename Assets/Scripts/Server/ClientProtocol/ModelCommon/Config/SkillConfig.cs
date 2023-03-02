

using System;
using System.Collections.Generic;
namespace DuiChongServerCommon.ClientProtocol
{
    public class SkillConfig : ConfigBase<SkillConfig>
    {
        public Dictionary<套装类型, TaoZhuangSkillModel[]> RandomTaoZhuangSkills = new Dictionary<套装类型, TaoZhuangSkillModel[]>();
        protected override void DeserializedInit()
        {
            foreach (var item in TaoZhuangSkillModels)
            {
                var lis = new List<TaoZhuangSkillModel>();
                foreach (var skillModel in item.Value)
                {
                    if (skillModel.LimitCount < 4)
                    {
                        lis.Add(skillModel);
                    }
                }
                RandomTaoZhuangSkills.Add(item.Key, lis.ToArray());
            }
            base.DeserializedInit();
        }
        public int 职业技能最大等级 { get; set; }
        public int 专精开启下一级最低等级限制 { get; set; } = 5;
        //[CodeAnnotation("技能数据")]
        public SDictionary<ushort, SkillModel> SkillModles { get; set; }
        //[CodeAnnotation("职业技能")]
        public SDictionary<兵种职业, SDictionary<ZJDir, SList<SkillModel>>> ZhiyeSkillModels { get; set; }
        //[CodeAnnotation("套装技能")]
        public SDictionary<套装类型, SList<TaoZhuangSkillModel>> TaoZhuangSkillModels { get; set; }
        //[CodeAnnotation("buff数据")]
        public SDictionary<ushort, BuffModel> AllBuff { get; set; }
        /// <summary>
        /// 技能石消耗
        /// </summary>
        //[CodeAnnotation("技能石消耗")]
        public SDictionary<int, double> Cost { get; set; }
   
   
   
   
   
   
   
   
        #region AutoProtocol
        public override void Deserialize(BufferReader reader)
        {
            reader.Read(out Int32 _职业技能最大等级);
            this.职业技能最大等级= _职业技能最大等级;
            reader.Read(out Int32 _专精开启下一级最低等级限制);
            this.专精开启下一级最低等级限制= _专精开启下一级最低等级限制;
            reader.Read(out SDictionary<UInt16,SkillModel> _SkillModles);
            this.SkillModles= _SkillModles;
            reader.Read(out SDictionary<兵种职业,SDictionary<ZJDir,SList<SkillModel>>> _ZhiyeSkillModels);
            this.ZhiyeSkillModels= _ZhiyeSkillModels;
            reader.Read(out SDictionary<套装类型,SList<TaoZhuangSkillModel>> _TaoZhuangSkillModels);
            this.TaoZhuangSkillModels= _TaoZhuangSkillModels;
            reader.Read(out SDictionary<UInt16,BuffModel> _AllBuff);
            this.AllBuff= _AllBuff;
            reader.Read(out SDictionary<Int32,Double> _Cost);
            this.Cost= _Cost;
            base.Deserialize(reader);
        }
        public override void Serialize(BufferWriter sender)
        {
            sender.Write(职业技能最大等级);
            sender.Write(专精开启下一级最低等级限制);
            sender.Write(SkillModles);
            sender.Write(ZhiyeSkillModels);
            sender.Write(TaoZhuangSkillModels);
            sender.Write(AllBuff);
            sender.Write(Cost);
            base.Serialize(sender);
        }
        #endregion

    }
}
