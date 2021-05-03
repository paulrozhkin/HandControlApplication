using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Google.Protobuf;
using HandControl.Model.Dto;
using HandControl.Model.Enums;
using HandControl.Model.Protobuf;
using HandControl.Services.IODevice;

namespace HandControl.Services.ProstheticServices
{
    public class ProstheticConnector : IProstheticConnector
    {
        private readonly IIoDevice _prostheticDevice;
        private readonly IMapper _mapper;

        public ProstheticConnector(IIoDevice prostheticDevice,
            IMapper mapper)
        {
            _prostheticDevice = prostheticDevice ?? throw new ArgumentNullException(nameof(prostheticDevice));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            TelemetryReceived = _prostheticDevice.TelemetryPackages.Select(package =>
            {
                var protobufModel = Telemetry.Parser.ParseFrom(package.Payload);
                return _mapper.Map<Telemetry, TelemetryDto>(protobufModel);
            });
        }

        public IObservable<TelemetryDto> TelemetryReceived { get; }

        public IObservable<bool> IsConnectionChanged => _prostheticDevice.IsConnectedStatusChanged;

        public Task ConnectAsync()
        {
            return _prostheticDevice.ConnectDeviceAsync();
        }

        public Task SaveGesturesAsync(SaveGestureDto saveGestures)
        {
            var protobufModel = _mapper.Map<SaveGestureDto, SaveGesture>(saveGestures);
            return _prostheticDevice.SendToDeviceAsync(CommandType.SaveGestures, protobufModel.ToByteArray());
        }

        public Task DeleteGestureAsync(DeleteGestureDto deleteGestureDto)
        {
            var protobufModel = _mapper.Map<DeleteGestureDto, DeleteGesture>(deleteGestureDto);
            return _prostheticDevice.SendToDeviceAsync(CommandType.DeleteGestures, protobufModel.ToByteArray());
        }

        public Task PerformGestureByIdAsync(PerformGestureByIdDto performGestureByIdDto)
        {
            var protobufModel = _mapper.Map<PerformGestureByIdDto, PerformGestureById>(performGestureByIdDto);
            return _prostheticDevice.SendToDeviceAsync(CommandType.PerformGestureId, protobufModel.ToByteArray());
        }

        public Task PerformGestureRawAsync(PerformGestureRawDto performGestureRawDto)
        {
            var protobufModel = _mapper.Map<PerformGestureRawDto, PerformGestureRaw>(performGestureRawDto);
            return _prostheticDevice.SendToDeviceAsync(CommandType.PerformGestureRaw, protobufModel.ToByteArray());
        }

        public Task SetPositionsAsync(SetPositionsDto setPositionsDto)
        {
            var protobufModel = _mapper.Map<SetPositionsDto, SetPositions>(setPositionsDto);
            return _prostheticDevice.SendToDeviceAsync(CommandType.SetPositions, protobufModel.ToByteArray());
        }

        public async Task<GetGesturesDto> GetGesturesAsync()
        {
            var response = await _prostheticDevice.SendToDeviceAsync(CommandType.GetGestures);
            var protobufModel = GetGestures.Parser.ParseFrom(response);
            return _mapper.Map<GetGestures, GetGesturesDto>(protobufModel);
        }

        public async Task<GetSettingsDto> GetSettingsAsync()
        {
            var response = await _prostheticDevice.SendToDeviceAsync(CommandType.GetSettings);
            var protobufModel = GetSettings.Parser.ParseFrom(response);
            return _mapper.Map<GetSettings, GetSettingsDto>(protobufModel);
        }

        public Task SetSettingsAsync(SetSettingsDto setSettingsDto)
        {
            var protobufModel = _mapper.Map<SetSettingsDto, SetSettings>(setSettingsDto);
            return _prostheticDevice.SendToDeviceAsync(CommandType.SetSettings, protobufModel.ToByteArray());
        }

        public Task UpdateLastTimeSyncAsync(UpdateLastTimeSyncDto updateLastTimeSyncDto)
        {
            var protobufModel = _mapper.Map<UpdateLastTimeSyncDto, UpdateLastTimeSync>(updateLastTimeSyncDto);
            return _prostheticDevice.SendToDeviceAsync(CommandType.UpdateLastTimeSync, protobufModel.ToByteArray());
        }

        public async Task<TelemetryDto> GetTelemetryAsync()
        {
            var response = await _prostheticDevice.SendToDeviceAsync(CommandType.GetTelemetry);
            var protobufModel = GetTelemetry.Parser.ParseFrom(response);
            return _mapper.Map<Telemetry, TelemetryDto>(protobufModel.Telemetry);
        }

        public Task StartTelemetryAsync(int intervalMs)
        {
            var telemetry = new StartTelemetryDto()
            {
                IntervalMs = intervalMs
            };

            var protobufModel = _mapper.Map<StartTelemetryDto, StartTelemetry>(telemetry);
            return _prostheticDevice.SendToDeviceAsync(CommandType.StartTelemetry, protobufModel.ToByteArray());
        }

        public Task StopTelemetryAsync()
        {
            return _prostheticDevice.SendToDeviceAsync(CommandType.StopTelemetry, null);
        }
    }
}