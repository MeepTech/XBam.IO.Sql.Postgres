namespace Meep.Tech.XBam.IO.Sql.Postgres {
  public static partial class Querries {
    public static partial class Opperations {
      public static partial class Initial {
        public class UPDATE : Sql.Query.Opperation.Initial.UPDATE {

          UPDATE()
            : base() { }

          protected override string BuildSetStatement((string name, object? value, bool isJson) entry, string parameterKey) {
            if (entry.isJson) {
              return $"\n\t{SetColumnValueCommandText} {SqlContext.JsonDataColumnName} = JSONB_SET({SqlContext.JsonDataColumnName}, {entry.name}, {parameterKey})";
            }
            else return base.BuildSetStatement(entry, parameterKey);
          }

          protected override string GetValueReplacementKey(SqlContext sqlContext, int index, bool withPrefix = true, bool forJsonField = false) 
            => base.GetValueReplacementKey(sqlContext, index, withPrefix, forJsonField) + (forJsonField ? "::jsonb" : "");
        }
      }
    }
  }
}
