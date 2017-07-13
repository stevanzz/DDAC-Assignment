namespace Events
{
    using Events.Models;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Linq;
    using System.Data.SqlClient;
    using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
    using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling.Data;


    public class EventsRepository : IEventsRepository
    {
        private const string EventsQuery = "SELECT {0} Id, Title, [Description], DepartureLocation, ArrivalLocation, ContainerId, StartDate, [Days] FROM [Shipment]";

        private const string EventsInsert = @"INSERT INTO [Shipment] (Title, Description, DepartureLocation, ArrivalLocation, ContainerID, StartDate, Days) 
                                            VALUES (@Title, @Description, @DepartureLocation, @ArrivalLocation, @ContainerID, @StartDate, @Days);
                                              SELECT @@IDENTITY";

        private const string EventsUpdate = @"UPDATE [Shipment] SET Title = @Title, [Description] = @Description, [DepartureLocation] = @DepartureLocation, 
                                            [ArrivalLocation] = @ArrivalLocation, [ContainerID] = @ContainerID, [StartDate] = @StartDate, [Days] = @Days 
                                               WHERE [Id] = @Id";

        private const string EventsDelete = @"DELETE FROM [Shipment] WHERE [Id] = @Id";

        private const string RegistrationQuery = @"SELECT Id, Title, [Description], Location, StartDate, [Days], AudienceId, OwnerId
                                                   FROM [Event]
                                                   INNER JOIN [Registration] ON [Event].Id = [Registration].EventId
                                                   WHERE [Registration].UserId = @UserId";

        private const string RegistrationInsert = @"INSERT INTO Registration (UserId, EventId, RegistrationDate) VALUES (@UserId, @EventId, @RegistrationDate)";


        static EventsRepository()
        {

        }

        public EventsRepository(string userName)
        {
        }

        public Event GetEvent(int eventId)
        {
            using (var cmd = this.CreateCommand(
                string.Format(EventsQuery, string.Empty) + "WHERE Id = @EventId",
                new Dictionary<string, object>() { { "@EventId", eventId } }))
            {
                return this.EventsFromDBQuery(cmd.ExecuteReader()).FirstOrDefault();
            }
        }

        public IEnumerable<Event> UpcomingEvents(int count)
        {
            using (var cmd = this.CreateCommand(
                string.Format(EventsQuery, count > 0 ? "TOP " + count : string.Empty) + "WHERE StartDate > GETDATE() ORDER BY StartDate",
                null))
            {
                return this.EventsFromDBQuery(cmd.ExecuteReader());
            }
        }

        public IEnumerable<Event> GetUserEvents(string activeDirectoryId)
        {
            using (var cmd = this.CreateCommand(
                RegistrationQuery,
                new Dictionary<string, object>() { { "@UserId", activeDirectoryId } }))
            {
                var reader = cmd.ExecuteReader();
                return this.EventsFromDBQuery(reader);
            }
        }
        public Event CreateEvent(Event @event)
        {
            using (var cmd = this.CreateCommand(
                EventsInsert,
                new Dictionary<string, object>() {
                { "@Title", @event.Title },
                { "@Description", @event.Description },
                { "@DepartureLocation", @event.DepartureLocation },
                { "@ArrivalLocation", @event.ArrivalLocation },
                { "@ContainerID", (int)@event.ContainerType },
                { "@StartDate", @event.StartDate },
                { "@Days", @event.Days }
                }))
            {
                var id = cmd.ExecuteScalar();
                @event.Id = Convert.ToInt32(id);
                return @event;
            }
        }

        public Event UpdateEvent(Event @event)
        {
            if (Convert.ChangeType(@event.Act, typeof(string)).Equals("Update"))
            {
                using (var cmd = this.CreateCommand(
                    EventsUpdate,
                    new Dictionary<string, object>() {
                { "@Title", @event.Title },
                { "@Description", @event.Description },
                { "@DepartureLocation", @event.DepartureLocation },
                { "@ArrivalLocation", @event.ArrivalLocation },
                { "@ContainerID", (int)@event.ContainerType },
                { "@StartDate", @event.StartDate },
                { "@Days", @event.Days },
                { "@Id", @event.Id }
                    }))
                {
                    var id = cmd.ExecuteScalar();
                    return @event;
                }
            }
            else if (Convert.ChangeType(@event.Act,typeof(string)).Equals("Delete"))
            {
                using (var cmd = this.CreateCommand(
                    EventsDelete,
                    new Dictionary<string, object>() { 
                { "@Id", @event.Id }
                    }))
                {
                    var id = cmd.ExecuteScalar();
                    return @event;
                }
            }
            
            return @event;
        }


        public bool RegisterUser(string activeDirectoryId, int eventId)
        {
            using (var cmd = this.CreateCommand(
                RegistrationInsert,
                new Dictionary<string, object>() {
                { "@UserId", activeDirectoryId },
                { "@EventId", eventId },
                { "@RegistrationDate", DateTime.Now }
                }))
            {
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // private SqlConnection GetConnection()
        // {
        //     String conString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        //     SqlConnection sqlConnection = new SqlConnection(conString);
        //     return sqlConnection;
        // }

        private ReliableSqlConnection GetConnection()
        {
            String conString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            ReliableSqlConnection sqlConnection = new ReliableSqlConnection(conString);
            return sqlConnection;
        }


        private IDbCommand CreateCommand(string sqlScript, IDictionary<string, object> @params)
        {
            //SqlConnection connection = GetConnection();
            //var command = new SqlCommand();
            //command.Connection = connection;
            ReliableSqlConnection connection = GetConnection();
            var command = SqlCommandFactory.CreateCommand(connection);
            command.CommandText = sqlScript;
            command.CommandType = CommandType.Text;

            if (@params != null)
            {
                foreach (var param in @params)
                {
                    var dbParam = command.CreateParameter();
                    dbParam.ParameterName = param.Key;
                    dbParam.Value = param.Value;
                    command.Parameters.Add(dbParam);
                }
            }

            command.Connection.Open();

            return command;
        }



        private IEnumerable<Event> EventsFromDBQuery(IDataReader reader)
        {
            var events = new List<Event>();

            while (reader.Read())
            {
                events.Add(new Event()
                {
                    Id = reader.GetInt32(0),
                    Title = reader.GetString(1),
                    Description = reader.GetString(2),
                    DepartureLocation = reader.GetString(3),
                    ArrivalLocation = reader.GetString(4),
                    ContainerType = (ContainerType)reader.GetByte(5),
                    StartDate = reader.GetDateTime(6),
                    Days = reader.GetInt32(7)
                });
            }
            return events;
        }
    }
}