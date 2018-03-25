using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using NOSQL_Read_Project.Events;
using NOSQL_Read_Project.MongoConn;

namespace NOSQL_Read_Project.ReadModel
{
    public class ReadModelFacade
    {

        public List<UsersReadModel> UserInfoList { get;  set; }
        private IMongoCollection<UsersReadModel> _usersCollection;

        public ReadModelFacade(UsersEventHandler info)
        {
            MongoConnection mObj = MongoConnection.MongoConnectionInstance;
            IMongoDatabase db = mObj.GetDataBase();
            _usersCollection = db.GetCollection<UsersReadModel>("Users");
            UserInfoList = _usersCollection.Find(new BsonDocument()).ToList();
        }

        public List<UsersReadModel> GetUsers()
        {
          if(_usersCollection!=null)
                UserInfoList = _usersCollection.Find(new BsonDocument()).ToList();

            return UserInfoList;
        }


    }

  
}