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
using Microsoft.EntityFrameworkCore;
using Workers.Models;

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
                    CompareAsync();
                    await Task.Delay(60000, stoppingToken);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.InnerException);
                    throw;
                }

            }
        }

        public async Task CompareAsync()
        {
            try
            {
                try
                {
                    //MONGO
                    List<CollectionMongo> ListPedMongo = Mongo.GetMongoCollection();
                    try
                    {
                        foreach (var listpedmongo in ListPedMongo)
                        {                           
                            //WAP
                            using (Wap_IngresosPedidosContext db = new Wap_IngresosPedidosContext())
                            {
                                var estados = db.WAP_INGRESOPEDIDOS
                                    .Where(p => p.IdTransacción == Convert.ToString(listpedmongo.idtransaccion) && p.Estado == 2);//NOS IMPORTA SOLO ESTADO 2, ESTADO 3 ES REPETIDO
                                //SI NO ESTA EN ESTADO 2, BUSCAR EN 3 SI QUEDO CON ERROR, SINO ESTA EN AMBOS, HAY QUE INFORMAR DESDE MONGO O VER O VER
                                //SI ESTA EN ESTADO 2 O 3 CHEQUEAMOS EN SCE
                                foreach (WAP_INGRESOPEDIDOS e in estados)
                                {
                                    Console.WriteLine("Idtransaccion: "+ e.IdTransacción + " Propietario: " +  e.Propietario + " Estado: " + e.Estado + " RazonFalla: " + e.RazonFalla);
                                }
                                //FIN WAP
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                Mail.Mail m = new Mail.Mail(_configuration);
                m.SendEmail("nada", "cliente");
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                throw;
            }
        }
    }
}
