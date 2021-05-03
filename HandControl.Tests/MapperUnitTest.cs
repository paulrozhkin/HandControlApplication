using System;
using System.Linq;
using AutoFixture;
using HandControl.Model;
using HandControl.Model.Dto;
using HandControl.Model.Enums;
using HandControl.Model.Protobuf;
using HandControl.Services.Mappers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HandControl.Tests
{
    [TestClass]
    public class MapperUnitTest
    {
        [TestMethod]
        public void TestMapper()
        {
            // Arrange
            var mapper = new MapperFabric().CreateMapper();

            // Act && Assert
            mapper.ConfigurationProvider.AssertConfigurationIsValid();
        }

        [TestMethod]
        public void TelemetryMapTest()
        {
            // Arrange
            var mapper = new MapperFabric().CreateMapper();

            var id = Guid.NewGuid();
            var expectedLastTimeSync = new DateTime(2021, 01, 08, 15, 39, 56);

            var telemetry = new Telemetry()
            {
                EmgStatus = ModuleStatusType.ModuleStatusWork,
                DisplayStatus = ModuleStatusType.ModuleStatusConnectionError,
                GyroStatus = ModuleStatusType.ModuleStatusDisabled,
                DriverStatus = DriverStatusType.DriverStatusInitialization,
                LastTimeSync = 1610109596,
                Emg = 3000,
                ExecutableGesture = new UUID()
                {
                    Value = id.ToString()
                },
                Power = 100,
                PointerFingerPosition = 1,
                MiddleFingerPosition = 2,
                RingFingerPosition = 3,
                LittleFingerPosition = 4,
                ThumbFingerPosition = 5
            };

            // Act
            var telemetryDto = mapper.Map<Telemetry, TelemetryDto>(telemetry);

            // Assert
            Assert.AreEqual(ModuleStatus.Work, telemetryDto.EmgStatus);
            Assert.AreEqual(ModuleStatus.ConnectionError, telemetryDto.DisplayStatus);
            Assert.AreEqual(ModuleStatus.Disabled, telemetryDto.GyroStatus);
            Assert.AreEqual(DriverStatus.Initialization, telemetryDto.DriverStatus);
            Assert.AreEqual(expectedLastTimeSync, telemetryDto.LastTimeSync);
            Assert.AreEqual(3000, telemetryDto.Emg);
            Assert.AreEqual(id, telemetryDto.ExecutableGesture);
            Assert.AreEqual(100, telemetryDto.Power);
            Assert.AreEqual(1, telemetryDto.PointerFingerPosition);
            Assert.AreEqual(2, telemetryDto.MiddleFingerPosition);
            Assert.AreEqual(3, telemetryDto.RingFingerPosition);
            Assert.AreEqual(4, telemetryDto.LittleFingerPosition);
            Assert.AreEqual(5, telemetryDto.ThumbFingerPosition);
        }

        [TestMethod]
        public void SetSettingMapTest()
        {
            // Arrange
            var mapper = new MapperFabric().CreateMapper();

            var settingsDto = new SetSettingsDto()
            {
                EnableDisplay = false,
                EnableDriver = true,
                EnableEmg = false,
                EnableGyro = true,
                PowerOff = true
            };

            // Act
            var settings = mapper.Map<SetSettingsDto, SetSettings>(settingsDto);

            // Assert
            Assert.AreEqual(false, settings.EnableDisplay);
            Assert.AreEqual(true, settings.EnableDriver);
            Assert.AreEqual(false, settings.EnableEmg);
            Assert.AreEqual(true, settings.EnableGyro);
            Assert.AreEqual(true, settings.PowerOff);
        }

        [TestMethod]
        public void GetSettingMapTest()
        {
            // Arrange
            var mapper = new MapperFabric().CreateMapper();

            var settings = new GetSettings()
            {
                EnableDisplay = false,
                EnableDriver = true,
                EnableEmg = false,
                EnableGyro = true,
            };

            // Act
            var settingsDto = mapper.Map<GetSettings, GetSettingsDto>(settings);

            // Assert
            Assert.AreEqual(false, settingsDto.EnableDisplay);
            Assert.AreEqual(true, settingsDto.EnableDriver);
            Assert.AreEqual(false, settingsDto.EnableEmg);
            Assert.AreEqual(true, settingsDto.EnableGyro);
        }

        [TestMethod]
        public void GetGesturesMapTest()
        {
            // Arrange
            var mapper = new MapperFabric().CreateMapper();

            var expectedLastTimeSync = new DateTime(2021, 01, 08, 15, 39, 56);

            var name = Guid.NewGuid().ToString();
            var id = Guid.NewGuid();

            var getGestureDto = new GetGestures()
            {
                LastTimeSync = 1610109596,
                Gestures =
                {
                    new Gesture()
                    {
                        Id = new UUID()
                        {
                            Value = id.ToString()
                        },
                        Iterable = false,
                        Repetitions = 1,
                        LastTimeSync = 1610109596,
                        Name = name,
                        Actions =
                        {
                            new GestureAction()
                            {
                                LittleFingerPosition = 1,
                                MiddleFingerPosition = 2,
                                PointerFingerPosition = 3,
                                RingFingerPosition = 4,
                                ThumbFingerPosition = 5,
                                Delay = 6
                            },
                            new GestureAction()
                            {
                                LittleFingerPosition = 10,
                                MiddleFingerPosition = 11,
                                PointerFingerPosition = 12,
                                RingFingerPosition = 13,
                                ThumbFingerPosition = 14,
                                Delay = 15
                            }
                        }
                    }
                }
            };

            // Act
            var result = mapper.Map<GetGestures, GetGesturesDto>(getGestureDto);

            // Assert
            Assert.AreEqual(expectedLastTimeSync, result.LastTimeSync);
            Assert.AreEqual(getGestureDto.Gestures.Count, result.Gestures.Count());

            var resultGesture1 = result.Gestures.First();
            var expectedGesture1 = getGestureDto.Gestures.First();
            Assert.AreEqual(name, resultGesture1.Name);
            Assert.AreEqual(id, resultGesture1.Id);
            Assert.AreEqual(expectedLastTimeSync, resultGesture1.LastTimeSync);
            Assert.AreEqual(expectedGesture1.Iterable, resultGesture1.Iterable);
            Assert.AreEqual(expectedGesture1.Repetitions, resultGesture1.Repetitions);

            var firstActionInResultGesture1 = resultGesture1.Actions.First();
            var firstActionInExpectedGesture1 = expectedGesture1.Actions.First();
            Assert.AreEqual(firstActionInExpectedGesture1.LittleFingerPosition,
                firstActionInResultGesture1.LittleFingerPosition);
            Assert.AreEqual(firstActionInExpectedGesture1.RingFingerPosition,
                firstActionInResultGesture1.RingFingerPosition);
            Assert.AreEqual(firstActionInExpectedGesture1.MiddleFingerPosition,
                firstActionInResultGesture1.MiddleFingerPosition);
            Assert.AreEqual(firstActionInExpectedGesture1.PointerFingerPosition,
                firstActionInResultGesture1.PointerFingerPosition);
            Assert.AreEqual(firstActionInExpectedGesture1.ThumbFingerPosition,
                firstActionInResultGesture1.ThumbFingerPosition);
            Assert.AreEqual(firstActionInExpectedGesture1.Delay, firstActionInResultGesture1.Delay);

            var lastActionInResultGesture1 = resultGesture1.Actions.Last();
            var lastActionInExpectedGesture1 = expectedGesture1.Actions.Last();
            Assert.AreEqual(lastActionInExpectedGesture1.LittleFingerPosition,
                lastActionInResultGesture1.LittleFingerPosition);
            Assert.AreEqual(lastActionInExpectedGesture1.RingFingerPosition,
                lastActionInResultGesture1.RingFingerPosition);
            Assert.AreEqual(lastActionInExpectedGesture1.MiddleFingerPosition,
                lastActionInResultGesture1.MiddleFingerPosition);
            Assert.AreEqual(lastActionInExpectedGesture1.PointerFingerPosition,
                lastActionInResultGesture1.PointerFingerPosition);
            Assert.AreEqual(lastActionInExpectedGesture1.ThumbFingerPosition,
                lastActionInResultGesture1.ThumbFingerPosition);
            Assert.AreEqual(lastActionInExpectedGesture1.Delay, lastActionInResultGesture1.Delay);
        }

        [TestMethod]
        public void GestureModelToDtoMapTest()
        {
            // Arrange
            var mapper = new MapperFabric().CreateMapper();
            var fixture = new Fixture();
            var gestureModel = fixture.Create<GestureModel>();

            // Act
            var gestureDto = mapper.Map<GestureModel, GestureDto>(gestureModel);

            // Assert
            Assert.AreEqual(gestureModel.Id, gestureDto.Id);
            Assert.AreEqual(gestureModel.InfoGesture.TimeChange, gestureDto.LastTimeSync);
            Assert.AreEqual(gestureModel.InfoGesture.IterableGesture, gestureDto.Iterable);
            Assert.AreEqual(gestureModel.InfoGesture.NumberOfGestureRepetitions, gestureDto.Repetitions);
            Assert.AreEqual(gestureModel.ListMotions.Count, gestureDto.Actions.Count());

            var actionsDto = gestureDto.Actions.ToList();
            for (var i = 0; i < gestureModel.ListMotions.Count; i++)
            {
                var actionDtoModel = actionsDto[i];
                var actionModel = gestureModel.ListMotions[i];

                Assert.AreEqual(actionModel.LittleFinger, actionDtoModel.LittleFingerPosition);
                Assert.AreEqual(actionModel.RingFinder, actionDtoModel.RingFingerPosition);
                Assert.AreEqual(actionModel.MiddleFinger, actionDtoModel.MiddleFingerPosition);
                Assert.AreEqual(actionModel.PointerFinger, actionDtoModel.PointerFingerPosition);
                Assert.AreEqual(actionModel.ThumbFinger, actionDtoModel.ThumbFingerPosition);
                Assert.AreEqual(actionModel.DelMotion, actionDtoModel.Delay);
            }
        }

        [TestMethod]
        public void StartTelemetryMapTest()
        {
            // Arrange
            var mapper = new MapperFabric().CreateMapper();
            var fixture = new Fixture();
            var startTelemetryDto = fixture.Create<StartTelemetryDto>();

            // Act
            var startTelemetry = mapper.Map<StartTelemetryDto, StartTelemetry>(startTelemetryDto);

            // Assert
            Assert.AreEqual(startTelemetryDto.IntervalMs, startTelemetry.IntervalMs);
        }

        [TestMethod]
        public void GetTelemetryMapTest()
        {
            // Arrange
            var mapper = new MapperFabric().CreateMapper();
            var fixture = new Fixture();
            var getTelemetry = fixture.Create<GetTelemetry>();

            // Act
            var getTelemetryDto = mapper.Map<GetTelemetry, GetTelemetryDto>(getTelemetry);

            // Assert
            Assert.IsNotNull(getTelemetryDto.Telemetry);
        }
    }
}