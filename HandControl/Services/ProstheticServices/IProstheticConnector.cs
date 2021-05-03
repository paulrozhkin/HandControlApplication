using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Documents;
using HandControl.Model.Dto;

namespace HandControl.Services.ProstheticServices
{
    public interface IProstheticConnector
    {
        IObservable<TelemetryDto> TelemetryReceived { get; }

        IObservable<bool> IsConnectionChanged { get; }

        Task ConnectAsync();

        Task SaveGesturesAsync(SaveGestureDto saveGestures);

        Task DeleteGestureAsync(DeleteGestureDto deleteGestureDto);

        Task PerformGestureByIdAsync(PerformGestureByIdDto performGestureByIdDto);

        Task PerformGestureRawAsync(PerformGestureRawDto performGestureRawDto);

        Task SetPositionsAsync(SetPositionsDto setPositionsDto);

        Task<GetGesturesDto> GetGesturesAsync();

        Task<GetSettingsDto> GetSettingsAsync();

        Task SetSettingsAsync(SetSettingsDto setSettingsDto);

        Task UpdateLastTimeSyncAsync(UpdateLastTimeSyncDto updateLastTimeSyncDto);

        Task<TelemetryDto> GetTelemetryAsync();

        Task StartTelemetryAsync(int intervalMs);

        Task StopTelemetryAsync();

        Task<GetMioPatternsDto> GetMioPatternsAsync();

        Task SetMioPatternsAsync(SetMioPatternsDto mioPatterns);
    }
}