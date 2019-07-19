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
                lock (this.gesturesField)
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
            
        }

        public IEnumerable<GestureModel> Query(IEntitySpecification<GestureModel> specification)
        {
            List<GestureModel> resultGestures = new List<GestureModel>();

            if (this.GesturesField is null)
            {
                this.GesturesField = this.LoadGestures();
            }

            foreach (GestureModel gesture in this.GesturesField)
            {
                if (specification.Specified(gesture))
                {
                    resultGestures.Add(gesture);
                }
            }

            return resultGestures;
        }

        public void RemoveAccount(GestureModel gesture)
        {
            throw new NotImplementedException();
        }

        public void UpdateAccount(GestureModel gesture)
        {
            throw new NotImplementedException();
        }

        private List<GestureModel> LoadGestures()
        {
            List<GestureModel> gestures = new List<GestureModel>();



            return gestures;
        }
    }
}
