using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace normalization
{
    class Word
    {
        
        protected String w;

        /// <summary>
        /// Конструктор для Word c параметрм
        /// </summary>
        /// <param name="word">Слово</param>
        public Word(String word)
        {
            setWord(word);
        }

        /// <summary>
        /// Возвращает массив триграмм для слова
        /// </summary>
        /// <returns>Массив триграмм</returns>
        public String[] getTrigrams()
        {
            int n = 3;// триграмм
            String[] trigrams = new String[this.w.Length - n + 1];
            for (int i = 0; i < trigrams.Length; i++)
            {
                for (int j = i; j < i + n; j++)
                {
                    trigrams[i] = trigrams[i] + w[j];
                }
            }
            return trigrams;
        }

        /// <summary>
        /// Возвращает строковое значение слова
        /// </summary>
        /// <returns>Слово</returns>
        public String getWord()
        {
            return this.w;
        }

       
        /// <summary>
        /// Устанавливает значение слова
        /// </summary>
        /// <param name="word">Слово</param>
        private void setWord(String word)
        {
            Regex regex = new Regex("\\w+");
            //TODO добавить проверку что word является словом
            //if(word.Equals(regex.Match(word).Value))
            if (true)
            {
                w = word;
            }
            else
            {
                throw new ArgumentException(word + " is not a word");
            }
        }

        /// <summary>
        /// Возвращает метрику до слова, указанного параметром
        /// </summary>
        /// <param name="word2">Слово, до которого определяется метрика</param>
        /// <returns></returns>
        public int getMetric(Word word2)
        {
            return DamerauLevenshteinDistance(this.ToString(), word2.ToString());
        }

        /// <summary>
        /// Возвращает метрику между двумя словами
        /// </summary>
        /// <param name="word1">Первое слово</param>
        /// <param name="word2">Второе слово</param>
        /// <returns></returns>
        private static int getMetric(Word word1, Word word2)
        {
            return DamerauLevenshteinDistance(word1.ToString(), word2.ToString());
        }

        /// <summary>
        /// Определяет расстояние по Дамерау-Левенштейну
        /// Источник http://en.academic.ru/dic.nsf/enwiki/1618870
        /// </summary>
        /// <param name="source">Строка 1</param>
        /// <param name="target">Строка 2</param>
        /// <returns></returns>
        private static Int32 DamerauLevenshteinDistance(String source, String target)
        {
            if (String.IsNullOrEmpty(source))
            {
                if (String.IsNullOrEmpty(target))
                {
                    return 0;
                }
                else
                {
                    return target.Length;
                }
            }
            else if (String.IsNullOrEmpty(target))
            {
                return source.Length;
            }

            Int32 m = source.Length;
            Int32 n = target.Length;
            Int32[,] H = new Int32[m + 2, n + 2];

            Int32 INF = m + n;
            H[0, 0] = INF;
            for (Int32 i = 0; i <= m; i++) { H[i + 1, 1] = i; H[i + 1, 0] = INF; }
            for (Int32 j = 0; j <= n; j++) { H[1, j + 1] = j; H[0, j + 1] = INF; }

            SortedDictionary<Char, Int32> sd = new SortedDictionary<Char, Int32>();
            foreach (Char Letter in (source + target))
            {
                if (!sd.ContainsKey(Letter))
                    sd.Add(Letter, 0);
            }

            for (Int32 i = 1; i <= m; i++)
            {
                Int32 DB = 0;
                for (Int32 j = 1; j <= n; j++)
                {
                    Int32 i1 = sd[target[j - 1]];
                    Int32 j1 = DB;

                    if (source[i - 1] == target[j - 1])
                    {
                        H[i + 1, j + 1] = H[i, j];
                        DB = j;
                    }
                    else
                    {
                        H[i + 1, j + 1] = Math.Min(H[i, j], Math.Min(H[i + 1, j], H[i, j + 1])) + 1;
                    }

                    H[i + 1, j + 1] = Math.Min(H[i + 1, j + 1], H[i1, j1] + (i - i1 - 1) + 1 + (j - j1 - 1));
                }

                sd[source[i - 1]] = i;
            }

            return H[m + 1, n + 1];
        }

        /// <summary>
        /// Отображает слово в String
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return w;
        }
    }
}
