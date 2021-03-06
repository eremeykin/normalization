﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace normalization
{
    class TrigramDictionary
    {
        Dictionary<String, List<DataBaseWord>> dict = new Dictionary<String, List<DataBaseWord>>();

        private static TrigramDictionary dictionary;

        private TrigramDictionary()
        {
        }
        /// <summary>
        /// Возвращает единственную сущность типа TrigramDictionary
        /// (паттерн Singleton http://www.cyberforum.ru/csharp-beginners/thread335171.html)
        /// </summary>
        public static TrigramDictionary getInstance()
        {
            // для исключения возможности создания двух объектов 
            // при многопоточном приложении
            if (dictionary == null)
            {
                lock (typeof(TrigramDictionary))
                {
                    if (dictionary == null)
                        dictionary = new TrigramDictionary();
                }
            }

            return dictionary;
        }

        /// <summary>
        /// Добавляет в словарь слово по триграмму
        /// </summary>
        /// <param name="trigram"> триграмм</param>
        /// <param name="word">слово</param>
        public void add(String trigram, DataBaseWord word)
        {
            if (!word.ToString().Contains(trigram))
            {
                throw new ArgumentException(word + " does not contain trigram " + trigram);
            }

            if (this.dict.ContainsKey(trigram))
            {
                this.dict[trigram].Add(word);
                //if (!this.dict[trigram].Contains(word))
                //    this.dict[trigram].Add(word);
            }
            else
            {
                List<DataBaseWord> wordList = new List<DataBaseWord>();
                //StringCollection strColl = new StringCollection();
                wordList.Add(word);
                //strColl.Add(word);
                this.dict.Add(trigram, wordList);
            }
        }

        /// <summary>
        /// Возвращает список слов базы данных для указанной триграммы
        /// </summary>
        /// <returns></returns>
        public List<DataBaseWord> getDBWordsByTrigram(String trigram)
        {
            return dict[trigram];
        }

        /// <summary>
        /// Показывает содержит ли словарь данную триграмму
        /// </summary>
        /// <param name="trigram"></param>
        /// <returns></returns>
        public bool containsTrigram(String trigram)
        {
            return dict.ContainsKey(trigram);
        }
        /// <summary>
        /// ДЛЯ ОТЛАДКИ печатает словарь в консоль
        /// </summary>
        public void printDictionary()
        {
            foreach (String trigram in this.dict.Keys)
            {
                Console.WriteLine("_____________" + trigram + "_____________");
                for (int i = 0; i < this.dict[trigram].Count; i++)
                {
                    Console.Write(this.dict[trigram][i] + " #"+this.dict[trigram][i].getDataBaseIndex());
                    Console.WriteLine();
                }
                Console.WriteLine("");
            }
        }


    }
}
