
using ConsoleApplication1;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using static System.Net.Mime.MediaTypeNames;

class MyTcpListener
{
    public static void Main()
    {
        TcpListener server = null;
        string IpProvenienza = null;
        DateTime TimeStamp;
        Uri uri;

        try
        {

            Utility.WelcomeMesg();

            Int32 port = 8000;

            IPAddress localAddr =  IPAddress.Parse("192.168.1.6"/* Utility.GetLocalIPAddress()*/); //SERVER IP

            
           // TcpListener server = new TcpListener(port);
            server = new TcpListener(localAddr, port);

            // Start listening for client requests.
            server.Start();

            using (StreamWriter w = File.AppendText("log.txt"))
            {
                Utility.Log("Inizializzato\t" + DateTime.Now.ToString(), w);

            }

            // Buffer for reading data
            Byte[] bytes = new Byte[256];
            String data = null;




            Console.WriteLine("Inizializzato pronto per le connessioni in ingresso.\n\n ");
            
            while (true)
            {

                // Perform a blocking call to accept requests.
                // You could also user server.AcceptSocket() here.
                //server.AcceptSocket();
                TcpClient client = server.AcceptTcpClient();
                Console.WriteLine("\n\nNUOVA CONNESSIONE IN INGRESSO----------------------------------------");
                Console.WriteLine("\n");
                data = null;
                using (StreamWriter w = File.AppendText("log.txt"))
                {
                    Utility.Log("Nuova connessione IN\t" + DateTime.Now.ToString(), w);

                }
                // Get a stream object for reading and writing
                NetworkStream stream = client.GetStream();

                int i;

                // Loop to receive all the data sent by the client.
                while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    // Translate data bytes to a ASCII string.
                    //cambiato in codifica utilizzata dai reader
                    data = Utility.ConvHexDec(System.Text.Encoding.ASCII.GetString(bytes, 0, i));
                    //data = Convert.ToInt32(System.Text.Encoding.ASCII.GetString(bytes, 0, i), 16).ToString();

                    IpProvenienza = client.Client.RemoteEndPoint.ToString();
                    TimeStamp = DateTime.Now;
                    uri = new Uri("http://" + IpProvenienza);
                    Console.WriteLine("Dato letto: {0} | Inviato da {1} | LogTime: {2}", data, uri.Host.ToString(), TimeStamp.ToString());
                    using (StreamWriter w = File.AppendText("log.txt"))
                    {
                        Utility.Log("Dato letto: "+ data +"\t" + DateTime.Now.ToString(), w);

                    }
                    new Thread(() =>
                    {
                        Thread.CurrentThread.IsBackground = true;

                        if (Utility.CercaDettagliStaff(data) || Utility.CercaDettagliDocente(data) || Utility.CercaDettagliStudente(data))
                        {
                            Utility.inviaComando("Ack", uri.Host.ToString());
                            using (StreamWriter w = File.AppendText("log.txt"))
                            {
                                Utility.Log("GRANTED\t" + DateTime.Now.ToString(), w);
                                Utility.InserisciAccesso(data, Convert.ToInt32(uri.Host.ToString().Substring(11, 2)));
                            }
                        }
                        else
                        {
                            //Utility.inviaComando("NAck", uri.Host.ToString());
                            using (StreamWriter w = File.AppendText("log.txt"))
                            {
                                Utility.Log("NOT GRANTED\t" + DateTime.Now.ToString(), w);

                            }
                        }

                    }).Start();

                }

                // Shutdown and end connection
                client.Close();
 
            }
            
        }
        catch (SocketException e)
        {
            Console.WriteLine("Eccezione nel Socket: {0}", e.Message);
        }
        finally
        {
            // Stop listening for new clients.
            server.Stop();
        }

        string myApp = System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase;
        System.Diagnostics.Process.Start(myApp);
        Environment.Exit(0);
  
    }


}
