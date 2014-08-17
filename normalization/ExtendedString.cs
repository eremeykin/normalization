using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace normalization
{
    class ExtendedString
    {
        String s;

        public ExtendedString(String str)
        {
            s = (String)str.Clone();
        }

        /// <summary>
        /// Удаляет все указанные символы из строки
        /// </summary>
        private void removeChar(char ch)
        {
            int index;
            do
            {
                index = s.IndexOf(ch);
                if (index != -1)
                    s = s.Remove(index, 1);
            }
            while (index != -1);
        }

        /// <summary>
        /// Удаляет все специальные символы из строки
        /// </summary>
        public void removeSpecChars()
        {
            if (s.Contains('.'))
                s = s.Replace('.', ' ');
            //s = s.ToLower();
            char[] specChars = "\"\'%()*+,-./:;=>\\^№!".ToCharArray();
            for (int i = 0; i < specChars.Length; i++)
            {
                this.removeChar(specChars[i]);
            }
        }

        /// <summary>
        /// Возвращает количество слов в строке
        /// </summary>
        public int getWordCount()
        {
            String[] words = s.Split(' ');
            return words.Length;
        }

        //
        public String[] getShingles()
        {
            int shingleLength = 1;
            String[] words = s.Split(' ');
            int shCount = this.getWordCount() - shingleLength + 1;
            String[] shingles = new String[shCount];
            for (int i = 0; i < shCount; i++)
            {
                for (int j = i; j < shingleLength+i; j++)
                {
                    shingles[i] = shingles[i] + " " + words[j];
                }
            }
            return shingles;
        }

        /// <summary>
        /// Возвращает все Слова в строке
        /// </summary>
        /// <returns>Слова[]</returns>
        public List<Word> getWords()
        {
            this.removeSpecChars();
            String[] words = s.Split(' ');
            List<Word> resultWords= new List<Word>();
            for (int i = 0; i < words.Length; i++)
            {
                if (words[i].Length > 2)
                    resultWords.Add(new Word(words[i]));
            }
            return resultWords;
        }


        public override string ToString()
        {
            return s;
        }
    }
}
