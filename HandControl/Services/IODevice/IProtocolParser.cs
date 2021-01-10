using System;
using HandControl.Model.BluetoothDto;
using HandControl.Model.Enums;

namespace HandControl.Services.IODevice
{
    public interface IProtocolParser
    {
        IObservable<PackageDto> ReceivedPackagesObservable { get; }

        void Update(byte[] data);

        void Update(byte data);

        byte[] CreatePackage(CommandType command, byte[] payload);
    }
}