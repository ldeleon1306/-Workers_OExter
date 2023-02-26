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

namespace Workers
{
    public class WorkerFilterCheck : BackgroundService
    {
        private readonly ILogger<WorkerFilterCheck> _logger;
        private readonly IConfiguration _configuration;

        public WorkerFilterCheck(ILogger<WorkerFilterCheck> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _logger.LogInformation("inicio: {time}", DateTimeOffset.Now);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation("ANTES DE COMPARAR: {time}", DateTimeOffset.Now);
                    Console.WriteLine("ANTES DE COMPARAR");
                    //Log.Logger = new LoggerConfiguration().WriteTo.File(@"C:\Users\ldeleon\OneDrive - ANDREANI LOGISTICA SA\Documentos\Bp\vane caso tarejta naranja\EventosLog\LogEvents" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".log").CreateLogger();
                    CompareAsync();
                    Console.WriteLine("termino mala");
                    await Task.Delay(60000, stoppingToken);
                    Console.WriteLine("termino malb");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.InnerException);
                    Console.WriteLine("termino mal");
                    throw;
                }

            }
        }

        private async Task CompareAsync()
        {
            try
            {
                Console.WriteLine("CompareAsync");
                try
                {
                    var client = new MongoClient("mongodb://10.20.2.46:27017");
                    List<string> NombrebaseDatos = client.ListDatabaseNames().ToList();
                    var database = client.GetDatabase("APIAlmacenes");

                    var collection = database.GetCollection<BsonDocument>("TransaccionesPedidos");
                    int cont = 0;


                    var list = await collection.Find(new BsonDocument())
                .Limit(2) //retrive only two documents
                .ToListAsync();
                    foreach (var docs in list)
                    {
                        _logger.LogInformation(docs.ToString());
                        Console.WriteLine(docs);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("error en mongo collection linea 78");
                    Console.WriteLine(ex.Message);
                }
                _logger.LogInformation("ANTES DAL MAIL: {time}", DateTimeOffset.Now);
                Console.WriteLine("linea 84");
                Mail.Mail m = new Mail.Mail(_configuration);
                Console.WriteLine("linea 85");
                m.SendEmail("nada", "cliente");
                //Console.WriteLine(cont);
                Console.WriteLine("ANTES DAL MAIL");
            }
            catch (Exception ex)
            {
                Console.WriteLine("error mail");
                Console.WriteLine(ex.Message);
                _logger.LogInformation(ex.Message);
                throw;
            }
        }
    }
}
