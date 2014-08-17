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
        private String w;

        public Word(String word)
        {
            setWord(word);
        }
        public String[] getTrigrams()
        {
            int n = 3;// триграмм
            String[] trigrams = new String[this.w.Length-n+1];
            for (int i = 0; i < trigrams.Length; i++)
            {
                for (int j = i; j < i+n; j++)
                {
                    trigrams[i] = trigrams[i] + w[j];
                }
            }
            return trigrams;
        }

        public String getWord()
        {
            return this.w;
        }

        private void setWord(String word)
        {
            Regex regex = new Regex("\\w+");
            //if(word.Equals(regex.Match(word).Value))
            if (true)
            {
                w=word;
            }
            else
            {
                throw new ArgumentException(word + " is not a word");
            }
        }
        public override string ToString()
        {
            return w;
        }
    }
}
