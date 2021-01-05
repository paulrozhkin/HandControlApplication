namespace HandControl.Model.Repositories.GestureRepositories
{
    public class GestureRepositoryFactory
    {
        public IGestureRepository Create()
        {
            return new GestureFileSystemRepository();
            //if (ProstheticManager.GetInstance()._connectedDevices._stateDeviceHand)
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