using System;
using System.Linq;
using HandControl.Model.Dto;
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
            Assert.AreEqual(firstActionInExpectedGesture1.LittleFingerPosition, firstActionInResultGesture1.LittleFingerPosition);
            Assert.AreEqual(firstActionInExpectedGesture1.RingFingerPosition, firstActionInResultGesture1.RingFingerPosition);
            Assert.AreEqual(firstActionInExpectedGesture1.MiddleFingerPosition, firstActionInResultGesture1.MiddleFingerPosition);
            Assert.AreEqual(firstActionInExpectedGesture1.PointerFingerPosition, firstActionInResultGesture1.PointerFingerPosition);
            Assert.AreEqual(firstActionInExpectedGesture1.ThumbFingerPosition, firstActionInResultGesture1.ThumbFingerPosition);
            Assert.AreEqual(firstActionInExpectedGesture1.Delay, firstActionInResultGesture1.Delay);

            var lastActionInResultGesture1 = resultGesture1.Actions.Last();
            var lastActionInExpectedGesture1 = expectedGesture1.Actions.Last();
            Assert.AreEqual(lastActionInExpectedGesture1.LittleFingerPosition, lastActionInResultGesture1.LittleFingerPosition);
            Assert.AreEqual(lastActionInExpectedGesture1.RingFingerPosition, lastActionInResultGesture1.RingFingerPosition);
            Assert.AreEqual(lastActionInExpectedGesture1.MiddleFingerPosition, lastActionInResultGesture1.MiddleFingerPosition);
            Assert.AreEqual(lastActionInExpectedGesture1.PointerFingerPosition, lastActionInResultGesture1.PointerFingerPosition);
            Assert.AreEqual(lastActionInExpectedGesture1.ThumbFingerPosition, lastActionInResultGesture1.ThumbFingerPosition);
            Assert.AreEqual(lastActionInExpectedGesture1.Delay, lastActionInResultGesture1.Delay);
        }
    }
}