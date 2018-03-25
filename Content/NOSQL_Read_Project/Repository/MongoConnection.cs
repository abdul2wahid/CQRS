
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using NOSQL_Read_Project.ReadModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NOSQL_Read_Project.MongoConn
{
    public class MongoConnection
    {
        private IMongoClient _client;
        private IMongoDatabase _database;
        private IMongoCollection<UsersReadModel> _usersCollection;
        private static  MongoConnection instance=null;
        private  MongoConnection()
        {
            _client = new MongoClient("mongodb://127.0.0.1:27017");
            _database = _client.GetDatabase("Notifcation_DB");
            _usersCollection = _database.GetCollection<UsersReadModel>("Users");
           
        }

        public static MongoConnection MongoConnectionInstance
        {
          
            get {
                if (instance == null)
                {
                    instance = new MongoConnection();
                }
                return instance;


            }
            set
            {
                
            }
            
        }
        public IMongoDatabase  GetDataBase()
        {
             return _client.GetDatabase("Notifcation_DB");
        }

        public void Insertone(UsersReadModel users)
        {
          
            _usersCollection.InsertOne(new UsersReadModel(users.Id,
                 users.Name, users.Email));
        }
    }

   
}