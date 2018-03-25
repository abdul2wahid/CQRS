using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using CQRS.Bus;
using CQRS.Commands;
using CQRS.Exceptions;
using Honeywell.RabbitMQ;
using Newtonsoft.Json;


namespace CQRS.Repository
{
    public class SqlStore : IStore
    {
        readonly CommandBus _bus;
        const string ConnectionStringName = "SqlEventStore";
        const string connectionString = "Server=IE3BLT7V2PRC2;Database=NOTIFICATOIN;Integrated Security=SSPI";
        public SqlStore(CommandBus bus)
        {
            _bus = bus;
        }

        public void StoreEvents(ICommand createUserCmd,object streamId, IEnumerable<object> events)
        {
         
                if (createUserCmd is CreateUserCommand)
                {
                    StoreUsers(createUserCmd);
                }
                else if (createUserCmd is UpdateUserCommand)
                {

                }
            string str= JsonConvert.SerializeObject(events.First<object>());
            byte[] message = Encoding.UTF8.GetBytes(str);

            RabbitMQConnectionFactory rmqConn = RabbitMQConnectionFactory.RabbitMQConnectionFactoryInstance;
            rmqConn.CreateDirectExchange("Events", false, true);
            rmqConn.PublishMessage("Events", "UserCreatedEvent", false, null, message, 0);
           // _bus.Publish(events);
        }

        public void StoreUsers(ICommand cmd)
        {
           
            CreateUserCommand userCmd = (CreateUserCommand)cmd;
            using (var con = new SqlConnection(connectionString))
            {
                con.Open();
                const string insertText =
                   "Insert into Users(UserId, Name,Email) values (@UserId, @Name,@Email);";
                using (var command = new SqlCommand(insertText, con))
                {
                    command.Parameters.AddWithValue("UserId", userCmd.UserId);
                    command.Parameters.AddWithValue("Name", userCmd.Name);
                    command.Parameters.AddWithValue("Email", userCmd.Email);

                    command.ExecuteNonQuery();
                }

                const string insertCreatedBy =
                   "Insert into UsersCreatedBy(UserId, CreatedBy) values (@UserId, @CreatedBy);";
                using (var command = new SqlCommand(insertCreatedBy, con))
                {
                    command.Parameters.AddWithValue("UserId", userCmd.UserId);
                    command.Parameters.AddWithValue("CreatedBy", userCmd.CreatedBy);

                    command.ExecuteNonQuery();
                }

            
            }
        }


        public IEnumerable<object> LoadEvents(object id, long version = 0)
        {
            const string cmdText = "SELECT EventType, BODY from EventWrappers WHERE StreamId = @StreamId AND Sequence >= @Sequence ORDER BY TimeStamp";
            using (var con = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(cmdText, con))
            {
                cmd.Parameters.AddWithValue("StreamId", id.ToString());
                cmd.Parameters.AddWithValue("Sequence", version);

                cmd.Connection.Open();

                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var eventTypeString = reader["EventType"].ToString();
                    var eventType = Type.GetType(eventTypeString);
                    var serializedBody = reader["Body"].ToString();
                    yield return JsonConvert.DeserializeObject(serializedBody, eventType);
                }
            }
        }



    }
}