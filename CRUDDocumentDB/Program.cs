using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json;
using System.Net;

namespace CRUDDocumentDB
{
   public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("1: Crear DB");
            Console.WriteLine("2: Eliminar DB");
            Console.WriteLine("3: Crear collecion");
            string option = Console.ReadLine();
            Program p = new Program();
            string DB = "";
            string Coll = "";
            switch (option)
            {
                    
                case "1":
                    Console.WriteLine("Nombre de la DB");
                    string name = Console.ReadLine();
                    p.CreateDB(name).Wait();
                    break;
                case "2":
                    Console.WriteLine("Nombre de la DB");
                    string ID = Console.ReadLine();
                    p.DeleteDB(ID).Wait();
                    break;
                case "3":
                    Console.WriteLine("Nombre de la DB");
                    DB = Console.ReadLine();
                    Console.WriteLine("Nombre de la Colleccion");
                    Coll = Console.ReadLine();
                    p.CreateCollection(DB,Coll).Wait();
                    break;
            }

        }

        private async Task CreateDB(string name)
        {
            DocumentClient client = getDocument();
            try
            {
               //await client.ReadDatabaseAsync(UriFactory.CreateDatabaseUri(name));
                await client.CreateDatabaseAsync(new Database { Id = name });
                Console.WriteLine("Creada DB :"+name);
                Console.ReadKey();
            }
            catch (DocumentClientException  ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }
        }
        private async Task DeleteDB(string ID)
        {
            DocumentClient client = getDocument();
            try
            {
                await client.DeleteDatabaseAsync(UriFactory.CreateDatabaseUri(ID));
                Console.WriteLine("Eliminada DB :" + ID);
                Console.ReadKey();
            }
            catch (DocumentClientException ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }

        }

        private async Task CreateCollection(string DB,string ID)
        {
            DocumentClient client = getDocument();
            try
            {
                DocumentCollection collectionInfo = new DocumentCollection();
                collectionInfo.Id = ID;
                collectionInfo.IndexingPolicy = new IndexingPolicy(new RangeIndex(DataType.String) { Precision = -1 });
                //await client.DeleteDatabaseAsync(UriFactory.CreateDocumentCollectionUri(DB,ID));
                await client.CreateDocumentCollectionAsync(UriFactory.CreateDatabaseUri(DB), collectionInfo, new RequestOptions { OfferThroughput = 400 });
                Console.WriteLine("Creada Collecion :" + ID);
                Console.ReadKey();
            }
            catch (DocumentClientException ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }

        }
        private async Task DeleteCollection(string DB, string ID)
        {
            DocumentClient client = getDocument();
            try
            {
                await client.DeleteDatabaseAsync(UriFactory.CreateDocumentCollectionUri(DB, ID));
                Console.WriteLine("Eliminada Collecion :" + ID);
                Console.ReadKey();
            }
            catch (DocumentClientException ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }

        }

        //Helper//

        public static DocumentClient getDocument()
        {
            DocumentClient client = null;
            try{
                 client = new DocumentClient(
                 new Uri("https://localhost:8081"),
                 "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.ReadKey();
            }
            return client;
            

        }


    }
}
