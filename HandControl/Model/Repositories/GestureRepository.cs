using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HandControl.Model;

namespace HandControl.Model.Repositories
{
    public class GestureRepository : IRepository<GestureModel>
    {
        private List<GestureModel> gesturesField = null;

        private List<GestureModel> Gestures
        {
            get
            {
                lock (this)
                {
                    if (this.gesturesField == null)
                    {
                        this.gesturesField = this.LoadGestures();
                    }

                    return this.gesturesField;
                }
            }
        }

        public void Add(GestureModel gesture)
        {
            lock (this.Gestures)
            {
                bool isContains = false;
                for (int i = 0; i < this.Gestures.Count; i++)
                {
                    if (gesture.ID.Equals(this.Gestures[i].ID))
                    {
                        this.Gestures[i] = gesture.Clone() as GestureModel;
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

                //TODO: Сохранение в файловую систему и синхронизация.

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
                    //TODO: Удаление из файловой системы и синхронизация.
                }
            }
        }

        private List<GestureModel> LoadGestures()
        {
            List<GestureModel> gestures = new List<GestureModel>();



            return gestures;
        }
    }
}
