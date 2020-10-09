using System.Collections.Generic;
using System.Linq;
using System;

namespace TextAnalysis
{
    static class FrequencyAnalysisTask
    {
        public static Dictionary<string, string> GetMostFrequentNextWords(List<List<string>> text)
        {
            var result = new Dictionary<string, string>();

            result = ParceNGrams(FindNGrams(text, 2));
            result = result.Concat(ParceNGrams(FindNGrams(text, 3))).ToDictionary(x => x.Key, x => x.Value);

            return result;
        }

        static List<string[]> FindNGrams(List<List<string>> sentence, int N)      //Возвращает лист Nмерных масивов Nграмм
        {
            List<string[]> NGram = new List<string[]>();

            for (int senNum = 0; senNum < sentence.Count; senNum++)
            {
                if (sentence[senNum].Count >= N)
                {
                    for (int wordNum = 0; wordNum < sentence[senNum].Count - (N - 1); wordNum++)
                    {
                        NGram.Add(new string[N]);
                        for (int i = 0; i < N; i++)
                        {
                            NGram[NGram.Count - 1][i] = sentence[senNum][wordNum + i];
                        }
                    }
                }
            }

            return NGram;
        }

        static Dictionary<string, string> ParceNGrams(List<string[]> nGram)       //создаё словарь, где key - начало, а volue - окончание N-грамма
        {
            Dictionary<string, string> nGrams = new Dictionary<string, string>();

            for (int num = 0; num < nGram.Count;)
            {
                Dictionary<string, int> valueDict = new Dictionary<string, int>();
                int N = nGram[0].Length;
                string[] key = new string[N - 1];

                for (int i = 0; i < N - 1; i++) key[i] = nGram[num][i];   //формирование массива начала N-граммы

                for (int i = num; i < nGram.Count; i++)                   //формирование словаря концов N-грамма с кол-вом их сочетаний с key
                {
                    if (IsBeginningNGram(nGram, key, N, i))
                    {
                        if (valueDict.ContainsKey(nGram[i][N - 1]))
                            valueDict[nGram[i][N - 1]]++;
                        else
                            valueDict.Add(nGram[i][N - 1], 1);

                        nGram.RemoveAt(i);
                        i--;
                    }
                }

                int max = valueDict.Values.Max();

                for (int i = 0; i < valueDict.Keys.Count; i++)              //Убирает редко встреченные окончания N-грамма
                {
                    string k = valueDict.ElementAt(i).Key;
                    if (valueDict[k] < max) { valueDict.Remove(k); i--; }
                }

                for (int i = 0; i < valueDict.Keys.Count - 1;)              //Отберает окончание по лексикографическому признаку
                {
                    string kNow = valueDict.ElementAt(i).Key;
                    string kNext = valueDict.ElementAt(i + 1).Key;
                    if (String.CompareOrdinal(kNow, 0, kNext, 0, Math.Max(kNow.Length, kNext.Length)) < 0)   // 1 если левый хуже, -1 если правый хуже
                        valueDict.Remove(kNext);
                    else
                        valueDict.Remove(kNow);
                }

                string keyStr = key[0];
                for (int i = 1; i < N - 1; i++) keyStr += " " + key[i];

                nGrams.Add(keyStr, valueDict.ElementAt(0).Key);
            }

            return nGrams;
        }
        static bool IsBeginningNGram(List<string[]> nGramm, string[] key, int N, int posNow)  //проверка, является ли key началом N-граммa
        {
            bool output = true;
            for (int i = 0; i < N - 1; i++)
            {
                output = output && nGramm[posNow][i] == key[i];
            }

            return output;
        }
    }
}