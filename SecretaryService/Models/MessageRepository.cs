using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace SecretaryService.Models
{
    public class MessageRepository
    {
        private SqlConnection connection;

        public MessageRepository()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            connection = new SqlConnection(connectionString);
        }

        public List<Message> GetAllMessages()
        {
            List<Message> messages = new List<Message>();
            using (SqlCommand command = new SqlCommand("GetMessages", connection) { CommandType = CommandType.StoredProcedure })
            {
                connection.Open();
                var reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Message message = new Message();
                        message.Id = (int)reader["Id"];
                        message.Title = (string)reader["Title"];
                        message.Content = (string)reader["Content"];
                        message.Registry_Date = (DateTime)reader["Registry_Date"];
                        message.Sender = new Person() { Id = (int)reader["SenderId"], Name = (string)reader["SenderName"], Email = (string)reader["SenderEmail"] };
                        message.Adressee = new Person() { Id = (int)reader["AdresseId"], Name = (string)reader["AdresseName"], Email = (string)reader["AdresseEmail"] };
                        message.Tags = reader["Tags"] == DBNull.Value ? null : Json.Decode<List<Tag>>((string)reader["Tags"]);
                        messages.Add(message);
                    }
                }
                connection.Close();
            }
            return messages;
        }

        public Message GetMessage(int id)
        {
            Message message = null;
            using (SqlCommand command = new SqlCommand("GetMessageById", connection) { CommandType = CommandType.StoredProcedure })
            {
                command.Parameters.Add(new SqlParameter("@mId", id));
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                var reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    message = new Message();
                    reader.Read();
                    message.Id = (int)reader["Id"];
                    message.Title = (string)reader["Title"];
                    message.Content = (string)reader["Content"];
                    message.Registry_Date = (DateTime)reader["Registry_Date"];
                    message.Sender = new Person() { Id = (int)reader["SenderId"], Name = (string)reader["SenderName"], Email = (string)reader["SenderEmail"] };
                    message.Adressee = new Person() { Id = (int)reader["AdresseId"], Name = (string)reader["AdresseName"], Email = (string)reader["AdresseEmail"] };
                    message.Tags = reader["Tags"] == DBNull.Value ? null : Json.Decode<List<Tag>>((string)reader["Tags"]);
                }
                connection.Close();
            }
            return message;
        }

        public List<Message> GetMessagesInDateInterval(DateTime dateStart, DateTime dateEnd)
        {
            List<Message> messages = new List<Message>();
            using (SqlCommand command = new SqlCommand("GetMessagesInDateInterval", connection) { CommandType = CommandType.StoredProcedure })
            {
                //command.Parameters.Add(new SqlParameter("@dateStart", SqlDbType.DateTime2)).Value=dateStart;
                //command.Parameters.Add(new SqlParameter("@dateEnd", SqlDbType.DateTime2)).Value = dateEnd;
                command.Parameters.Add(new SqlParameter("@dateStart", dateStart));
                command.Parameters.Add(new SqlParameter("@dateEnd", dateEnd));
                connection.Open();
                var reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Message message = new Message();
                        message.Id = (int)reader["Id"];
                        message.Title = (string)reader["Title"];
                        message.Content = (string)reader["Content"];
                        message.Registry_Date = (DateTime)reader["Registry_Date"];
                        message.Sender = new Person() { Id = (int)reader["SenderId"], Name = (string)reader["SenderName"], Email = (string)reader["SenderEmail"] };
                        message.Adressee = new Person() { Id = (int)reader["AdresseId"], Name = (string)reader["AdresseName"], Email = (string)reader["AdresseEmail"] };
                        message.Tags = reader["Tags"] == DBNull.Value ? null : Json.Decode<List<Tag>>((string)reader["Tags"]);
                        messages.Add(message);
                    }
                }
                connection.Close();
            }
            return messages;
        }

        public List<Message> GetMessagesOfPerson(string PersonRole, int id)
        {
            List<Message> messages = new List<Message>();
            using (SqlCommand command = new SqlCommand("GetMessagesOfPerson", connection) { CommandType = CommandType.StoredProcedure })
            {
                command.Parameters.Add(new SqlParameter("@personRole", PersonRole));
                command.Parameters.Add(new SqlParameter("@personId", id.ToString()));
                connection.Open();
                var reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Message message = new Message();
                        message.Id = (int)reader["Id"];
                        message.Title = (string)reader["Title"];
                        message.Content = (string)reader["Content"];
                        message.Registry_Date = (DateTime)reader["Registry_Date"];
                        message.Sender = new Person() { Id = (int)reader["SenderId"], Name = (string)reader["SenderName"], Email = (string)reader["SenderEmail"] };
                        message.Adressee = new Person() { Id = (int)reader["AdresseId"], Name = (string)reader["AdresseName"], Email = (string)reader["AdresseEmail"] };
                        message.Tags = reader["Tags"] == DBNull.Value ? null : Json.Decode<List<Tag>>((string)reader["Tags"]);
                        messages.Add(message);
                    }
                }
                connection.Close();
            }
            return messages;
        }

        public List<Message> GetMessagesByTag(int id)
        {
            List<Message> messages = new List<Message>();
            using (SqlCommand command = new SqlCommand("GetMessagesByTag", connection) { CommandType = CommandType.StoredProcedure })
            {
                command.Parameters.Add(new SqlParameter("@tagId", id));
                connection.Open();
                var reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Message message = new Message();
                        message.Id = (int)reader["Id"];
                        message.Title = (string)reader["Title"];
                        message.Content = (string)reader["Content"];
                        message.Registry_Date = (DateTime)reader["Registry_Date"];
                        message.Sender = new Person() { Id = (int)reader["SenderId"], Name = (string)reader["SenderName"], Email = (string)reader["SenderEmail"] };
                        message.Adressee = new Person() { Id = (int)reader["AdresseId"], Name = (string)reader["AdresseName"], Email = (string)reader["AdresseEmail"] };
                        message.Tags = reader["Tags"] == DBNull.Value ? null : Json.Decode<List<Tag>>((string)reader["Tags"]);
                        messages.Add(message);
                    }
                }
                connection.Close();
            }
            return messages;
        }




        /// <summary>Inserts message into db and returns id of inserted row
        /// <para>Returns -1 if insert isn't successful</para>
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public int InsertMessage(Message message)
        {
            int id = -1;
            string cmd = "INSERT INTO Message (Title, Registry_Date, Adressee_id, Sender_id, Content) OUTPUT INSERTED.ID VALUES (@Title, @Registry_Date, @Adresse_id, @Sender_id, @Content)";
            using (SqlCommand command = new SqlCommand(cmd, connection))
            {
                command.Parameters.AddWithValue("@Title", message.Title);
                command.Parameters.AddWithValue("@Registry_Date", message.Registry_Date);
                command.Parameters.AddWithValue("@Adresse_id", message.Adressee.Id);
                command.Parameters.AddWithValue("@Sender_id", message.Sender.Id);
                command.Parameters.AddWithValue("@Content", message.Content);
                connection.Open();
                id = (int)command.ExecuteScalar();
                connection.Close();
                if (message.Tags != null && message.Tags.Count != 0)
                    AddTags(id, message.Tags);
            }
            return id;
        }

        private void AddTags(int MessageId, IList<Tag> tags)
        {
            List<string> values = new List<string>();
            foreach (var tag in tags)
            {
                values.Add($"({MessageId},{tag.Id})");
            }
            string cmd = "INSERT INTO MessageTags (Message_id, Tags_id) VALUES " + string.Join(",", values);
            using (SqlCommand command = new SqlCommand(cmd, connection))
            {
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public int UpdateMessage(int id, Message message)
        {
            int rRow = -1;
            string cmd = "UPDATE Message SET Title=@Title, Adressee_id=@Adresse_id, Sender_id=@Sender_id, Content=@Content WHERE Id =@Id";
            using (SqlCommand command = new SqlCommand(cmd, connection))
            {
                command.Parameters.AddWithValue("@Title", message.Title);
                command.Parameters.AddWithValue("@Adresse_id", message.Adressee.Id);
                command.Parameters.AddWithValue("@Sender_id", message.Sender.Id);
                command.Parameters.AddWithValue("@Content", message.Content);
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();
                rRow = command.ExecuteNonQuery();
                connection.Close();
                UpdateTags(id, message.Tags);
            }
            return rRow;
        }

        private void UpdateTags(int MessageId, IList<Tag> tags)
        {
            string cmd = "DELETE FROM MessageTags WHERE Message_id=" + MessageId;
            SqlCommand command = new SqlCommand(cmd, connection);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
            if (tags != null && tags.Count != 0)
                AddTags(MessageId, tags);
        }

        public void DeleteMessage(int id)
        {
            UpdateTags(id, null);
            string cmd = "DELETE FROM Message WHERE Id=" + id;
            SqlCommand command = new SqlCommand(cmd, connection);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
            UpdateTags(id, null);
        }
    }
}