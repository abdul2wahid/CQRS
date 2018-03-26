using Events_Project.Events;

using NOSQL_Read_Project.Bus;
using NOSQL_Read_Project.Events;
using NOSQL_Read_Project.ReadModel;

using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using RabbitMQFactory;
using RabbitMQ.Client.Events;

namespace NOSQL_Read_Project.Configurations
{
    public class Configuration_Read
    {

        private readonly EventBus _ebus;
        private readonly ReadModelFacade _readModel;

        public EventBus eventBus { get { return _ebus; } }
        public ReadModelFacade ReadModel { get { return _readModel; } }


        private static readonly Configuration_Read Config = new Configuration_Read();
        public static Configuration_Read Instance()
        {
            return Config;
        }

       

        private Configuration_Read()
        {
            _ebus = new EventBus();
            //REgistering my Events to corresponding EventHandlers
            var infoProjection = new UsersEventHandler();
            _ebus.RegisterHandler<UserCreatedEvent>(infoProjection.Handle);
            _ebus.RegisterHandler<UserUpdatedEvent>(infoProjection.Handle);

            _readModel = new ReadModelFacade(infoProjection);
            RabbitMQSubscription();
        }

        private void RabbitMQSubscription()
        {
            RabbitMQConnectionFactory rmqConn = RabbitMQConnectionFactory.RabbitMQConnectionFactoryInstance;
            rmqConn.CreateDirectExchange("Events", false, true);

            rmqConn.CreateQueue("EventsQueue", false, true, null);
            rmqConn.BindQueue("EventsQueue", "Events", "UserCreatedEvent");
            try
            {
                var consumer = rmqConn.ConsumerEventHandlers();
                consumer.Received += Consumer_Received;
                rmqConn.Consume("EventsQueue", true, consumer);
            }
            catch (Exception ex)
            {

            }
        }

        private void Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            try
            {
                
                var body = e.Body;
                var content = Encoding.UTF8.GetString(body);
                var myevent=(UserCreatedEvent)JsonConvert.DeserializeObject<UserCreatedEvent>(content);
                _ebus.PublishEvent(myevent);
            }
            catch (Exception ex)
            {


                //using (StreamWriter sw = new StreamWriter(@"C:\wahid off\testExce.txt", true))
                //{
                //    sw.WriteLine(ex.ToString());
                //}
            }
        }
    }
}
