
using System.Threading;
namespace DuiChongServerCommon.ClientProtocol
{
    public class ConfigBase<T> : IKHSerializable where T : ConfigBase<T>
    {
        private static T _instance;
        public static T Instance => _instance;
        protected virtual void DeserializedInit() { }
        public virtual void Deserialize(BufferReader reader)
        {
            this.DeserializedInit();
            Interlocked.Exchange<T>(ref _instance, this as T);
        }
        public virtual void Serialize(BufferWriter writer)
        {
        }
    }
}
