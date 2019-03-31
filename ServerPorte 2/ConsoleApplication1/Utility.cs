using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Utility
    {

        public static string  ConvHexDec(string datoLetto)
        {
            //FUNZIONE SCRITTA CON IL MIO AMORE EVELYNE PER LA CONVERSIONE DELLE CARD
            string h2 = datoLetto.Substring(4, 2);
            string h4 = datoLetto.Substring(2, 2) + datoLetto.Substring(0, 2);
            string h2Dec = Convert.ToInt32(h2, 16).ToString();
            string h4Dec = Convert.ToInt32(h4, 16).ToString();
            return h2Dec.PadLeft(3, '0') + h4Dec.PadLeft(5, '0');
        }

        internal static void inviaComando(string comando, string Indirizzo)
        {
            //CREA LA CONNESSIONE
            Socket soc = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            System.Net.IPAddress ipAdd = System.Net.IPAddress.Parse(Indirizzo);
            System.Net.IPEndPoint remoteEP = new IPEndPoint(ipAdd, 8000);
            soc.Connect(remoteEP);

            //INVIA.
            byte[] byData = System.Text.Encoding.ASCII.GetBytes(comando);
            soc.Send(byData);
            //DISPOSE
            soc.Close();
            soc.Dispose();
        }

        internal static bool CercaDettagliStudente(string data)
        {

            SqlConnection sqlConnection1 = new SqlConnection("Data Source=my.sienajazz.it;Initial Catalog=SCUOLADB;Persist Security Info=True;User ID=sa;Password=R1cc4rd0");
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            cmd.CommandText = "SELECT * FROM Anagrafica where Card = @Card";
            cmd.Parameters.AddWithValue("@Card", data);
            cmd.CommandType = CommandType.Text;
            cmd.Connection = sqlConnection1;

            sqlConnection1.Open();

            reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Console.WriteLine("TROVATO");
                    // Process the data sent by the client.

                    Console.WriteLine("Nome Studente: " + reader["Cognome"].ToString() + " " + reader["Nome"].ToString());

                   

                    return true;

                }
                sqlConnection1.Close();

                return true;
            }
            else
            {
                Console.WriteLine("NON TROVATO");
                //inviaComando("bb", IPAddress.Parse(((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString()).ToString());

                return false;
            }

        }

        internal static void InserisciAccesso(string data, int aula)
        {
            SqlConnection sqlConnection1 = new SqlConnection("Data Source=my.sienajazz.it;Initial Catalog=SCUOLADB;Persist Security Info=True;User ID=sa;Password=R1cc4rd0");
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "INSERT INTO Accessi (Nominativo, Aula) VALUES(@Nominativo, @Aula)";
            cmd.Parameters.AddWithValue("@Nominativo", data);
            cmd.Parameters.AddWithValue("@Aula", aula);

            cmd.CommandType = CommandType.Text;
            cmd.Connection = sqlConnection1;

            sqlConnection1.Open();

             cmd.ExecuteNonQuery();

            sqlConnection1.Close();
        }

        internal static bool CercaDettagliStaff(string data)
        {

            SqlConnection sqlConnection1 = new SqlConnection("Data Source=my.sienajazz.it;Initial Catalog=SCUOLADB;Persist Security Info=True;User ID=sa;Password=R1cc4rd0");
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            cmd.CommandText = "SELECT * FROM Staff where Card = @Card";
            cmd.Parameters.AddWithValue("@Card", data);
            cmd.CommandType = CommandType.Text;
            cmd.Connection = sqlConnection1;

            sqlConnection1.Open();

            reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Console.WriteLine("TROVATO");
                    // Process the data sent by the client.

                    Console.WriteLine("Nome Staff: " + reader["Cognome"].ToString() + " " + reader["Nome"].ToString());



                    return true;

                }
                sqlConnection1.Close();

                return true;
            }
            else
            {
                Console.WriteLine("NON TROVATO");
                //inviaComando("bb", IPAddress.Parse(((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString()).ToString());

                return false;
            }

        }

        internal static bool CercaDettagliDocente(string data)
        {

            SqlConnection sqlConnection1 = new SqlConnection("Data Source=my.sienajazz.it;Initial Catalog=SCUOLADB;Persist Security Info=True;User ID=sa;Password=R1cc4rd0");
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            cmd.CommandText = "SELECT * FROM Docenti where Card = @Card";
            cmd.Parameters.AddWithValue("@Card", data);
            cmd.CommandType = CommandType.Text;
            cmd.Connection = sqlConnection1;

            sqlConnection1.Open();

            reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Console.WriteLine("TROVATO");
                    // Process the data sent by the client.

                    Console.WriteLine("Nome Docente: " + reader["Cognome"].ToString() + " " + reader["Nome"].ToString());



                    return true;

                }
                sqlConnection1.Close();

                return true;
            }
            else
            {
                Console.WriteLine("NON TROVATO");
                //inviaComando("bb", IPAddress.Parse(((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString()).ToString());

                return false;
            }

        }

        internal static void WelcomeMesg()
        {
            Console.WriteLine(@"   _____         _         _  __                                       ");
            Console.WriteLine(@"  / ____|       | |       | |/ /                                       ");
            Console.WriteLine(@" | |  __   __ _ | |_  ___ | ' /  ___   ___  _ __    ___  _ __          ");
            Console.WriteLine(@" | | |_ | / _` || __|/ _ \|  <  / _ \ / _ \| '_ \  / _ \| '__|         ");
            Console.WriteLine(@" | |__| || (_| || |_|  __/| . \|  __/|  __/| |_) ||  __/| |            ");
            Console.WriteLine(@"  \_____| \__,_| \__|\___||_|\_\\___| \___|| .__/ _\___||_|     _    _ ");
            Console.WriteLine(@" |  __ \ (_)                          | |  | |   |  _ \        | |  (_)");
            Console.WriteLine(@" | |__) | _   ___  ___  __ _  _ __  __| |  |_|   | |_) | _   _ | |_  _ ");
            Console.WriteLine(@" |  _  / | | / __|/ __|/ _` || '__|/ _` | / _ \  |  _ < | | | || __|| |");
            Console.WriteLine(@" | | \ \ | || (__| (__| (_| || |  | (_| || (_) | | |_) || |_| || |_ | |");
            Console.WriteLine(@" |_|  \_\|_| \___|\___|\__,_||_|   \__,_| \___/  |____/  \__,_| \__||_|");
        }

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    Console.WriteLine("SERVER START AT: " + ip.ToString());
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        public static void Log(string logMessage, TextWriter w)
        {
            w.Write("\r\nRegistro di Log : ");
            w.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
                DateTime.Now.ToLongDateString());
            w.WriteLine("  :");
            w.WriteLine("  :{0}", logMessage);
            w.WriteLine("-------------------------------");
        }
    }
}
