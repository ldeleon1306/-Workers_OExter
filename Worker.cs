using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace WorkerConecMongo
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                //Context.Wap_IngresosPedidosContexts s = new Context.Wap_IngresosPedidosContexts();
                //foreach (var item in s.WapIngresopedidos.Take(10).ToList())
                //{
                //    Console.WriteLine(item.OrdenExterna1);
                //}


                //            var client = new MongoClient("mongodb://10.20.7.44:27017");
                //            Console.WriteLine("conecto a mongo 20");
                //            Console.WriteLine("entra a mongo 22");
                //            List<string> NombrebaseDatos = client.ListDatabaseNames().ToList();
                //            Console.WriteLine("entra a mongo 23");
                //            var database = client.GetDatabase("APIAlmacenes");
                //            Console.WriteLine(database);

                //            List<CollectionMongo> listRange = new List<CollectionMongo>();
                //            var collection = database.GetCollection<BsonDocument>("TransaccionesPedidos");
                //            var list = collection.Find(new BsonDocument())
                //               .Limit(2) //retrive only two documents
                //              .ToList();

                //            foreach (var docs in list)
                //            {
                //                //_logger.LogInformation(docs.ToString());
                //                //Console.WriteLine("Idtransaccion: " + docs["response"]["idtransaccion"] + "  Estado: " + docs["estado"]);

                //                listRange.Add(new CollectionMongo() { idtransaccion = (int)Convert.ToInt64(docs["response"]["idtransaccion"]), estado = (string)docs["estado"] });
                //                //_logger.LogInformation(docs.ToString());
                //                Console.WriteLine(docs);
                //            }

                Wap_IngresosPedidosContext db = new Wap_IngresosPedidosContext();
                WAP_INGRESOPEDIDOS wp = new WAP_INGRESOPEDIDOS();
                var idlist = new int[] { 2, 3 };
                Console.WriteLine("antes  raw sql");

                var students = db.WAP_INGRESOPEDIDOS
                  .FromSqlRaw("SELECT * FROM [InterfacesWHS ].[dbo].[WAP_INGRESOPEDIDOS]")
    .Take(10)
    .ToList();

                Console.WriteLine("despues  raw sql");
                Console.WriteLine(students.Count());


                //var estados = from p in db.WAP_INGRESOPEDIDOS
                //              where p.IdTransacción == "660" && idlist.Contains(p.Estado)
                //              select new WAP_INGRESOPEDIDOS()
                //              {
                //                  IdTransacción = p.IdTransacción,
                //                  Estado = p.Estado,
                //                  Propietario = p.Propietario,
                //                  RazonFalla = p.RazonFalla,
                //                  Almacen = p.Almacen,
                //                  OrdenExterna1 = p.OrdenExterna1
                //              };

                //Console.WriteLine("contar estados");
                //int a = 0;
                //foreach (var item in estados)
                //{
                //    a++;
                //}
                //    Console.WriteLine("estados");
                //    Console.WriteLine(a);

                try
                {
                    MailMessage mail = new MailMessage();
                    //Console.WriteLine(_mailFrom.ToString());
                    //Console.WriteLine(_mailTo);
                    mail.From = new MailAddress("apptestbrdcsrv@andreani.com");
                    mail.To.Add("ldeleon@andreani.com");
                    //var multiple = _mailTo.Split(';');
                    //foreach (var to in multiple)
                    //{
                    //    if (to != string.Empty)
                    //        mail.To.Add(to);
                    //}              
                    System.Net.Mail.Attachment attachment;
                    //attachment = new System.Net.Mail.Attachment("output.txt");
                    //mail.Attachments.Add(attachment);
                    //if (!String.IsNullOrEmpty(txterror))
                    //{
                    //  attachment = new System.Net.Mail.Attachment(txterror);
                    //  mail.Attachments.Add(attachment);
                    //}
                    string cliente = "prueba";
                    string subject = string.Format($"Prueba {cliente} Mail");
                    string bodyMsg = string.Format($"Se procesó un mongo {cliente}");
                    mail.Subject = subject;
                    mail.Body = bodyMsg;
                    mail.IsBodyHtml = true;
                    Console.WriteLine(bodyMsg);
                    SmtpClient smtp = new SmtpClient("10.20.7.16");
                    smtp.EnableSsl = false;
                    smtp.Port = 25;
                    smtp.UseDefaultCredentials = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("dentro de casa mail");
                    //Console.WriteLine(_mailFrom);
                    //Console.WriteLine(_mailTo);
                    Console.WriteLine(ex.Message);
                    //_logger.LogError(ex, "Se produjo una excepción en el metodo SendEmail: ", ex.Message);
                    throw ex;
                }
               

                await Task.Delay(300000, stoppingToken);
            }
        }
    }
}
