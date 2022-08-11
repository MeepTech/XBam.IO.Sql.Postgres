namespace Meep.Tech.XBam.IO.Sql.Postgres {
  public static partial class Querries {
    public static partial class Opperations {
      public static partial class Initial {
        public class SELECT_TOP : Sql.Query.Opperation.Initial.SELECT {

          public static Parameter CountParameter {
            get;
          } = new Parameter("Count", typeof(int));

          SELECT_TOP()
            : base(
                new SELECT_TOP.Identity(nameof(SELECT_TOP)),
                new[] { CountParameter }
            ) { }
        }
      }
    }
  }
}
