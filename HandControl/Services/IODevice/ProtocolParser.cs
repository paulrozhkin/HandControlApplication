using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using HandControl.Model.BluetoothDto;
using HandControl.Model.Enums;

namespace HandControl.Services.IODevice
{
    public class ProtocolParser : IProtocolParser
    {
        private SemaphoreSlim _receiveSemaphoreSlim = new SemaphoreSlim(1);

        public ProtocolParser()
        {
            _state = ProtocolState.Sfd;
            _lastReceiveTime = DateTime.MinValue;
            _receivedPackagesSubject = new Subject<PackageDto>();
            ReceivedPackagesObservable = _receivedPackagesSubject.AsObservable();
        }

        #region Properties

        public IObservable<PackageDto> ReceivedPackagesObservable { get; }

        #endregion

        #region Classes

        private enum ProtocolState
        {
            Sfd,
            Type,
            Size,
            Payload,
            Crc8
        }

        #endregion

        #region Fields

        private const int Timeout = 5;
        private readonly byte[] _sfd = {0xfd, 0xba, 0xdc, 0x01, 0x50, 0xb4, 0x11, 0xff};
        private ProtocolState _state;
        private DateTime _lastReceiveTime;
        private readonly List<byte> _receiveBuffer = new List<byte>();
        private PackageDto _currentPackage;
        private byte _crc;
        private readonly Subject<PackageDto> _receivedPackagesSubject;

        #endregion

        #region Methods

        public async void Update(byte[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            await _receiveSemaphoreSlim.WaitAsync();
            
            foreach (var dataByte in data)
            {
                Update(dataByte);
            }

            _receiveSemaphoreSlim.Release();
        }

        public void Update(byte data)
        {
            var receiveTime = DateTime.Now;

            // Если вышел таймаут, то сбрасываем пакет
            if (_state != ProtocolState.Sfd
                && (receiveTime - _lastReceiveTime).TotalSeconds > Timeout)
            {
                _receiveBuffer.Clear();
                _state = ProtocolState.Sfd;
            }

            _receiveBuffer.Add(data);

            // Если идет прием полезной нагрузки, то расчитываем CRC8
            if (_state != ProtocolState.Sfd && _state != ProtocolState.Crc8)
                _crc = Crc8Calculator.ComputeChecksum(data, _crc);

            switch (_state)
            {
                case ProtocolState.Sfd:
                {
                    if (_receiveBuffer.Count == 8)
                    {
                        if (_receiveBuffer.SequenceEqual(_sfd))
                        {
                            _currentPackage = new PackageDto();
                            _crc = 0;
                            _state = ProtocolState.Type;
                        }
                        else
                        {
                            _receiveBuffer.RemoveAt(0);
                        }
                    }

                    break;
                }
                case ProtocolState.Type:
                {
                    _currentPackage.Command = (CommandType) data;
                    _state = ProtocolState.Size;
                    break;
                }
                case ProtocolState.Size:
                {
                    if (_receiveBuffer.Count == 11)
                    {
                        _currentPackage.PayloadSize = (_receiveBuffer[10] << 8) | _receiveBuffer[9];

                        if (_currentPackage.PayloadSize == 0)
                            _state = ProtocolState.Crc8;
                        else
                            _state = ProtocolState.Payload;
                    }

                    break;
                }
                case ProtocolState.Payload:
                {
                    if (_currentPackage.PayloadSize + 11 == _receiveBuffer.Count)
                    {
                        _currentPackage.Payload = _receiveBuffer.GetRange(11, _currentPackage.PayloadSize).ToArray();
                        _state = ProtocolState.Crc8;
                    }

                    break;
                }
                case ProtocolState.Crc8:
                {
                    _currentPackage.ReceivedCrc = data;
                    _currentPackage.Crc = _crc;
                    _receiveBuffer.Clear();
                    _state = ProtocolState.Sfd;
                    _receivedPackagesSubject.OnNext(_currentPackage);
                    break;
                }
                default:
                {
                    throw new ArgumentOutOfRangeException();
                }
            }

            _lastReceiveTime = receiveTime;
        }

        public byte[] CreatePackage(CommandType command, byte[] payload)
        {
            if (payload?.Length > ushort.MaxValue)
                throw new ArgumentOutOfRangeException($"The payload can be no more than {ushort.MaxValue} bytes");

            var packageLength = payload?.Length ?? 0;

            var package = new byte[12 + packageLength];
            _sfd.CopyTo(package, 0);
            package[8] = (byte) command;

            if (payload != null)
            {
                package[9] = (byte) (payload.Length & 0xff);
                package[10] = (byte) ((payload.Length >> 8) & 0xff);
                payload.CopyTo(package, 11);
            }

            var crc = Crc8Calculator.ComputeChecksum(package[8]);
            crc = Crc8Calculator.ComputeChecksum(package[9], crc);
            crc = Crc8Calculator.ComputeChecksum(package[10], crc);

            if (payload != null)
            {
                crc = Crc8Calculator.ComputeChecksum(payload, crc);
            }

            package[package.Length - 1] = crc;

            return package;
        }

        #endregion
    }
}