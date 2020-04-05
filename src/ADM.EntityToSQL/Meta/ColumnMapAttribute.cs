using System;
using System.Collections.Generic;
using System.Text;

namespace E2S.Meta
{
    [AttributeUsage( AttributeTargets.Property )]
    public class ColumnMapAttribute : Attribute
    {
        public string Name { get; set; }
    }
}
