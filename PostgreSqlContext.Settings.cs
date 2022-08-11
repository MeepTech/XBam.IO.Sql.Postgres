using System.Collections.Generic;

namespace Meep.Tech.XBam.IO.Sql.Postgres {
  public partial class PostgreSqlContext {
    /// <summary>
    /// Settings for postgresql
    /// </summary>
    public new class Settings : SqlContext.Settings {
      internal readonly string _connectionString;

      /// <summary>
      /// If the properties with unknown sql data types should use json fields.
      /// </summary>
      public bool UseJsonFieldsForUnknownColumnDataTypes {
        get;
        init;
      } = false;

      public Settings(string connectionString, SqlContext.Settings.ModelDiscoveryMethod modelDiscoveryMethod = ModelDiscoveryMethod.XBamModelTypes | ModelDiscoveryMethod.TableAttributeOnlyClasses | ModelDiscoveryMethod.IncludedListClasses, HashSet<System.Type>? includedModelTypes = null) 
        : base(modelDiscoveryMethod, includedModelTypes) {
        _connectionString = connectionString;
      }
    }
  }
}
