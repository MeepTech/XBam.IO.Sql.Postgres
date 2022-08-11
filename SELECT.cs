namespace Meep.Tech.XBam.IO.Sql.Postgres {
  public static partial class Querries {
    public static partial class Opperations {
      public static partial class Initial {
        public class SELECT : Sql.Query.Opperation.Initial.SELECT {

          SELECT()
            : base() { }

          protected override string BuildJsonDataPropertySelectColumn(string propertyName)
            => SqlContext.JsonDataColumnName + "->>'" + propertyName + "' AS " + propertyName;
        }
      }
    }
  }
}
