using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace L2.Net.Skills.Effects
{
    //public delegate void 

    public abstract class AbstractEffect : ICompilable
    {
        public abstract void Initialize();
    }
}
