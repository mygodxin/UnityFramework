using System;


namespace DuiChongServerCommon.ClientProtocol
{
    public class Mail : IKHSerializable
    {
        public uint ID { get; set; }
        public string Tittle { get; set; }
        public uint SenderID { get; set; }
        public DateTime CreatTime { get; set; }
        public string Message { get; set; }
        public Award Award { get; set; }
        public bool Geted { get; set; } = false;
        public bool Readed { get; set; } = false;
   
   
   
   
   
   
   
        #region AutoProtocol
        public virtual void Deserialize(BufferReader reader)
        {
            reader.Read(out UInt32 _ID);
            this.ID= _ID;
            reader.Read(out String _Tittle);
            this.Tittle= _Tittle;
            reader.Read(out UInt32 _SenderID);
            this.SenderID= _SenderID;
            reader.Read(out DateTime _CreatTime);
            this.CreatTime= _CreatTime;
            reader.Read(out String _Message);
            this.Message= _Message;
            reader.Read(out Award _Award);
            this.Award= _Award;
            reader.Read(out Boolean _Geted);
            this.Geted= _Geted;
            reader.Read(out Boolean _Readed);
            this.Readed= _Readed;
        }
        public virtual void Serialize(BufferWriter sender)
        {
            sender.Write(ID);
            sender.Write(Tittle);
            sender.Write(SenderID);
            sender.Write(CreatTime);
            sender.Write(Message);
            sender.Write(Award);
            sender.Write(Geted);
            sender.Write(Readed);
        }
        #endregion

    }
}
