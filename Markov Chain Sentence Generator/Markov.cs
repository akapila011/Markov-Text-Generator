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
            text = CleanText(text);
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
                    if (String.IsNullOrWhiteSpace(word))
                    {
                        continue;
                    }
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
                        if (!model.ContainsKey(word))
                        {
                            model[word] = new List<string>();  // create entry for current word and add the preceding word to its list
                        }
                    model[word].Add(words[j + 1].ToLower());
                    }
                }
            }
            trained = true;
        }   // end Train()

        public String GenerateSentence(int length = 20)
        {
            if (!trained)
            {
                throw new Exception("The model has not been trained yet.");
            }
            List<String> sentence = new List<string>();
            while (sentence.Count != length)
            {
                List<String> foundWords = new List<String>();
                if (sentence.Count == 0) // no words used yet
                {
                    foundWords = model["START"];
                }
                else
                {
                    try
                    {
                        foundWords = model[sentence[sentence.Count - 1]];  // related words to the last word in constructed sentence
                    }
                    catch (KeyNotFoundException)  // No key found, choose another word
                    {
                        List<string> keyList = new List<string>(model.Keys);
                        string randomKey = "";  // new random word to be used
                        do
                        {
                            randomKey = keyList[random.Next(keyList.Count)];
                        } while ((randomKey == "START") || (randomKey == "END")); // Keep looping for a word that isn't start or end
                        foundWords = model[randomKey];
                    }
                }
                sentence.Add(foundWords[random.Next(foundWords.Count)]); // choose a random word from the list associated with the current word
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

        private String CleanText(String text)
        {
            String cleaned = text.Replace("...", "");
            cleaned = cleaned.Replace("\n", "");
            return cleaned;
        }

    } // end Markov{}
}
