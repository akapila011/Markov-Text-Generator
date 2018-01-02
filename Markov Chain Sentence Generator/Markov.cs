using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markov_Chain_Sentence_Generator
{
    public class Markov
    {
        public String ModelName;
        private String text;
        private Dictionary<String, List<String>> model;
        private bool trained;


        public Markov(String name, String text)
        {
            this.ModelName = name;
            this.text = text;
            model["START"] = new List<string>();
            model["END"] = new List<string>();
            this.trained = false;
        }

        public void Train()
        {
            trained = false;
            String[] sentences = text.Split('.');
            for(int i=0; i!=sentences.Length; i++)
            {
                String[] words = sentences[i].Split(' ');
                if (words.Length < 1) { break; } // no words in sentence don't use
                for(int j=0; j!=words.Length; j++)
                {
                    String word = words[j].ToLower();
                    if (j == 0)  // first word, add to start
                    {
                        model["START"].Add(word);
                    }
                    else if (j == words.Length -1)  // last word, goes to end
                    {
                        model["END"].Add(word);
                    }
                    // Now create an entry for each word if it doesn't exist
                }
            }


        }   // end Train()

        public String GenerateSentence()
        {
            return "";
        }   // end GenerateSentence()
    } // end Markov{}
}
