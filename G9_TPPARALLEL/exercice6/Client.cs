using System;
using System.Collections.Generic;
using Devart.Data.MySql;

namespace exercice6
{
    public class Client
    {
        public int Age;
        public String Nom;
        
        public static List<Client> CreerClients(int nombreClient)
        {
            List<Client> x = new List<Client>();
            try
            {
                Random alea = new Random();
                MySqlConnection cn = new MySqlConnection("SERVER=localhost;PORT=3306;DATABASE=DIC3Clients;UID=root;");
                if (cn.State == System.Data.ConnectionState.Closed)
                {
                    cn.Open();
                   
                }
                for (int i = 0; i < nombreClient; i++)
                {
                    MySqlCommand cmd = new MySqlCommand("insert into Clients values(@age, @nom)", cn);
                    int age = (Convert.ToInt32(alea.NextDouble() * 100));
                    cmd.Parameters.AddWithValue("@age", age);
                    cmd.Parameters.AddWithValue("@nom", "nom" + age);
                    cmd.ExecuteNonQuery();
                   
                }
                MySqlCommand cm = new MySqlCommand("select * from Clients", cn);
                cm.ExecuteNonQuery();
                MySqlDataReader reader = cm.ExecuteReader();
                while (reader.Read())
                {
                    Client c = new Client();
                    c.Age = Convert.ToInt32(reader["age"]);
                    c.Nom = Convert.ToString(reader["nom"]);
                    x.Add(c);
                }

                cn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur d'etablissement de la connexion");
            }
            return x;
        }
    }
}
