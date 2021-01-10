using System;
using HandControl.Model.Dto;

namespace HandControl.Services.LocalStorage
{
    public interface IGesturesLocalStorage
    {
        DateTime LastTimeSync { get; }
        void Add(SaveGestureDto saveGestureDto);
        void Remove(DeleteGestureDto deleteGestureDto);
        void UpdateLastTimeSync(UpdateLastTimeSyncDto updateLastTimeSyncDto);
        GetGesturesDto GetGestures();
    }
}