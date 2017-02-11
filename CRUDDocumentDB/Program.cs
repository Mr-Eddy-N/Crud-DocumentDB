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
            while (true) { 
            Console.WriteLine("1: Crear DB");
            Console.WriteLine("2: Eliminar DB");
            Console.WriteLine("3: Crear collecion");
            Console.WriteLine("4: Eliminar collecion");
            Console.WriteLine("5: Crear Documento");
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
                case "4":
                    Console.WriteLine("Nombre de la DB");
                    DB = Console.ReadLine();
                    Console.WriteLine("Nombre de la Colleccion");
                    Coll = Console.ReadLine();
                    p.DeleteCollection(DB, Coll).Wait();
                    break;
                case "5":
                    Console.WriteLine("Nombre de la DB");
                    DB = Console.ReadLine();
                    Console.WriteLine("Nombre de la Colleccion");
                    Coll = Console.ReadLine();
                    Family fam = new Family();
                    p.CreateDocument(DB, Coll,fam).Wait();
                    break;
                case "6":
                    Console.WriteLine("Nombre de la DB");
                    DB = Console.ReadLine();
                    Console.WriteLine("Nombre de la Colleccion");
                    Coll = Console.ReadLine();
                    Family fam2 = new Family();
                    fam2.Id = "Los Rojas";
                    fam2.LastName = "Rojas";
                    Parent mama= new Parent{FamilyName="Rojas",FirstName="Lore"};
                    //fam2.Parents.
                    //p.CreateDocument(DB, Coll, fam).Wait();
                    break;

            }
            Console.Clear();
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
               // await client.DeleteDatabaseAsync(UriFactory.CreateDocumentCollectionUri(DB, ID));
                await client.DeleteDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(DB, ID));
                Console.WriteLine("Eliminada Collecion :" + ID);
                Console.ReadKey();
            }
            catch (DocumentClientException ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }

        }

        private async Task CreateDocument(string DB, string Coll,Family fam)
        {
            DocumentClient client = getDocument();
            try
            {

                  dynamic document1Definition = new {
                  name = "New Customer 1", address = new {
                     addressType = "Main Office", 
                     addressLine1 = "123 Main Street", 
                     location = new {
                        city = "Brooklyn", stateProvinceName = "New York" 
                     }, postalCode = "11229", countryRegionName = "United States"
                  }, 
               };
               //Family fam = new Family();
	
                // await client.DeleteDatabaseAsync(UriFactory.CreateDocumentCollectionUri(DB, ID));
               //await client.ReadDocumentAsync(UriFactory.CreateDocumentUri(DB, Coll, fam.Id));
               await client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(DB,Coll),fam);
                Console.WriteLine("Creado Documento :" + DB);
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
    #region Clases

       // ADD THIS PART TO YOUR CODE
       public class Family
       {
           [JsonProperty(PropertyName = "id")]
           public string Id { get; set; }
           public string LastName { get; set; }
           public Parent[] Parents { get; set; }
           public Child[] Children { get; set; }
           public Address Address { get; set; }
           public bool IsRegistered { get; set; }
           public override string ToString()
           {
               return JsonConvert.SerializeObject(this);
           }
       }




       public class Parent
       {
           public string FamilyName { get; set; }
           public string FirstName { get; set; }
       }

       public class Child
       {
           public string FamilyName { get; set; }
           public string FirstName { get; set; }
           public string Gender { get; set; }
           public int Grade { get; set; }
           public Pet[] Pets { get; set; }
       }

       public class Pet
       {
           public string GivenName { get; set; }
       }

       public class Address
       {
           public string State { get; set; }
           public string County { get; set; }
           public string City { get; set; }
       }
   }
    #endregion

