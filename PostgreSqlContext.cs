using System;
using System.Collections.Generic;
using Meep.Tech.Collections.Generic;
using Npgsql;
using Meep.Tech.XBam.Reflection;
using System.ComponentModel.DataAnnotations.Schema;
using Meep.Tech.XBam.IO.Sql.Metadata;

namespace Meep.Tech.XBam.IO.Sql.Postgres {
  public partial class PostgreSqlContext : SqlContext {
    IReadOnlyDictionary<Type, Query.Opperation>? _operationTypes;

    public new Settings Options {
      get;
    }

    public PostgreSqlContext(Settings options)
      : base(options) { Options = options; }

    public override IReadOnlyDictionary<Type, Query.Opperation> OperationTypes
      => _operationTypes ??= base.OperationTypes
        .Append(
          typeof(Querries.Opperations.Initial.SELECT_TOP),
          Query.Builder.Token.Types.Get<Querries.Opperations.Initial.SELECT_TOP>()
        );

    protected override Query.Opperation.Initial.SELECT SelectArchetype
      => Query.Builder.Token.Types.Get<Querries.Opperations.Initial.SELECT>();

    protected override Query.Opperation.Initial.INSERT InsertArchetype
      => Query.Builder.Token.Types.Get<Querries.Opperations.Initial.INSERT>();

    protected override Query.Opperation.Initial.UPSERT UpsertArchetype 
      => Query.Builder.Token.Types.Get<Querries.Opperations.Initial.UPSERT>();

    protected override Query.Opperation.Initial.UPDATE UpdateArchetype
      => Query.Builder.Token.Types.Get<Querries.Opperations.Initial.UPDATE>();

    protected override string GetColumnDataType(Column column)
      => column.Property is not null ? column.Property.PropertyType switch {
        Type when column.Property.PropertyType.TryToGetAttribute<ColumnAttribute>(out var attribute)
          && attribute.TypeName is not null
            => attribute.TypeName,
        Type boolType when boolType == typeof(bool)
          || boolType == typeof(Boolean)
            => "boolean",
        Type smallType when smallType == typeof(short)
          || smallType == typeof(byte)
          || smallType == typeof(Byte)
          || smallType == typeof(sbyte)
            => "smallint",
        Type intType when intType == typeof(int)
          || intType == typeof(Int32)
            => "integer",
        Type bigType when bigType == typeof(long)
          || bigType == typeof(Int64)
            => "bigint",
        Type floatType when floatType == typeof(float)
            => "real",
        Type doubleType when doubleType == typeof(double)
          || doubleType == typeof(Double)
            => "double precision",
        Type numericType when numericType == typeof(decimal)
          || numericType == typeof(Decimal)
          || numericType == typeof(System.Numerics.BigInteger)
            => "numeric",
        Type textType when textType == typeof(string)
          || textType == typeof(String)
          || textType == typeof(char)
          || textType == typeof(Char)
          || textType == typeof(char[])
          || textType == typeof(Char[])
          || typeof(Enum).IsAssignableFrom(textType)
            => "text",
        Type uniqueIdType when uniqueIdType == typeof(Guid)
            => "uuid",
        Type byteArrayType when byteArrayType == typeof(byte[])
            || byteArrayType == typeof(sbyte[])
            || byteArrayType == typeof(Byte[])
            => "bytea",
        Type dateTimeType when dateTimeType == typeof(DateTime)
            => "timestamp with time zone",
        Type timeSpanType when timeSpanType == typeof(TimeSpan)
            => "interval",
        Type mapType when typeof(IDictionary<string, string>).IsAssignableFrom(mapType)
            => "hstore",
        Type when PropertyShouldBeAutoPorted(column.Property)
            => "text",

        _ => Options.UseJsonFieldsForUnknownColumnDataTypes
          ? "jsonb"
          : throw new NotImplementedException($"No Registered SQL Data Type for Property Type: {column.Property.PropertyType.ToFullHumanReadableNameString()}"),
      } : throw new NotImplementedException($"No Registered SQL Data Type for Column: {column.Name}");

    protected override Query.Result ExecuteDatabaseQuery(string query, IReadOnlyList<object>? parameters) {
      NpgsqlDataReader? reader = null;
      NpgsqlConnection? connection = null;
      try {
        connection = new(Options._connectionString);
        connection.Open();

        NpgsqlCommand? command = new(query, connection);
        parameters?.ForEach((p, i) => 
          command.Parameters.Add(new NpgsqlParameter(
            GetValueReplacementKey(i, false),
            p
          )));

        reader = command.ExecuteReader();

        List<IEnumerable<RawCellData>>? rows = reader.HasRows 
          ? new() 
          : null;

        while (reader.Read()) {
          RawCellData[] cellValues = new RawCellData[reader.FieldCount];
          for (int i = 0; i < reader.FieldCount; i++) {
            cellValues[i] = MakeRawCell(
              reader.GetName(i),
              reader.GetDataTypeName(i),
              reader.GetValue(i),
              reader.GetFieldType(i)
            );
          }
          rows!.Add(cellValues);
        }

        return Success(rows);
      }
      catch (Exception e) {
        return Failure(e);
      }
      finally {
        reader?.Dispose();
        connection?.Dispose();
      }
    }

#if DEBUG

    /// <summary>
    /// Execute the query to build all tables.
    /// </summary>
    /// <param name="checkExists"></param>
    public Query.Result DropTable(Table table)
      => base.DropTable(table);

#endif
  }
}
