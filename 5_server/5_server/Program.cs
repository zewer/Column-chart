using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using MySql.Data.MySqlClient;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace _5_server
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpListener server = null;
            try
            {
                int MaxThreadsCount = Environment.ProcessorCount * 4;
                ThreadPool.SetMaxThreads(MaxThreadsCount, MaxThreadsCount);
                ThreadPool.SetMinThreads(2, 2);

                Int32 port = 9998;
                IPAddress localAddr = IPAddress.Parse("127.0.0.1");
                int counter = 0;
                server = new TcpListener(localAddr, port);

                server.Start();

                while (true)
                {
                    Console.Write("\n\tWaiting for a connection... ");

                   ThreadPool.QueueUserWorkItem(ConnectFunc, server.AcceptTcpClient());
                   counter++;
                   Console.Write("\nConnection №" + counter.ToString() + "!");
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                server.Stop();
            }

            Console.WriteLine("\nHit enter to continue...");
            //Console.Read();
        }
        static void ConnectFunc(object client_obj)
        {

            string[] split;
            byte[] msg;
            DBMySQL db = new DBMySQL();
            Byte[] bytes = new Byte[256];
            String data = null;

            TcpClient client = client_obj as TcpClient;

            data = null;

            NetworkStream stream = client.GetStream();

            //Information info = new Information();

            int i;
            while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
            {
                data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);

                split = data.Split(new Char[] { ' ' });

                switch (split[0])
                {
                    case "HELLO":
                        msg = System.Text.Encoding.ASCII.GetBytes(db.GetAutomatsNumbers().ToString());
                        break;
                    //TRANSACTION_TYPE
                    //TRANSACTION_TIME
                    case "TRANSACTION_TYPE":
                        string s = db.GetTransactionType(split[1], 1).ToString() + " "
                            + db.GetTransactionType(split[1], 2).ToString() + " "
                            + db.GetTransactionType(split[1], 3).ToString();
                        msg = System.Text.Encoding.ASCII.GetBytes("ANSWER_TYPE " + s);
                        break;
                        
                    case "TRANSACTION_TIME":
                        string s1 = "";
                        for (int iter = 0; iter < 24; iter++ )
                        {
                            s1 += " " + db.GetTransactionTime(split[1], iter.ToString());
                        }
                        msg = System.Text.Encoding.ASCII.GetBytes("ANSWER_TIME" + s1);
                        break;                   
                        
                    default:
                        msg = System.Text.Encoding.ASCII.GetBytes("Unknown Command");
                        break;
                }

                stream.Write(msg, 0, msg.Length);
            }
        }
    }
    class DBMySQL
    {
        public MySqlConnection connection;
        public string server;
        public string database;
        public string uid;
        public string password;

        public DBMySQL()
        {
            Initialize();
        }
        public void Initialize()
        {
            server = "localhost";
            database = "simulate";
            uid = "root";
            password = "121212";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

            connection = new MySqlConnection(connectionString);
        }
        public bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 0:
                        Console.Write("Cannot connect to server.  Contact administrator;\n");
                        break;

                    case 1045:
                        Console.Write("Invalid username/password, please try again;\n");
                        break;
                }
                return false;
            }
        }
        public bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                Console.Write("Cannot close connection;\n", ex.Message);
                return false;
            }
        }       

        //SELECT COUNT(DISTINCT `field_1`) FROM `table`
        public int GetAutomatsNumbers()//STAT
        {
            string query = "SELECT COUNT(DISTINCT `id`) FROM result";

            if (this.OpenConnection() == true)
            {

                MySqlCommand cmd = new MySqlCommand(query, connection);
                int i = Convert.ToInt32(cmd.ExecuteScalar());
                this.CloseConnection();
                return i;
            }
            else
                return -1;
        }

        public int GetTransactionType(string id, int num)//STAT
        {
            string query = "SELECT COUNT(`type`) FROM result WHERE `id`=" + id + " AND `type` = " + num.ToString();

            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                int i = Convert.ToInt32(cmd.ExecuteScalar());
                this.CloseConnection();
                return i;
            }
            else
                return -1;
        }
        //mysql> SELECT COUNT(`type`) FROM result WHERE time RLIKE '2015-06-08 12:[0-9][0-9]:[0-9][0-9]';
        public int GetTransactionTime(string id, string HH)//STAT
        {
            //string query = "SELECT COUNT(`type`) FROM result WHERE `id`=" + id + " AND `type` = " + num.ToString();
            if (Convert.ToInt32(HH) >= 0 && Convert.ToInt32(HH) <= 9)
                HH = FixHH(HH);
            string query = "SELECT COUNT(`type`) FROM result WHERE `id`=" 
                + id + " AND time RLIKE '2015-06-08 " + HH + ":[0-9][0-9]:[0-9][0-9]'";
            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                int i = Convert.ToInt32(cmd.ExecuteScalar());
                this.CloseConnection();
                return i;
            }
            else
                return -1;
        }

        private string FixHH(string HH)
        {
            return "0" + HH;
        }
    }
}
