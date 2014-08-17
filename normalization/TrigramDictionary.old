using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace normalization
{
    class TrigramDictionary
    {
        Dictionary<String, StringCollection> dict = new Dictionary<String, StringCollection>();

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
        public void add(String trigram, String word)
        {
            if (!word.Contains(trigram))
            {
                throw new ArgumentException(word + " does not contain trigram " + trigram);
            }
            if (this.dict.ContainsKey(trigram))
            {
                if (!this.dict[trigram].Contains(word))
                    this.dict[trigram].Add(word);
            }
            else
            {
                StringCollection strColl = new StringCollection();
                strColl.Add(word);
                this.dict.Add(trigram, strColl);
            }
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
                    Console.Write(this.dict[trigram][i] + " ");
                    if (i % 6 == 0)
                        Console.WriteLine();
                }
                Console.WriteLine("");
            }
        }


    }
}
