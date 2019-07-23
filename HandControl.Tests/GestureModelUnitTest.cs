// --------------------------------------------------------------------------------------
// <copyright file = "GestureModelUnitTest.cs" company = "Студенческий проект HandControl‎"> 
//      Copyright © 2019 HandControl. All rights reserved.
// </copyright> 
// -------------------------------------------------------------------------------------
namespace HandControl.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using HandControl.Model;

    /// <summary>
    /// Unit test класса <see cref="GestureModel"/>.
    /// </summary>
    [TestClass]
    public class GestureModelUnitTest
    {
        /// <summary>
        /// Тест сериализации и десериализации экземляра <see cref="GestureModel"/>.
        /// </summary>
        [TestMethod]
        public void BinaryForamtterTest_SerializerAndDesirializeGestureModel()
        {
            // Arrange
            GestureModel gesture = GestureModel.GetDefault(Guid.NewGuid(), "Какое то имя");
            gesture.InfoGesture.NumberOfMotions = 3;
            gesture.ListMotions.Add(GestureModel.MotionModel.GetDefault(0));
            gesture.ListMotions.Add(GestureModel.MotionModel.GetDefault(1));
            gesture.ListMotions.Add(GestureModel.MotionModel.GetDefault(2));

            gesture.ListMotions[0].LittleFinger = 1;
            gesture.ListMotions[0].MiddleFinger = 2;
            gesture.ListMotions[0].PointerFinger = 3;
            gesture.ListMotions[0].RingFinder = 4;
            gesture.ListMotions[0].StatePosBrush = 5;
            gesture.ListMotions[0].ThumbFinger = 6;
            gesture.ListMotions[0].DelMotion = 7;

            // Act
            byte[] dataSerialize = gesture.BinarySerialize();
            GestureModel gestureDes = GestureModel.GetDefault(new Guid(), string.Empty);
            gestureDes.BinaryDesserialize(dataSerialize);

            // Assert
            Assert.AreEqual(gesture, gestureDes);
        }

        /// <summary>
        /// Тест сравнения экземпляров <see cref="GestureModel"/>.
        /// </summary>
        [TestMethod]
        public void EqualsGestureTest()
        {
            // Arrange
            GestureModel gesture = GestureModel.GetDefault(Guid.NewGuid(), "Сжать");
            GestureModel gestureClone = gesture.Clone() as GestureModel;
            GestureModel gesture2 = GestureModel.GetDefault(Guid.NewGuid(), "Разжать");

            // Act

            // Assert
            EqualityTests.TestUnequalObjects(gesture, gesture2);
            EqualityTests.TestEqualObjects(gesture, gestureClone);
            EqualityTests.TestAgainstNull(gesture);
        }
    }
}
