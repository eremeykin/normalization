using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace normalization
{
    class DataBaseString : ExtendedString
    {
        private int dbIndex;
        
        public DataBaseString(String str,int stringCode):base(str)
        {
            base.s = (String)str.Clone();
            this.dbIndex = stringCode;
        }

        /// <summary>
        /// Возвращает все Слова в строке
        /// </summary>
        /// <returns>Слова[]</returns>
        public new List<DataBaseWord> getWords()
        {
            this.removeSpecChars();
            String[] words = s.Split(' ');
            List<DataBaseWord> resultWords = new List<DataBaseWord>();
            for (int i = 0; i < words.Length; i++)
            {
                if (words[i].Length > 2)
                    resultWords.Add(new DataBaseWord(words[i], this.dbIndex));
            }
            return resultWords;
        }

        /// <summary>
        /// Возвращает первичный ключ под которым строка хранится в базе данных
        /// </summary>
        /// <returns></returns>
        public int getDataBaseIndex()
        {
            return dbIndex;
        }


    }
}
