using System;
using System.Collections.Generic;
using System.Text;

namespace ADM.EntityToSQL.Meta
{
    [AttributeUsage( AttributeTargets.Property )]
    public class ColumnMapAttribute : Attribute
    {
        public string Name { get; set; }
    }
}
