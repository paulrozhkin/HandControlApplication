using HandControl.Services;

namespace HandControl.Model.Repositories.GestureRepositories
{
    public class GestureRepositoryFactory
    {
        public IGestureRepository Create()
        {
            return new GestureFileSystemRepository();
            //if (CommunicationManager.GetInstance().СonnectedDevices.StateDeviceHand)
            //{
            //    return null;
            //}
            //else
            //{
            //    return new GestureFileSystemRepository();
            //}
        }
    }
}
