using System;
using System.Collections.Generic;

namespace HandControl.Model.Dto
{
    public class GestureDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime LastTimeSync { get; set; }
        public bool Iterable { get; set; }
        public int Repetitions { get; set; }
        public IEnumerable<GestureActionDto> Actions { get; set; }
    }
}
