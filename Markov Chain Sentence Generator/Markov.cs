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

        static Random random = new Random();


        public Markov(String name, String text)
        {
            this.ModelName = name;
            this.text = text;
            model = new Dictionary<string, List<string>>();
            model["START"] = new List<string>();
            model["END"] = new List<string>();
            this.trained = false;
        }

        public void Train()
        {
            trained = false;
            String[] sentences = text.Split('.');
            if (sentences.Length < 2)
            {
                throw new FormatException("The text provided has no periods('.'). Please use a text with sentences seperated with periods.");
            }
            for(int i=0; i!=sentences.Length; i++)
            {
                String[] words = sentences[i].Split(' ');
                if (words.Length < 1) { break; } // no words in sentence don't use
                for(int j=0; j!=words.Length; j++)
                {
                    String word = words[j].ToLower();
                    if (j == words.Length - 1)  // last word, goes to end
                    {
                        model["END"].Add(word);
                    }
                    else   // end words cannot be saved as indexes for lookup in the model, only start and middle words
                    {  
                        if (j == 0)  // first word, add to start
                        {
                            model["START"].Add(word);
                        }
                        model[word] = new List<string>();  // create entry for current word and add the preceding word to its list
                        model[word].Add(words[j + 1].ToLower());
                    }
                }
            }
        }   // end Train()

        public String GenerateSentence()
        {
            List<String> sentence = new List<string>();
            while (true)
            {
                if (sentence.Count == 0) // no words used yet
                {
                    sentence.Add(model["START"][random.Next(model["START"].Count)]); // choose any random word from list of start words
                }
                else if (IsEndWord(sentence[sentence.Count-1]))  // last word, stop generating text
                {
                    break;
                }
                else
                {
                    try
                    {
                        String lastWord = sentence[sentence.Count - 1];
                        sentence.Add(model[lastWord][random.Next(model[lastWord].Count)]);
                    }
                    catch (KeyNotFoundException)
                    {
                        continue;
                    }
                }
            }
            return String.Join(" ", sentence);
        }   // end GenerateSentence()

        private bool IsEndWord(String word)
        {
            foreach(String w in model["END"])
            {
                if (w == word) { return true; }  // match found
            }
            return false;
        }   // end IsEndWord()

    } // end Markov{}
}
