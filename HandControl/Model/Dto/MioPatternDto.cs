using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandControl.Model.Dto
{
    public class MioPatternDto
    {
        /// <summary>
        /// Паттерн системы.
        /// </summary>
        public int Pattern { get; set; }

        public Guid GestureId { get; set; }
    }
}
