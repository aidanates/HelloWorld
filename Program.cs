using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace HelloWorld
{

    public class Program : HelloWorld
    {

        static void Main(string[] args)
        {
            Program pr = new Program();
            pr.Write("Hello World");
            Console.ReadLine();
        }
    }

    public class HelloWorld
    {
        public void Write(string message)
        {
            //Write to Database
            bool Write2Database = Convert.ToBoolean(ConfigurationManager.AppSettings["Write2Database"].ToString());
            if (Write2Database)
            {
                var connectionString = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
                using (var connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "insert into Console(text) values(@text);";
                    command.Parameters.AddWithValue("@text", message);
                    try
                    {
                        connection.Open();
                        int numOfRecords = command.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }

                //Write To File
                bool Write2File = Convert.ToBoolean(ConfigurationManager.AppSettings["Write2File"].ToString());
                if (Write2File)
                {
                    try
                    {
                        StreamWriter sw = new StreamWriter("C:\\HelloWorld.txt",true);
                        sw.WriteLine(message);
                        sw.Close();
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine("Write to File Error: {0}", ex.Message);
                    }
                }

                //Write to Console
                bool Write2Console = Convert.ToBoolean(ConfigurationManager.AppSettings["Write2Console"].ToString());
                if(Write2Console)
                {
                    try
                    {
                        Console.WriteLine(message);
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine("Write to Console Error: {0}", ex.Message);
                    }
                }
            }


        }
    }
}
