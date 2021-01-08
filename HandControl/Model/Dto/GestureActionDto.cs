namespace HandControl.Model.Dto
{
    public class GestureActionDto: IBinarySerialize
    {
        public int PointerFingerPosition { get; set; }
        public int MiddleFingerPosition { get; set; }
        public int RingFingerPosition { get; set; }
        public int LittleFingerPosition { get; set; }
        public int ThumbFingerPosition { get; set; }
        public int Delay { get; set; }

        public byte[] BinarySerialize()
        {
            throw new System.NotImplementedException();
        }

        public void BinaryDeserialize(byte[] data)
        {
            throw new System.NotImplementedException();
        }
    }
}
