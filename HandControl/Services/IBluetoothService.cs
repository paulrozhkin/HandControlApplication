using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HandControl.Model;

namespace HandControl.Services
{
    /// <summary>
    ///     Define the sender Bluetooth service interface.
    /// </summary>
    public interface IBluetoothService : IDisposable
    {
        IObservable<byte[]> ReceivedDataObservable { get; }

        /// <summary>
        ///     Connect to bluetooth device
        /// </summary>
        /// <param name="device"></param>
        /// <returns>Result of connect</returns>
        Task<bool> Connect(Device device);

        /// <summary>
        ///     Disconnect device.
        /// </summary>
        void Disconnect();

        /// <summary>
        ///     Gets the devices.
        /// </summary>
        /// <returns>The list of the devices.</returns>
        Task<IList<Device>> GetAuthenticDevices();

        /// <summary>
        ///     Gets all devices.
        /// </summary>
        /// <returns>The list of the devices.</returns>
        Task<IList<Device>> GetAllDevices();

        /// <summary>
        ///     Sends the string UTF8 data to the Receiver.
        /// </summary>
        /// <param name="content">The bytes content.</param>
        /// <returns>If was sent or not.</returns>
        Task Send(byte[] content);
    }
}