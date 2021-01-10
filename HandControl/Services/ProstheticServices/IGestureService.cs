using System;
using System.Threading.Tasks;
using HandControl.Model;

namespace HandControl.Services.ProstheticServices
{
    public interface IGestureService
    {
        DateTime LastTimeSync { get; }
        IObservable<GestureModel> Gestures { get; }
        Task AddGestureAsync(GestureModel gesture);
        Task RemoveGestureAsync(GestureModel gesture);
        Task SyncGesturesAsync();
    }
}
