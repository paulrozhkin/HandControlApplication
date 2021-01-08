using System;
using HandControl.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HandControl.Tests
{
    [TestClass]
    public class UnixTimestampConverterUnitTest
    {
        [TestMethod]
        public void UnixTimestampToDatetimeConverterTest()
        {
            // Arrange
            const long timestampUnix = 1610109596;
            var expectedTime = new DateTime(2021, 01, 08, 15, 39, 56);

            // Act
            var time = UnixTimestampConverter.DateTimeFromUnix(timestampUnix);

            // Assert
            Assert.AreEqual(expectedTime, time);
        }

        [TestMethod]
        public void DatetimeToUnixTimestampConverterTest()
        {
            // Arrange
            var time = new DateTime(2021, 01, 08, 15, 39, 56);
            const long expectedTimestampUnix = 1610109596;

            // Act
            var timestampUnix = UnixTimestampConverter.DateTimeToUnix(time);

            // Assert
            Assert.AreEqual(expectedTimestampUnix, timestampUnix);
        }
    }
}