using System;
using System.Collections.Generic;
using System.Text;

namespace E2S.Meta
{
    [AttributeUsage( AttributeTargets.Class )]
    public class TableMapAttribute : Attribute
    {
        public string Name { get; set; }
    }
}
