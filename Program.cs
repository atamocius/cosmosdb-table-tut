using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace cosmosdb_table_tut
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var root = Directory.GetCurrentDirectory();
            var dotenv = Path.Combine(root, ".env");
            DotEnv.Load(dotenv);

            var config =
                new ConfigurationBuilder()
                    .AddEnvironmentVariables()
                    .Build();

            var CONNECTION_STRING = config["CONNECTION_STRING"];

            var db = new TableDB(CONNECTION_STRING);
            var demo = new Demo();

            await demo.Run(db);
        }
    }
}
