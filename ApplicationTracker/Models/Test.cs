using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace ApplicationTracker.Models
{
    public class Test
    {
        static jagdevEntities entities = new jagdevEntities();
        public static int TestMethod(CancellationToken token) {
            //int testiterator;
            using (entities)
            {
                while (true)
                {
                    //testiterator = entities.ae_RebuildAllStatements();
                    if (token.IsCancellationRequested)
                    {
                        return 18797;
                    }
                }
            }
            //return 1;
        }
    }
}