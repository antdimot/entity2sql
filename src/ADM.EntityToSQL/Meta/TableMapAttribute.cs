using System;
using System.Collections.Generic;
using System.Text;

namespace ADM.EntityToSQL.Meta
{
    [AttributeUsage( AttributeTargets.Class )]
    public class TableMapAttribute : Attribute
    {
        public string Name { get; set; }
    }
}
