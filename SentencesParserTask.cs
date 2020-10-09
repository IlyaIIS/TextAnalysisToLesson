using System.Collections.Generic;
using System.Linq;
using System;

namespace TextAnalysis
{
    static class SentencesParserTask
    {
        public static List<List<string>> ParseSentences(string text)
        {
            var sentencesList = new List<List<string>>();
            text = " " + text.ToLower();                                          //на случай пустых строк
            
            char[] sign = { '.', '?', '!', ';', ':', '(', ')' };           //список символов, отделяющих предложения

            int sentNum = 0, wordNum = 0, i = 0;
            sentencesList.Add(new List<string>());                         //деление текста на предложения со словами
            do
            {
                if (Char.IsLetter(text[i]) || text[i] == '\'')
                {
                    sentencesList[sentNum].Add("");
                    do
                    {
                        sentencesList[sentNum][wordNum] += text[i];
                        i++;
                    } while ((i < text.Length) && (Char.IsLetter(text[i]) || text[i] == '\''));
                    wordNum++;
                    i--;
                }
                else
                if (sign.Contains(text[i]))
                {
                    wordNum = 0;
                    sentencesList.Add(new List<string>());
                    sentNum++;
                }

                i++;
            } while (i < text.Length);

            for (int sen = 0; sen < sentencesList.Count; sen++)             //удаление пустых предложений
            {
                if (sentencesList[sen].Count == 0) { sentencesList.RemoveAt(sen); sen--; }
            }

            return sentencesList;
        }
    }
}