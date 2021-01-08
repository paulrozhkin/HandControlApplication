using System;
using HandControl.Model.Protobuf;

namespace HandControl.Model.Dto
{
    public class DeleteGestureDto : IBinarySerialize
    {
        public DateTime TimeSync { get; set; }
        public UUID Id { get; set; }
        public byte[] BinarySerialize()
        {
            throw new NotImplementedException();
        }

        public void BinaryDeserialize(byte[] data)
        {
            throw new NotImplementedException();
        }
    }
}
