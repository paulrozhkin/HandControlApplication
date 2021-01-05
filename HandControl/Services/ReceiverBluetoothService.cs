// -------------------------------------------------------------------------------------
// <copyright file = "ReceiverBluetoothService.cs" company = "Студенческий проект HandControl‎"> 
//      Copyright © 2019 HandControl. All rights reserved.
// </copyright> 
// -------------------------------------------------------------------------------------

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using InTheHand.Net.Sockets;

namespace Bluetooth.Services
{
    /// <summary>
    ///     Define the receiver Bluetooth service.
    /// </summary>
    public class ReceiverBluetoothService : ObservableObject, IDisposable, IReceiverBluetoothService
    {
        private readonly Guid _serviceClassId;
        private CancellationTokenSource _cancelSource;
        private BluetoothListener _listener;
        private Action<string> _responseAction;
        private bool _wasStarted;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ReceiverBluetoothService" /> class.
        /// </summary>
        public ReceiverBluetoothService()
        {
            _serviceClassId = new Guid("0e6114d0-8a2e-477a-8502-298d1ff4b4ba");
        }

        /// <summary>
        ///     The dispose.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Gets or sets a value indicating whether was started.
        /// </summary>
        /// <value>
        ///     The was started.
        /// </value>
        public bool WasStarted
        {
            get => _wasStarted;
            set { Set(() => WasStarted, ref _wasStarted, value); }
        }

        /// <summary>
        ///     Starts the listening from Senders.
        /// </summary>
        /// <param name="reportAction">
        ///     The report Action.
        /// </param>
        public void Start(Action<string> reportAction)
        {
            WasStarted = true;
            _responseAction = reportAction;
            if (_cancelSource != null && _listener != null) Dispose(true);

            _listener = new BluetoothListener(_serviceClassId)
            {
                ServiceName = "MyService"
            };

            _listener.Start();

            _cancelSource = new CancellationTokenSource();

            Task.Run(() => Listener(_cancelSource));
        }

        /// <summary>
        ///     Stops the listening from Senders.
        /// </summary>
        public void Stop()
        {
            WasStarted = false;
            _cancelSource.Cancel();
        }

        /// <summary>
        ///     Listeners the accept bluetooth client.
        /// </summary>
        /// <param name="token">
        ///     The token.
        /// </param>
        private void Listener(CancellationTokenSource token)
        {
            try
            {
                while (true)
                    using (var client = _listener.AcceptBluetoothClient())
                    {
                        if (token.IsCancellationRequested) return;

                        using (var streamReader = new StreamReader(client.GetStream()))
                        {
                            try
                            {
                                var content = streamReader.ReadToEnd();
                                if (!string.IsNullOrEmpty(content)) _responseAction(content);
                            }
                            catch (IOException)
                            {
                                client.Close();
                                break;
                            }
                        }
                    }
            }
            catch (Exception)
            {
                // todo handle the exception
            }
        }

        /// <summary>
        ///     The dispose.
        /// </summary>
        /// <param name="disposing">
        ///     The disposing.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                if (_cancelSource != null)
                {
                    _listener.Stop();
                    _listener = null;
                    _cancelSource.Dispose();
                    _cancelSource = null;
                }
        }
    }
}