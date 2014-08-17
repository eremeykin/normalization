using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace normalization
{
    class DataBaseWord : Word
    {
        private int dbIndex;//сюда записывается первичный ключ строки в которую входит слово

        // <summary>
        /// Конструктор для DataBaseWord c параметрм
        /// </summary>
        /// <param name="word">Слово</param>
        public DataBaseWord(String word,int index):base(word)
        {
            setWord(word,index);
        }

        /// <summary>
        /// Устанавливает значение слова
        /// </summary>
        /// <param name="word">Слово</param>
        private void setWord(String word,int index)
        {
            Regex regex = new Regex("\\w+");
            //TODO добавить проверку что word является словом
            //if(word.Equals(regex.Match(word).Value))
            if (true)
            {
                base.w = word;
                this.dbIndex = index;
            }
            else
            {
                throw new ArgumentException(word + " is not a word");
            }
        }
        /// <summary>
        /// Возвращает значение первичного ключа строки в которой записано слово
        /// </summary>
        /// <returns>Первичный ключ</returns>
        public int getDataBaseIndex()
        {
            return this.dbIndex;
        }

    }
}
