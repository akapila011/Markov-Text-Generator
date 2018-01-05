using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markov_Chain_Sentence_Generator
{
    class Trigram
    {

        public string[] prefixWords;
        public List<string> suffixes;

        public Trigram(string prefix1, string prefix2)
        {
            prefixWords = new string[] { prefix1, prefix2 };
            this.suffixes = new List<string>();
        }

        public void Add(string suffix)
        {
            suffixes.Add(suffix);
        }   // end Add()
    }   // END Trigram{}
}
