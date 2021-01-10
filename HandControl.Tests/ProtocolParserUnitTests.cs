// --------------------------------------------------------------------------------------
// <copyright file = "GestureRepositoryUnitTest.cs" company = "Студенческий проект HandControl‎"> 
//      Copyright © 2020 HandControl. All rights reserved.
// </copyright> 
// -------------------------------------------------------------------------------------

using System;
using HandControl.Model;
using HandControl.Model.BluetoothDto;
using HandControl.Model.Enums;
using HandControl.Services;
using HandControl.Services.IODevice;
using Microsoft.Reactive.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HandControl.Tests
{
    /// <summary>
    ///     Unit test класса <see cref="ProtocolParser" />.
    /// </summary>
    [TestClass]
    public class ProtocolParserUnitTests
    {
        /// <summary>
        ///     Тест корректного приема послеовательности пакетов.
        /// </summary>
        [TestMethod]
        public void ReceiveSequencePackagesTest()
        {
            // Arrange
            byte[] packageFirst =
                {0xfd, 0xba, 0xdc, 0x01, 0x50, 0xb4, 0x11, 0xff, 0x09, 0x05, 0x00, 0xff, 0xff, 0xff, 0xff, 0xff, 0xb6};
            byte[] incorrectSfd = {0xfb, 0xba, 0xdc, 0xfa, 0xad, 0xbd, 0x23, 0xaa, 0xd1, 0x32};
            byte[] packageSecond =
                {0xfd, 0xba, 0xdc, 0x01, 0x50, 0xb4, 0x11, 0xff, 0x05, 0x02, 0x00, 0xab, 0xcd, 0x8a};
            var protocolParser = new ProtocolParser();

            var scheduler = new TestScheduler();
            var observer = scheduler.CreateObserver<PackageDto>();
            protocolParser.ReceivedPackagesObservable.Subscribe(observer);

            // Act
            protocolParser.Update(packageFirst);
            protocolParser.Update(incorrectSfd);
            protocolParser.Update(packageSecond);

            // Assert
            var packages = observer.Messages;
            Assert.AreEqual(2, packages.Count);

            var firstPackageDto = packages[0].Value.Value;
            Assert.AreEqual(CommandType.PerformGestureId, firstPackageDto.Command);
            Assert.AreEqual(5, firstPackageDto.PayloadSize);
            Assert.AreEqual(0xb6, firstPackageDto.ReceivedCrc);
            Assert.AreEqual(0xc6, firstPackageDto.Crc);
            CollectionAssert.AreEqual(new byte[] {0xff, 0xff, 0xff, 0xff, 0xff}, firstPackageDto.Payload);

            var secondPackageDto = packages[1].Value.Value;
            Assert.AreEqual(CommandType.SetSettings, secondPackageDto.Command);
            Assert.AreEqual(2, secondPackageDto.PayloadSize);
            Assert.AreEqual(0x8a, secondPackageDto.ReceivedCrc);
            Assert.AreEqual(0x23, secondPackageDto.Crc);
            CollectionAssert.AreEqual(new byte[] {0xab, 0xcd}, secondPackageDto.Payload);
        }

        [TestMethod]
        public void CreatePackageTest()
        {
            // Arrange
            var command = CommandType.GetGestures;
            var payload = new byte[] {0xba, 0x05};
            var protocolParser = new ProtocolParser();
            byte[] expectedBytes =
                {0xfd, 0xba, 0xdc, 0x01, 0x50, 0xb4, 0x11, 0xff, 0x06, 0x02, 0x00, 0xba, 0x05, 0xb1};

            // Act
            var package = protocolParser.CreatePackage(command, payload);

            // Assert
            CollectionAssert.AreEqual(expectedBytes, package);
        }
    }
}