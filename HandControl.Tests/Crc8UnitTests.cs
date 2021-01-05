// --------------------------------------------------------------------------------------
// <copyright file = "GestureRepositoryUnitTest.cs" company = "Студенческий проект HandControl‎"> 
//      Copyright © 2020 HandControl. All rights reserved.
// </copyright> 
// -------------------------------------------------------------------------------------

using HandControl.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HandControl.Tests
{
    /// <summary>
    ///     Unit test класса <see cref="Crc8Calculator" />.
    /// </summary>
    [TestClass]
    public class Crc8UnitTests
    {
        /// <summary>
        ///     Тест на расчет CRC8
        /// </summary>
        [TestMethod]
        public void Crc8CalculationCorrect()
        {
            // Arrange
            byte[] data = {0x04, 0x03, 0xFB, 0xAD, 0xFF, 0xBD};
            byte expectedCrc8 = 0xBF;

            // Act
            var resultCrc = Crc8Calculator.ComputeChecksum(data);

            // Assert
            Assert.AreEqual(expectedCrc8, resultCrc);
        }
    }
}
