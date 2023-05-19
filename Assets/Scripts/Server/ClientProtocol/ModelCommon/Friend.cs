using System;

namespace DuiChongServerCommon.ClientProtocol
{
    public class Friend : IKHSerializable
    {
        public int ID { get; set; }
        public string AvatarUrl { get; set; }
        public string Name { get; set; }
        public int Mission { get; set; }
        public int AreanScore { get; set; }
        public bool IsOnline { get; set; }
   
   
   
   
   
   
   
        #region AutoProtocol
        public virtual void Deserialize(BufferReader reader)
        {
            reader.Read(out Int32 _ID);
            this.ID= _ID;
            reader.Read(out String _AvatarUrl);
            this.AvatarUrl= _AvatarUrl;
            reader.Read(out String _Name);
            this.Name= _Name;
            reader.Read(out Int32 _Mission);
            this.Mission= _Mission;
            reader.Read(out Int32 _AreanScore);
            this.AreanScore= _AreanScore;
            reader.Read(out Boolean _IsOnline);
            this.IsOnline= _IsOnline;
        }
        public virtual void Serialize(BufferWriter sender)
        {
            sender.Write(ID);
            sender.Write(AvatarUrl);
            sender.Write(Name);
            sender.Write(Mission);
            sender.Write(AreanScore);
            sender.Write(IsOnline);
        }
        #endregion

    }
}
