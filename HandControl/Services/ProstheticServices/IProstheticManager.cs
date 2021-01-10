using System;
using System.Threading.Tasks;
using HandControl.Model.Dto;

namespace HandControl.Services.ProstheticServices
{
    public interface IProstheticManager
    { 
        IObservable<bool> IsConnectionChanged { get; }
        IObservable<TelemetryDto> TelemetryReceived { get; }
        IGestureService GestureService { get; }
        Task ConnectAsync();
    }
}