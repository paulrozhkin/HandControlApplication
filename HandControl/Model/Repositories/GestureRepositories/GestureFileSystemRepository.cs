// -------------------------------------------------------------------------------------
// <copyright file = "GestureRepository.cs" company = "Студенческий проект HandControl‎"> 
//      Copyright © 2019 HandControl. All rights reserved.
// </copyright> 
// -------------------------------------------------------------------------------------
namespace HandControl.Model.Repositories.GestureRepositories
{

    using System;
    using System.Collections.Generic;
    using HandControl.Services;
    
    /// <summary>
    /// Репозиторий, содержащий жесты, хранимые в системе.
    /// </summary>
    public class GestureFileSystemRepository : IGestureRepository
    {
        /// <summary>
        /// Коллекция жестов.
        /// </summary>
        private List<GestureModel> gesturesCacheField = null;

        private List<GestureModel> Gestures
        {
            get
            {
                lock (this)
                {
                    if (this.gesturesCacheField == null)
                    {
                        this.gesturesCacheField = this.LoadGestures();
                    }

                    return this.gesturesCacheField;
                }
            }
        }

        public void Add(GestureModel gesture)
        {
            lock (this)
            {
                gesture = gesture.Clone() as GestureModel;

                bool isContains = false;
                for (int i = 0; i < this.Gestures.Count; i++)
                {
                    if (gesture.ID.Equals(this.Gestures[i].ID))
                    {
                        this.Gestures[i] = gesture;
                        isContains = true;
                        break;
                    }
                    else
                    {
                        if (gesture.Name.Equals(this.Gestures[i].Name))
                        {
                            throw new ArgumentException("An entity with the same name is already in the collection.");
                        }
                    }
                }

                if (!isContains)
                {
                    this.Gestures.Add(gesture);
                }

                ////TODO: Сохранение в файловую систему и синхронизация.
                byte[] data = gesture.BinarySerialize();
                FileSystemFacade.WriteBinaryData(PathManager.GetGesturePath(gesture.ID.ToString()), data);

            }
        }

        public IEnumerable<GestureModel> Query(IEntitySpecification<GestureModel> specification)
        {
            List<GestureModel> resultGestures = new List<GestureModel>();

            lock (this)
            {
                foreach (GestureModel gesture in this.Gestures)
                {
                    if (specification.Specified(gesture))
                    {
                        resultGestures.Add(gesture.Clone() as GestureModel);
                    }
                }
            }

            return resultGestures;
        }

        public void Remove(GestureModel gesture)
        {
            lock (this)
            {
                if (!this.Gestures.Remove(gesture))
                {
                    throw new ArgumentException("Unable to delete.");
                }
                else
                {
                    ////TODO: Удаление из файловой системы и синхронизация.
                    FileSystemFacade.DeleteFolder(PathManager.GetGestureFolderPath(gesture.ID.ToString()));
                }
            }
        }

        private List<GestureModel> LoadGestures()
        {
            List<GestureModel> gestures = new List<GestureModel>();

            foreach (string file in PathManager.GetGesturesFilesPaths())
            {
                try
                {
                    GestureModel newGesture = GestureModel.GetDefault(new Guid(),string.Empty);
                    newGesture.BinaryDesserialize(FileSystemFacade.ReadBinaryData(file));
                    gestures.Add(newGesture);
                }
                catch(Exception)
                {
                    continue;
                }
            }

            return gestures;
        }
    }
}
