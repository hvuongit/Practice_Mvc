using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StructureMap.Configuration.DSL;

namespace Practice_Mvc.Infrastructure
{
    public class StandardRegistry: Registry
    {
        public StandardRegistry()
        {
            Scan(scan =>
            {
                scan.TheCallingAssembly();
                scan.WithDefaultConventions();
            });
        }
    }
}