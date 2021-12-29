﻿using System;
using System.Collections.Generic;
using System.IO;
using BigInt = System.Numerics.BigInteger;
using System.Text;

namespace JSONXML
{
    class FileAbout
    {
        public static readonly char[] seporators = {
                '\u0020',
                '\u00A0',
                '\u1680',
                '\u2000',
                '\u2001',
                '\u2002',
                '\u2003',
                '\u2004',
                '\u2006',
                '\u2007',
                '\u2008',
                '\u2009',
                '\u200A',
                '\u202F',
                '\u205F',
                '\u3000',
                '\u2028',
                '\u2029'};

        string _path;
        public string Name { get; private set; }
        public int Size { get; private set; }
        public int CountLines { get; private set; }
        public int WordsWithHyphen { get; private set; }
        public int WordCount { get; private set; }
        public int LettersCount { get; private set; }
        public int PunctuationsCount { get; private set; }
        public string LongestWord { get; private set; } = String.Empty;

        public List<AFileAtribuites> Words { get; private set; } = new List<AFileAtribuites>();
        public List<AFileAtribuites> Letters { get; private set; } = new List<AFileAtribuites>();
        public int NumbersCount { get; private set; }
        public int DigitsCount { get; private set; }

        public FileAbout(string path)
        {
            _path = path;
            Make(_path);
        }

        private void Make(string path)
        {
            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    Name = path.Substring(path.LastIndexOf("\\") + 1);

                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        Size += line.Length;
                        CountLines++;

                        line = line.ToLowerInvariant();
                        var words = line.Split(seporators, StringSplitOptions.RemoveEmptyEntries);
                        for (int i = 0; i < words.Length; i++)
                        {
                            string word = string.Empty;
                            bool isWord = IsWord(words[i], out word);
                            int? indexOfWord = isWord ? indexOfWordOrLetterInList(word, Words) : null;

                            if (isWord && word.IndexOf('-') != -1)
                            {
                                WordsWithHyphen++;
                            }

                            if (BigInt.TryParse(words[i], out _))
                            {
                                NumbersCount++;
                                DigitsCount += words[i].Length;
                            }
                            else if (!isWord)
                            {
                                AddLettersOrNumbersFromInput(words[i]);
                            }
                            else if (!(indexOfWord is null) && indexOfWord != -1)
                            {
                                Words[(int)indexOfWord].Count++;
                               
                                AddLettersOrNumbersFromInput(words[i], isWord: true);
                            }
                            else
                            {
                                if (LongestWord.Length < word.Length)
                                {
                                    LongestWord = word;
                                }

                                AFileAtribuites Fileword = new AFileAtribuites() { Value = word };

                                Words.Add(Fileword);

                                AddLettersOrNumbersFromInput(words[i], isWord: true);
                            }
                        }
                    }

                    Words.Sort(new FileAtribuitesCompare());
                    Letters.Sort(new FileAtribuitesCompare());
                    LettersCount = CountValue(Letters);
                    WordCount = CountValue(Words);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        
        }

        private void AddLettersOrNumbersFromInput(string word, bool isWord = false)
        {
            for (int i = 0; i < word.Length; i++)
            {
                if (char.IsLetterOrDigit(word[i]))
                {
                    int indexOfLetter = indexOfWordOrLetterInList(word[i].ToString(), Letters);

                    if (char.IsDigit(word[i]))
                    {
                        DigitsCount++;
                    }
                    else if (indexOfLetter != -1)
                    {
                        Letters[indexOfLetter].Count++;
                    }
                    else
                    {
                        AFileAtribuites letter = new AFileAtribuites() { Value = word[i].ToString() };
                        Letters.Add(letter);
                    }
                }
                else if (char.IsPunctuation(word[i]))
                {
                    PunctuationsCount++;
                }
            }
        }

        private bool IsWord(string iptut, out string word)
        {
            int indexFirstLetter = GetIndexOfFirstOrLastLetter(iptut, isLast: false);
            int indexLastLetter = GetIndexOfFirstOrLastLetter(iptut, isLast: true);

            if (indexFirstLetter != -1 && indexLastLetter != -1)
            {
                for (int i = indexFirstLetter; i <= indexLastLetter; i++)
                {
                    if (iptut[i] == '-' || iptut[i] == '\'')
                    {
                        continue;
                    }
                    else if (!char.IsLetter(iptut[i]))
                    {
                        word = iptut;
                        return false;
                    }
                }

                word = iptut[indexFirstLetter..++indexLastLetter];
                return true;
            }
            else
            {
                word = iptut;
                return false;
            }
        }

        private int GetIndexOfFirstOrLastLetter(string iptut, bool isLast)
        {
            int index = -1;
            int lenght = iptut.Length;
            int start = isLast ? lenght - 1 : 0;

            while (lenght-- > 0)
            {
                if (char.IsLetter(iptut[start]))
                {
                    index = start;
                    break;
                }

                start = isLast ? --start : ++start;
            }

            return index;
        }

        private int indexOfWordOrLetterInList(string word, List<AFileAtribuites> listOfIndexOfAFileAtribuites)
        {
            for (int i = 0; i < listOfIndexOfAFileAtribuites.Count; i++)
            {
                if (word == listOfIndexOfAFileAtribuites[i].Value) return i;
            }

            return -1;
        }

        int CountValue(List<AFileAtribuites> List)
        {
            int sum = 0;
            for (int i = 0; i < List.Count; i++)
            {
                sum += List[i].Count;
            }

            return sum;
        }
    }
}
