// -------------------------------------------------------------------------------------
// <copyright file = "GestureRepository.cs" company = "Студенческий проект HandControl‎"> 
//      Copyright © 2019 HandControl. All rights reserved.
// </copyright> 
// -------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using HandControl.Services;

namespace HandControl.Model.Repositories.GestureRepositories
{
    /// <summary>
    ///     Репозиторий, содержащий жесты, хранимые в системе.
    /// </summary>
    public class GestureFileSystemRepository : IGestureRepository
    {
        /// <summary>
        ///     Коллекция жестов.
        /// </summary>
        private List<GestureModel> gesturesCacheField;

        private List<GestureModel> Gestures
        {
            get
            {
                lock (this)
                {
                    if (gesturesCacheField == null) gesturesCacheField = LoadGestures();

                    return gesturesCacheField;
                }
            }
        }

        public void Add(GestureModel gesture)
        {
            lock (this)
            {
                gesture = gesture.Clone() as GestureModel;

                var isContains = false;
                for (var i = 0; i < Gestures.Count; i++)
                    if (gesture.Id.Equals(Gestures[i].Id))
                    {
                        Gestures[i] = gesture;
                        isContains = true;
                        break;
                    }
                    else
                    {
                        if (gesture.Name.Equals(Gestures[i].Name))
                            throw new ArgumentException("An entity with the same name is already in the collection.");
                    }

                if (!isContains) Gestures.Add(gesture);

                ////TODO: Сохранение в файловую систему и синхронизация.
                var data = gesture.BinarySerialize();
                FileSystemFacade.WriteBinaryData(PathManager.GetGesturePath(gesture.Id.ToString()), data);

                //TODO: для прототипа, потом убрать.
                FileSystemFacade.WriteBinaryData(
                    PathManager.GetGestureFolderPath(gesture.Id.ToString()) + gesture.Name + ".bin", data);
            }
        }

        public IEnumerable<GestureModel> Query(IEntitySpecification<GestureModel> specification)
        {
            var resultGestures = new List<GestureModel>();

            lock (this)
            {
                foreach (var gesture in Gestures)
                    if (specification.Specified(gesture))
                        resultGestures.Add(gesture.Clone() as GestureModel);
            }

            return resultGestures;
        }

        public void Remove(GestureModel gesture)
        {
            lock (this)
            {
                if (!Gestures.Remove(gesture))
                    throw new ArgumentException("Unable to delete.");
                FileSystemFacade.DeleteFolder(PathManager.GetGestureFolderPath(gesture.Id.ToString()));
            }
        }

        private List<GestureModel> LoadGestures()
        {
            var gestures = new List<GestureModel>();

            foreach (var file in PathManager.GetGesturesFilesPaths())
                try
                {
                    var newGesture = GestureModel.GetDefault(new Guid(), string.Empty);
                    newGesture.BinaryDesserialize(FileSystemFacade.ReadBinaryData(file));
                    gestures.Add(newGesture);
                }
                catch (Exception)
                {
                }

            return gestures;
        }
    }
}