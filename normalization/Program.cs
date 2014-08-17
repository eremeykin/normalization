using System;
using System.Data.SqlClient;
using System.Data;
using System.Threading;
using System.Collections;
using System.Collections.Generic;

namespace normalization
{
    class Program
    {
        static void Main(string[] args)
        {
            TrigramDictionary TD = TrigramDictionary.getInstance();

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
            SqlCommand cmd = new SqlCommand("SELECT [Наименование] ,[Номер материала] FROM [Мосгортранс].[dbo].[МосгортрансБД]", conn);
            using (SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
            {
                while (dr.Read())
                {
                    // Считать строку из БД
                    int stringIndex = Convert.ToInt32(dr.GetValue(1).ToString());
                    DataBaseString str = new DataBaseString(dr.GetValue(0).ToString().Trim(), stringIndex);
                    str.removeSpecChars();
                    // Для каждого слова в считанной из БД строки
                    foreach (DataBaseWord word in str.getWords())
                    {
                        // Получить массив триграмм
                        String[] trigram = word.getTrigrams();
                        // Для каждого из триграмм в текущем слове текущей строки
                        for (int j = 0; j < trigram.Length; j++)
                        {
                            // Добавить в словарь триграмму и её слово
                            TD.add(trigram[j], word);
                        }
                    }
                }
                //TD.printDictionary();
            }
            //закрыть соединение
            conn.Close();
            conn.Dispose();
            Chars.Sort();
            Console.WriteLine("Enter your query:");
            ExtendedString query = new ExtendedString(Console.ReadLine());
            //Создаем массив списков для каждого из слов запроса (размерность массива равна числу слов в запросе)
            /////////////List<KeyValuePair<DataBaseWord, int>>[] relatedWords = new List <KeyValuePair<DataBaseWord,int>>[query.getWordCount()];
            Dictionary<int, int>[] relatedRecords = new Dictionary<int, int>[query.getWordCount()];//Содержит записи связанные с текущим словом
            for (int i = 0; i < relatedRecords.Length; i++)
            {
                relatedRecords[i] = new Dictionary<int, int>();
            }

            // Тоесть разбиваем запрос на слова -> слова на триграммы -> по ториграммам ищем связанные с даннвм словом слова
            // и добавляем к списку слов,  связанных с данным 

            //Для каждого слова запроса
            for (int w = 0; w < query.getWordCount(); w++)
            {
                Word currentWord = query.getWords()[w];
                String[] qTrigram = currentWord.getTrigrams();
                // Для каждой триграммы слова 
                for (int i = 0; i < qTrigram.Length; i++)
                {
                    String currentTrigram = qTrigram[i];
                    //
                    if (TD.containsTrigram(currentTrigram))
                    {
                        foreach (DataBaseWord dbWord in TD.getDBWordsByTrigram(currentTrigram))
                        {
                            int currIndex = dbWord.getDataBaseIndex();
                            int currDistance = currentWord.getMetric(dbWord);
                            if (relatedRecords[w].ContainsKey(currIndex))
                            {
                                if (relatedRecords[w][currIndex] > currDistance)
                                {
                                    relatedRecords[w].Remove(currIndex);
                                    relatedRecords[w].Add(currIndex, currDistance);
                                }
                            }
                            else
                            {
                                relatedRecords[w].Add(currIndex, currDistance);
                            }

                            //KeyValuePair<DataBaseWord, int> kv = new KeyValuePair<DataBaseWord, int>(dbWord, currentWord.getMetric(dbWord));
                            //relatedWords[j].Add(kv);
                        }
                    }

                }
            }

            /////////////////////////////////////////////

            List<int> allRelatedRecords = new List<int>();//Содержит все строки связанные с данным запросом
            for (int w = 0; w < relatedRecords.Length; w++)
            {
                foreach (int index in relatedRecords[w].Keys)
                {
                    if (!allRelatedRecords.Contains(index))
                        allRelatedRecords.Add(index);
                }
            }

            /////////////////////////////////////////////

            Dictionary<int, int> finalDictionary = new Dictionary<int, int>();
            foreach (int index in allRelatedRecords)
            {
                bool inAllWordsDictionary=true;
                int sumDist=0;
                for (int w = 0; w < relatedRecords.Length; w++)
                {
                    if (!relatedRecords[w].ContainsKey(index))
                    {
                        inAllWordsDictionary = false;
                        break;
                    }
                    else {
                        sumDist+=relatedRecords[w][index];
                    }
                }
                if (inAllWordsDictionary)
                    finalDictionary.Add(index, sumDist);
            }

            String sqlCMD = "SELECT [Наименование] ,[Номер материала] FROM [Мосгортранс].[dbo].[МосгортрансБД]  WHERE [Номер материала]=0 ";
            for (int i = 0; i < 10; i++)
            {
                foreach (int index in finalDictionary.Keys)
                {
                    if (finalDictionary[index] == i)
                    {
                        Console.WriteLine(index);
                        sqlCMD += " OR [Номер материала]=" + index+" ";
                    }
                }
            }
            Console.ReadKey();


            ////////////////////////////////////////////////
            // Подключаемся чтоб выцепить найденные строки//
            ////////////////////////////////////////////////

            conn = new SqlConnection(connStr);
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
            cmd = new SqlCommand(sqlCMD, conn);
            using (SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
            {
                while (dr.Read())
                {
                    // Считать строку из БД
                    int stringIndex = Convert.ToInt32(dr.GetValue(1).ToString());
                    DataBaseString str = new DataBaseString(dr.GetValue(0).ToString().Trim(), stringIndex);
                    Console.WriteLine(str);
                }
            }
            //закрыть соединение
            conn.Close();
            conn.Dispose();
            Console.ReadKey();
        }
    }
}
