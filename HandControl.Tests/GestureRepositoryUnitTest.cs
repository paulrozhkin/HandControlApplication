﻿// --------------------------------------------------------------------------------------
// <copyright file = "GestureRepositoryUnitTest.cs" company = "Студенческий проект HandControl‎"> 
//      Copyright © 2019 HandControl. All rights reserved.
// </copyright> 
// -------------------------------------------------------------------------------------
namespace HandControl.Tests
{
    using System;
    using System.Collections.Generic;
    using HandControl.Model;
    using HandControl.Model.Repositories;
    using HandControl.Model.Repositories.Specifications;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    
    /// <summary>
    /// Unit test класса <see cref="GestureRepository"/>.
    /// Для тестирорования требуется отсутсвие данных на физическом носителе.
    /// Выполняется проверка только хранение в памяти, а не в файловой системе.
    /// </summary>
    [TestClass]
    public class GestureRepositoryUnitTest
    {
        /// <summary>
        /// Тест добавления двух сущностей жеста в репозиторий.
        /// После добавления в репозитории должно содержаться две сущности.
        /// </summary>
        [TestMethod]
        public void AddTestSimple_TwoEnityAddToRepo()
        {
            // Arrange
            GestureRepository gestures = new GestureRepository();
            GestureModel gesture1 = GestureModel.GetDefault(Guid.NewGuid(), "Жест 1");
            GestureModel gesture2 = GestureModel.GetDefault(Guid.NewGuid(), "Жест 2");

            // Act
            gestures.Add(gesture1);
            gestures.Add(gesture2);
            IEntitySpecification<GestureModel> specGetAll = new GesturesSpecificationByAll();
            List<GestureModel> gesturesInRepo = new List<GestureModel>(gestures.Query(specGetAll));

            // Assert
            Assert.IsTrue(gesturesInRepo.Count.Equals(2));
        }

        /// <summary>
        /// Тест обновления сущности в репозитории.
        /// После обновления извлекаемая сущность должна соотвествовать первоначальной.
        /// </summary>
        [TestMethod]
        public void AddTestUpdate_EnityUpdateByIdInRepo()
        {
            // Arrange
            string nameBase = "Name";
            string nameNew = "NewName";
            GestureRepository gestures = new GestureRepository();
            GestureModel gesture1 = GestureModel.GetDefault(Guid.NewGuid(), nameBase);
            GestureModel gesture2 = gesture1.Clone() as GestureModel;
            gesture2.Name = nameNew;

            // Act
            gestures.Add(gesture1);
            gestures.Add(gesture2);
            IEntitySpecification<GestureModel> specGetByName = new GesturesSpecificationByName(nameNew);
            List<GestureModel> gesturesInRepo = new List<GestureModel>(gestures.Query(specGetByName));

            // Assert
            // TODO: Вернуть.
            Assert.IsTrue(gesture2.Equals(gesturesInRepo?[0]));
        }

        /// <summary>
        /// Тест добавления двух сущностей жеста с одинаковыми именами в репозиторий.
        /// После добавления в репозитории должна только первая сущность и должно возникнуть исключение второго добавления.
        /// </summary>
        [TestMethod]
        public void AddTestError_TwoEnityWithTheSameNameAddToRepo()
        {
            // Arrange
            GestureRepository gestures = new GestureRepository();
            GestureModel gesture1 = GestureModel.GetDefault(Guid.NewGuid(), "Жест 1");
            GestureModel gesture2 = GestureModel.GetDefault(Guid.NewGuid(), "Жест 1");
            bool isThrowArgumentException = false;

            // Act
            gestures.Add(gesture1);
            try
            {
                gestures.Add(gesture2);
            }
            catch (ArgumentException)
            {
                isThrowArgumentException = true;
            }

            IEntitySpecification<GestureModel> specGetAll = new GesturesSpecificationByAll();
            List<GestureModel> gesturesInRepo = new List<GestureModel>(gestures.Query(specGetAll));

            // Assert
            Assert.IsTrue(gesturesInRepo.Count.Equals(1));
            Assert.IsTrue(isThrowArgumentException.Equals(true));
        }

        /// <summary>
        /// Тест удаления сущности в репозитории.
        /// После удаления в репозитории должно содержаться одна сущность (две добавляются изначально).
        /// </summary>
        [TestMethod]
        public void RemoveTestSimple_TwoEnityAddToRepoAndOneDelite()
        {
            // Arrange
            string nameFirst = "Name1";
            string nameSecond = "Name2";
            GestureRepository gestures = new GestureRepository();
            GestureModel gesture1 = GestureModel.GetDefault(Guid.NewGuid(), nameFirst);
            GestureModel gesture2 = GestureModel.GetDefault(Guid.NewGuid(), nameSecond);

            // Act
            gestures.Add(gesture1);
            gestures.Add(gesture2);
            gestures.Remove(gesture2);
            IEntitySpecification<GestureModel> specGetByName = new GesturesSpecificationByName(gesture1.Name);
            List<GestureModel> gesturesInRepo = new List<GestureModel>(gestures.Query(specGetByName));

            // Assert
            // TODO: Вернуть.
            Assert.IsTrue(gesturesInRepo.Count == 1 && gesturesInRepo[0].Equals(gesture1));
            //Assert.IsTrue(gesture2.Equals(gesturesInRepo?[0]));
        }

        /// <summary>
        /// Тест извлечения сущности из репозитория по Id.
        /// Выполняется добавление двух сущностей, после чего первая извлекается по Id, а вторая по имени.
        /// </summary>
        [TestMethod]
        public void QueryTestSimple_TwoEnityAndFirstGetByNameAndID()
        {
            // Arrange
            string nameFirst = "Name1";
            string nameSecond = "Name2";
            GestureRepository gestures = new GestureRepository();
            GestureModel gesture1 = GestureModel.GetDefault(Guid.NewGuid(), nameFirst);
            GestureModel gesture2 = GestureModel.GetDefault(Guid.NewGuid(), nameSecond);
            gestures.Add(gesture1);
            gestures.Add(gesture2);

            // Act
            IEntitySpecification<GestureModel> specGetById = new GesturesSpecificationById(gesture1.ID);
            List<GestureModel> gesturesInRepoFirstTest = new List<GestureModel>(gestures.Query(specGetById));

            IEntitySpecification<GestureModel> specGetByName = new GesturesSpecificationByName(gesture2.Name);
            List<GestureModel> gesturesInRepoSecondTest = new List<GestureModel>(gestures.Query(specGetByName));

            // Assert
            // TODO: Вернуть.
            Assert.IsTrue(gesturesInRepoFirstTest[0].Equals(gesture1));
            Assert.IsTrue(gesturesInRepoSecondTest[0].Equals(gesture2));
        }
    }
}