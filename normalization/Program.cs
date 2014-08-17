using System;
using System.Data.SqlClient;
using System.Data;
using System.Threading;
using System.Collections;

namespace normalization
{
    class Program
    {
        static void Main(string[] args)
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
                TrigramDictionary TD = TrigramDictionary.getInstance();
                while (dr.Read())
                {
                    ExtendedString str = new ExtendedString(dr.GetValue(0).ToString().Trim());
                    str.removeSpecChars();
                    foreach (Word word in str.getWords())
                    {
                        String[] trigram = word.getTrigrams();
                        for (int j = 0; j < trigram.Length; j++)
                        {
                            TD.add(trigram[j], word.ToString());
                        }
                    }
                }
                TD.printDictionary();
            }
            //закрыть соединение
            conn.Close();
            conn.Dispose();
            Chars.Sort();
            Console.ReadKey();
        }
    }
}
