// --------------------------------------------------------------------------------------
// <copyright file = "GestureModelUnitTest.cs" company = "Студенческий проект HandControl‎"> 
//      Copyright © 2019 HandControl. All rights reserved.
// </copyright> 
// -------------------------------------------------------------------------------------

using System;
using HandControl.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HandControl.Tests
{
    /// <summary>
    ///     Unit test класса <see cref="GestureModel" />.
    /// </summary>
    [TestClass]
    public class GestureModelUnitTest
    {
        /// <summary>
        ///     Тест сравнения экземпляров <see cref="GestureModel" />.
        /// </summary>
        [TestMethod]
        public void EqualsGestureTest()
        {
            // Arrange
            var gesture = GestureModel.GetDefault(Guid.NewGuid(), "Сжать");
            var gestureClone = gesture.Clone() as GestureModel;
            var gesture2 = GestureModel.GetDefault(Guid.NewGuid(), "Разжать");

            // Act

            // Assert
            EqualityTests.TestUnequalObjects(gesture, gesture2);
            EqualityTests.TestEqualObjects(gesture, gestureClone);
            EqualityTests.TestAgainstNull(gesture);
        }
    }
}