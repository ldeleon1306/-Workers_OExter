using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;
using System.IO;
using System.Net.Http;
using System.Text;
using Serilog;
using Microsoft.Extensions.Configuration;

namespace WorkerS_OExtern
{
    public class WorkerFilterCheck : BackgroundService
    {
        private readonly ILogger<WorkerFilterCheck> _logger;
        private readonly IConfiguration _configuration;

        public WorkerFilterCheck(ILogger<WorkerFilterCheck> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                Log.Logger = new LoggerConfiguration().WriteTo.File(@"C:\Users\ldeleon\OneDrive - ANDREANI LOGISTICA SA\Documentos\Bp\vane caso tarejta naranja\EventosLog\LogEvents" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".log").CreateLogger();
                CompareAsync();
                await Task.Delay(9000000, stoppingToken);
            }
        }

        private async Task CompareAsync()
        {
            try
            {
                var client = new MongoClient("mongodb://10.20.2.46:27017");
                List<string> NombrebaseDatos = client.ListDatabaseNames().ToList();
                var database = client.GetDatabase("APIAlmacenes");

                var collection = database.GetCollection<BsonDocument>("TransaccionesPedidos");
                int cont = 0;
                //using (StreamReader leer = new StreamReader(@"C:\Users\ldeleon\OneDrive - ANDREANI LOGISTICA SA\Documentos\Bp\vane caso tarejta naranja\ordenExt.txt"))
                using (StreamReader leer = new StreamReader(@"C:\Users\ldeleon\OneDrive - ANDREANI LOGISTICA SA\Documentos\Bp\vane caso tarejta naranja\idtran.txt"))
                {
                    while (!leer.EndOfStream)
                    {
                        string url = leer.ReadLine();

                        try
                        {
                            var list = await collection.Find(new BsonDocument())
                        .Limit(2) //retrive only two documents
                        .ToListAsync();
                            foreach (var docs in list)
                            {
                                Console.WriteLine(docs);
                            }
                        }
                        catch (Exception ex)
                        {


                        }
                        break;

                    }
                }
                Mail.Mail m = new Mail.Mail(_configuration);
                m.SendEmail("nada", "cliente");
                Console.WriteLine(cont);

            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
