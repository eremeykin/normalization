using System;
using System.Data.SqlClient;
using System.Data;
using System.Threading;
using System.Collections;

namespace normalization
{
    class Statistic
    {
        public static void getChars()
        {
            
            // Источник: http://www.cyberforum.ru/ado-net/thread182279.html
            string connStr = @"Data Source=(local)\SQLEXPRESS; 
                            Initial Catalog=Мосгортранс;
                            Integrated Security=True";//Имя сервера, имя БД, параметры безопасности
            SqlConnection conn = new SqlConnection(connStr);
            try
            {
                conn.Open();
            }
            catch (SqlException se)
            {
                Console.WriteLine("Ошибка подключения:{0}", se.Message);
                return;
            }
            Console.WriteLine("Соедение успешно произведено");
            ArrayList Chars = new ArrayList();
            SqlCommand cmd = new SqlCommand("SELECT [Наименование] FROM [Мосгортранс].[dbo].[МосгортрансБД]", conn);
            using (SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
            {
                for (int i = 0; i < dr.FieldCount; i++)
                    Console.Write("{0}\t", dr.GetName(i).ToString().Trim());
                while (dr.Read())
                {
                    String str = dr.GetValue(0).ToString().Trim();
                    for (int i = 0; i < str.Length; i++)
                    {
                        char ch = str[i];
                        if (!Chars.Contains(ch))
                            Chars.Add(ch);
                    }
                }
            }
            //закрыть соединение
            conn.Close();
            conn.Dispose();
            Chars.Sort();
            for (int i = 0; i < Chars.Count; i++)
                Console.WriteLine(Chars[i]);
            Console.ReadKey();
        
        }
        public static void getWordAVGCount()
        {
            // Источник: http://www.cyberforum.ru/ado-net/thread182279.html
            string connStr = @"Data Source=(local)\SQLEXPRESS; 
                            Initial Catalog=Мосгортранс;
                            Integrated Security=True";//Имя сервера, имя БД, параметры безопасности
            SqlConnection conn = new SqlConnection(connStr);
            try
            {
                conn.Open();
            }
            catch (SqlException se)
            {
                Console.WriteLine("Ошибка подключения:{0}", se.Message);
                return;
            }
            Console.WriteLine("Соедение успешно произведено");
            ArrayList Chars = new ArrayList();
            SqlCommand cmd = new SqlCommand("SELECT [Наименование] FROM [Мосгортранс].[dbo].[МосгортрансБД] WHERE  [Наименование] LIKE '%.%'", conn);
            using (SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
            {
                for (int i = 0; i < dr.FieldCount; i++)
                    Console.Write("{0}\t", dr.GetName(i).ToString().Trim());
                float avgWordCount = 0;
                int count = 0;
                while (dr.Read())
                {
                    //String str = dr.GetValue(0).ToString().Trim();
                    ExtendedString str = new ExtendedString(dr.GetValue(0).ToString().Trim());
                    str.removeSpecChars();
                    Console.WriteLine(str + " " + str.getWordCount());
                    avgWordCount += str.getWordCount();
                    count++;
                }
                avgWordCount = avgWordCount / count;
                Console.WriteLine(avgWordCount);
            }
            //закрыть соединение
            conn.Close();
            conn.Dispose();
            Chars.Sort();
            for (int i = 0; i < Chars.Count; i++)
                Console.WriteLine(Chars[i]);
            Console.ReadKey();
        }
    }
}
