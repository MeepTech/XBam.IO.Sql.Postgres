using Meep.Tech.Collections.Generic;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Meep.Tech.XBam.IO.Sql.Postgres {
  public static partial class Querries {
    public static partial class Opperations {
      public static partial class Initial {
        public class UPSERT : Sql.Query.Opperation.Initial.UPSERT {

          UPSERT()
            : base() { }

          protected override (string text, IEnumerable<object> parameters) BuildPostInsertText(Query.Builder.Token token, SqlContext sqlContext) {
            throw new System.NotImplementedException();
          }

          protected override (string text, IEnumerable<object> parameters) BuildPreInsertText(Query.Builder.Token token, SqlContext sqlContext) {
            throw new System.NotImplementedException();
          }
        }
      }
    }
  }
}
