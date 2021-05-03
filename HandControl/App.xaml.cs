// --------------------------------------------------------------------------------------
// <copyright file = "App.xaml.cs" company = "Студенческий проект HandControl‎"> 
//      Copyright © 2019 HandControl. All rights reserved.
// </copyright> 
// -------------------------------------------------------------------------------------

using System.Windows;
using HandControl.Services;
using HandControl.Services.IODevice;
using HandControl.Services.IODevice.Bluetooth;
using HandControl.Services.LocalStorage;
using HandControl.Services.Mappers;
using HandControl.Services.ProstheticServices;
using HandControl.View;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Unity;

namespace HandControl
{
    /// <summary>
    ///     Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            Container.Resolve<IProstheticManager>().ConnectAsync();
            return Container.Resolve<MainWindow>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            var mapper = new MapperFabric().CreateMapper();
            containerRegistry.RegisterInstance(mapper);

            containerRegistry.RegisterSingleton<IFileSystemFacade, FileSystemFacade>();
            containerRegistry.RegisterSingleton<IGesturesLocalStorage, GesturesLocalStorage>();
            containerRegistry.RegisterSingleton<IGestureService, GestureService>();
            containerRegistry.RegisterSingleton<IIoDevice, DeviceBluetooth>();
            containerRegistry.RegisterSingleton<IProstheticConnector, ProstheticConnector>();
            containerRegistry.RegisterSingleton<IProstheticManager, ProstheticManager>();
            containerRegistry.RegisterSingleton<IMioPatternsService, MioPatternsService>();
        }
    }
}