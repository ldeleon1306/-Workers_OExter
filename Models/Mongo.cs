using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Threading.Tasks;

namespace Workers.Models
{
    public class Mongo
    {
        // static  List<string> GetMongoCollectionAsync()
        //{
        //    var client = new MongoClient("mongodb://10.20.2.46:27017");
        //    List<string> NombrebaseDatos = client.ListDatabaseNames().ToList();
        //    var database = client.GetDatabase("APIAlmacenes");

        //    var collection = database.GetCollection<BsonDocument>("TransaccionesPedidos");

        //    dynamic list = collection.Find(new BsonDocument())
        //         .Limit(2) //retrive only two documents
        //        .ToListAsync();

        //    List<string> listRange = new List<string>();
        //    foreach (var docs in list)
        //    {
        //        //_logger.LogInformation(docs.ToString());
        //        Console.WriteLine("Idtransaccion: " + docs["response"]["idtransaccion"] + "  Estado: " + docs["estado"]);
            
        //        listRange.Add((string)docs["response"]["idtransaccion"]);
        //        listRange.Add((string)docs["estado"]);
        //        //_logger.LogInformation(docs.ToString());
        //        //Console.WriteLine(docs);
        //    }

        //    return listRange;   
        //}

        internal static List<CollectionMongo> GetMongoCollection()
        {         
            List<CollectionMongo> listRange = new List<CollectionMongo>();
            try
            {
                var client = new MongoClient("mongodb://10.20.2.46:27017");
                List<string> NombrebaseDatos = client.ListDatabaseNames().ToList();
                var database = client.GetDatabase("APIAlmacenes");

                var collection = database.GetCollection<BsonDocument>("TransaccionesPedidos");

                var list = collection.Find(new BsonDocument())
                     .Limit(2) //retrive only two documents
                    .ToList();
        
                foreach (var docs in list)
                {
                    //_logger.LogInformation(docs.ToString());
                    Console.WriteLine("Idtransaccion: " + docs["response"]["idtransaccion"] + "  Estado: " + docs["estado"]);

                    listRange.Add(new CollectionMongo() { idtransaccion = (int)Convert.ToInt64(docs["response"]["idtransaccion"]), estado = (string)docs["estado"] });
                    //_logger.LogInformation(docs.ToString());
                    //Console.WriteLine(docs);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }            

            return listRange;
        }

        //internal static List<string> GetMongoCollection()
        //{
        //    throw new NotImplementedException();
        //}
    }
}
