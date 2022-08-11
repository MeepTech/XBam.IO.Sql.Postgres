using Meep.Tech.Collections.Generic;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Meep.Tech.XBam.IO.Sql.Postgres {
  public static partial class Querries {
    public static partial class Opperations {
      public static partial class Initial {

        public class INSERT : Sql.Query.Opperation.Initial.INSERT {

          INSERT()
            : base() { }

          protected override string GetJsonDataKey(SqlContext sqlContext, ref int queryParamIndex) 
            => base.GetJsonDataKey(sqlContext, ref queryParamIndex) + "::jsonb";
        }
      }
    }
  }
}
