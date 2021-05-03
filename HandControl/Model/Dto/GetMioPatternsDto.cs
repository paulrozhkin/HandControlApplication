using System;
using System.Collections.Generic;

namespace HandControl.Model.Dto
{
    public class GetMioPatternsDto
    {
        public IEnumerable<MioPatternDto> Patterns { get; set; }
    }
}
