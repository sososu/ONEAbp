using System;
using System.Collections.Generic;

namespace Volo.Abp.Identity
{
    public class TreeLabel
    {
        public Guid Id { get; set; }

        public string Label { get; set; }

        public IList<TreeLabel> Children { get; set; }
    }
}
