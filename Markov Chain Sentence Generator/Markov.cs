using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markov_Chain_Sentence_Generator
{
    class Markov
    {
        private string text;  // full text read in from file
        private Dictionary<string, Trigram> model;  // {"Hello my": {prefix=["Hello", "my], suffix=List()},...}
        private bool trained;

        static Random random = new Random();

        public Markov(string text)
        {
            this.text = text;
            model = new Dictionary<string, Trigram>();
            this.trained = false;
        }

        public void Train()
        {
            this.trained = false;
            text = CleanText(text);
            string[] sentences = text.Split('.');
            if (sentences.Length < 2)   // too few fullstops
            {
                throw new FormatException("The text provided has no periods('.'). Please use a text with sentences seperated with periods.");
            }
            for (int i = 0; i != sentences.Length; i++)
            {
                string[] words = sentences[i].Split(' ');
                if (words.Length < 3) { break; } // not enough words in sentence, skip sentence
                for (int j = 0; j != words.Length - 2; j++)
                {
                    string index = words[j] + words[j + 1]; // bi-gram prefix to 1 word suffix
                    if (!model.ContainsKey(index))
                    {
                        model[index] = new Trigram(words[j], words[j+1]);  // create entry for current word and add the preceding word to its list
                    }
                    model[index].Add(words[j + 2]);  // e.g. k:v -> "Hello my": {prefix=["Hello", "my], suffix=List()}
                }
            }
            trained = true;
        }   // end Train()

        public string GenerateText(int length = 20)
        {
            if (!trained)
            {
                throw new Exception("The model has not been trained yet.");
            }
            List<string> keyList = new List<string>(model.Keys);  // list of all keys in model [["Hello my"], ["my name"], ["name is"]...]
            List<string> sentence = new List<string>();  // words used in generated sentence, to be returned
            string[] index = model[keyList[random.Next(keyList.Count)]].prefixWords; // get array of words used in the current index
            sentence.Add(index[0]);  // Must ensure the sentence starts with 2 words before the rest of the text commences
            sentence.Add(index[1]);
            for (int i = 1; i != length; i++)
            {
                index[0] = sentence[i - 1];  // get previous 2 words in an array to find the next word
                index[1] = sentence[i];
                try
                {  
                    List<string> suffixes = model[index[0] + index[1]].suffixes;
                    string choice = suffixes[random.Next(suffixes.Count)];
                    sentence.Add(choice);   // get a random suffix using the index (i.e. last 2 words)
                }
                catch (KeyNotFoundException)    // If no prefix pair found select a whole new pair
                {
                    index = model[keyList[random.Next(keyList.Count)]].prefixWords;
                    sentence.Add(index[0]); 
                    sentence.Add(index[1]);
                }
            }
            return string.Join(" ", sentence);
        }   // GenerateText()

        private string CleanText(string text)
        {   // Remove/replace unwanted characters from the full text
            string cleaned = text.Replace("...", "");
            cleaned = cleaned.Replace("\n", "");
            return cleaned;
        }   // end CleanText()

    }   // END Markov{}
}
