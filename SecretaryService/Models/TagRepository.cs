using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SecretaryService.Models
{
    public class TagRepository
    {
        private SqlConnection connection;

        public TagRepository()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            connection = new SqlConnection(connectionString);
        }

        public List<Tag> GetAllTags()
        {
            List<Tag> tags = new List<Tag>();
            using (SqlCommand command = new SqlCommand("SELECT * FROM Tags", connection))
            {
                connection.Open();
                var reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Tag tag = new Tag();
                        tag.Id = (int)reader["Id"];
                        tag.Value = (string)reader["Value"];
                        tags.Add(tag);
                    }
                }
                connection.Close();
            }
            return tags;
        }

        public Tag GetTag(int id)
        {
            Tag tag = null;
            using (SqlCommand command = new SqlCommand("SELECT * FROM Tags WHERE Id=" + id, connection))
            {
                connection.Open();
                var reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    tag = new Tag();
                    tag.Id = (int)reader["Id"];
                    tag.Value = (string)reader["Value"];
                }
                connection.Close();
            }
            return tag;
        }

        public int InsertTag(Tag tag)
        {
            int id = -1;
            string cmd = "INSERT INTO Tags (Value) OUTPUT INSERTED.ID VALUES (@Value)";
            using (SqlCommand command = new SqlCommand(cmd, connection))
            {
                command.Parameters.AddWithValue("@Value", tag.Value);
                connection.Open();
                id = (int)command.ExecuteScalar();
                connection.Close();
            }
            return id;
        }

        public int UpdateTag(Tag tag)
        {
            int id = -1;
            string cmd = "UPDATE Tags SET Value=@Value WHERE Id =@Id";
            using (SqlCommand command = new SqlCommand(cmd, connection))
            {
                command.Parameters.AddWithValue("@Value", tag.Value);
                command.Parameters.AddWithValue("@Id", tag.Id);
                connection.Open();
                id = command.ExecuteNonQuery();
                connection.Close();
            }
            return id;
        }

        public void DeleteTag(int id)
        {
            string cmd = "DELETE FROM MessageTags WHERE Tags_id=" + id + ";" +
            "DELETE FROM Tags WHERE Id=" + id + ";";
            SqlCommand command = new SqlCommand(cmd, connection);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }

    }
}