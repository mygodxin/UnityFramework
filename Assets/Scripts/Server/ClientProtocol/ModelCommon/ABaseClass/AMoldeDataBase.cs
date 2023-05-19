using System;

namespace DuiChongServerCommon.ClientProtocol
{
    public class AMoldeDataBase : IKHSerializable
    {
        //[CodeAnnotation("Pid")]
        public ushort Pid { get; set; }
        //[CodeAnnotation("描述")]
        public string Explain { get; set; }
        //[CodeAnnotation("名称")]
        public string Name { get; set; }
        //[CodeAnnotation("美术ID")]
        public ushort TextureID { get; set; }
   
   
   
   
   
   
   
        #region AutoProtocol
        public virtual void Deserialize(BufferReader reader)
        {
            reader.Read(out UInt16 _Pid);
            this.Pid= _Pid;
            reader.Read(out String _Explain);
            this.Explain= _Explain;
            reader.Read(out String _Name);
            this.Name= _Name;
            reader.Read(out UInt16 _TextureID);
            this.TextureID= _TextureID;
        }
        public virtual void Serialize(BufferWriter sender)
        {
            sender.Write(Pid);
            sender.Write(Explain);
            sender.Write(Name);
            sender.Write(TextureID);
        }
        #endregion

    }
}
