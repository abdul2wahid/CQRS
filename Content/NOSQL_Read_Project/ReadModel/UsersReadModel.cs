using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace NOSQL_Read_Project.ReadModel
{
    public class UsersReadModel
    {
      

        [BsonId]
        public Guid Id { get; set; }
        public BsonString Name { get; set; }
        public BsonString Email { get; set; }

        public UsersReadModel(Guid userId, BsonString name, BsonString email)
        {
            Id = userId;
            Name = name;
            Email = email;


        }



    }
}