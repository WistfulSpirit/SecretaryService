using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SecretaryService.Models
{
    public class PersonRepository
    {
        private SqlConnection connection;

        public PersonRepository()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            connection = new SqlConnection(connectionString);
        }

        public List<Person> GetAllPersons()
        {
            List<Person> persons = new List<Person>();
            using (SqlCommand command = new SqlCommand("SELECT * FROM Person", connection))
            {
                connection.Open();
                var reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Person person = new Person();
                        person.Id = (int)reader["Id"];
                        person.Name = (string)reader["Name"];
                        person.Email = (string)reader["Email"];
                        persons.Add(person);
                    }
                }
                connection.Close();
            }
            return persons;
        }


        /// <summary>
        /// Returns person by given id or null if no person is found
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Person GetPerson(int id)
        {
            Person person = null;
            using (SqlCommand command = new SqlCommand("SELECT * FROM Person WHERE Id=" + id, connection))
            {
                connection.Open();
                var reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    person = new Person();
                    person.Id = (int)reader["Id"];
                    person.Name = (string)reader["Name"];
                    person.Email = (string)reader["Email"];
                }
                connection.Close();
            }
            return person;
        }

        /// <summary>Inserts person into db and returns id of inserted row
        /// <para>Returns -1 if insert isn't successful</para>
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public int InsertPerson(Person person)
        {
            int id = -1;
            string cmd = "INSERT INTO Person (Name, Email) OUTPUT INSERTED.ID VALUES (@Name, @Email)";
            using (SqlCommand command = new SqlCommand(cmd, connection))
            {
                command.Parameters.AddWithValue("@Name", person.Name);
                command.Parameters.AddWithValue("@Email", person.Email);
                connection.Open();
                id = (int)command.ExecuteScalar();
                connection.Close();
            }
            return id;
        }

        public int UpdatePerson(Person person)
        {
            int id = -1;
            string cmd = "UPDATE Person SET Name=@Name, Email=@Email WHERE Id =@Id";
            using (SqlCommand command = new SqlCommand(cmd, connection))
            {
                command.Parameters.AddWithValue("@Name", person.Name);
                command.Parameters.AddWithValue("@Email", person.Email);
                command.Parameters.AddWithValue("@Id", person.Id);
                connection.Open();
                id = command.ExecuteNonQuery();
                connection.Close();
            }
            return id;
        }


        public bool IsSafeToDelete(int id)
        {
            bool answer = true;
            using (SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM Message WHERE Adressee_id=@pId OR Sender_id=@pId", connection))
            {
                connection.Open();
                command.Parameters.AddWithValue("@pId", id);
                var reader = (int)command.ExecuteScalar();
                if (reader > 0)
                {
                    answer = false;
                }
                connection.Close();
            }
            return answer;
        }

        public void DeletePerson(int id)
        {
            string cmd = "DELETE FROM Person WHERE Id=@pId;" +
     "DELETE FROM Message WHERE Adressee_id=@pId or Sender_id=@pId";
            SqlCommand command = new SqlCommand(cmd, connection);
            command.Parameters.AddWithValue("@pId", id);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
}