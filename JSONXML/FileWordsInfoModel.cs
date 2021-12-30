using System;
using System.Collections.Generic;
using System.IO;
using BigInt = System.Numerics.BigInteger;
using System.Text;

namespace JSONXML
{
    class FileWordsInfoModel
    {
        public static readonly char[] seporators = {
                '\u0020', //SP. Space (SP)
                '\u00A0', //NBSP. No-Break Space (NBSP)
                '\u1680', //Ogham Space Mark.
                '\u2000', //En Quad
                '\u2001', //Em Quad
                '\u2002', //En Space
                '\u2003', //Em Space
                '\u2004', //Three-Per-Em Space
                '\u2005', //Four-Per-Em Space
                '\u2006', //Six-Per-Em Space
                '\u2007', //Fiqure space
                '\u2008', //Punctuation space
                '\u2009', //Thin space
                '\u200A', //Hair space
                '\u202F', //Narrow No-Break Space (NNBSP)
                '\u205F', //Medium Mathematical Space (MMSP)
                '\u3000', //Ideographic Space
                '\u2028', //Line Separator
                '\u2029'  //Paragraph Separator
        };

        readonly string _path;
        public string Name { get; private set; }
        public int Size { get; private set; }
        public int CountLines { get; private set; }
        public int WordsWithHyphen { get; private set; }
        public int WordCount { get; private set; }
        public int LettersCount { get; private set; }
        public int PunctuationsCount { get; private set; }
        public string LongestWord { get; private set; } = String.Empty;

        public List<FileAtribuiteModel> Words { get; private set; } = new List<FileAtribuiteModel>();
        public List<FileAtribuiteModel> Letters { get; private set; } = new List<FileAtribuiteModel>();
        public int NumbersCount { get; private set; }
        public int DigitsCount { get; private set; }

        public FileWordsInfoModel(string path)
        {
            _path = path;
            MakeModel(_path);
        }

        private void MakeModel(string path)
        {
            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    Name = path.Substring(path.LastIndexOf("\\") + 1);

                    string line;
                    Console.WriteLine("Making model");
                    while ((line = sr.ReadLine()) != null)
                    {
                        Size += line.Length;
                        CountLines++;

                        line = line.ToLowerInvariant();
                        var words = line.Split(seporators, StringSplitOptions.RemoveEmptyEntries);
                        for (int i = 0; i < words.Length; i++)
                        {
                            string word = string.Empty;
                            bool isWord = TryParseWord(words[i], out word);
                            int? indexOfWord = isWord ? indexOfWordOrLetterInList(word, Words) : null;

                            if (BigInt.TryParse(words[i], out _))
                            {
                                NumbersCount++;
                                DigitsCount += words[i].Length;
                                continue;
                            }
                            else if (isWord)
                            {
                                if (word.IndexOf('-') != -1)
                                {
                                    WordsWithHyphen++;
                                }

                                if (indexOfWord != -1)
                                {
                                    Words[(int)indexOfWord].Count++;
                                }
                                else
                                {
                                    if (LongestWord.Length < word.Length) LongestWord = word;

                                    FileAtribuiteModel Fileword = new FileAtribuiteModel() { Value = word };

                                    Words.Add(Fileword);
                                }
                            }

                            AddLettersOrNumbersFromInput(words[i]);
                        }
                    }

                    Words.Sort(new FileAtribuitesCompare());
                    Letters.Sort(new FileAtribuitesCompare());
                    LettersCount = CountValue(Letters);
                    WordCount = CountValue(Words);
                    Console.WriteLine("End making model");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        
        }

        private void AddLettersOrNumbersFromInput(string word)
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
                        FileAtribuiteModel letter = new FileAtribuiteModel() { Value = word[i].ToString() };
                        Letters.Add(letter);
                    }
                }
                else if (char.IsPunctuation(word[i]))
                {
                    PunctuationsCount++;
                }
            }
        }

        private bool TryParseWord(string iptut, out string word)
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
                    else if (!char.IsLetter(iptut[i])) // 12B is not a word 
                    {
                        word = iptut;
                        return false;
                    }
                }

                word = iptut[indexFirstLetter.. ++indexLastLetter];
                return true;
            }
            else
            {
                word = iptut;
                return false;
            }
        }

        private int GetIndexOfFirstOrLastLetter(string input, bool isLast)
        {
            int index = -1;
            int lenght = input.Length;
            int start = isLast ? lenght - 1 : 0;

            while (lenght-- > 0)
            {
                if (char.IsLetter(input[start]))
                {
                    index = start;
                    break;
                }

                start = isLast ? --start : ++start;
            }

            return index;
        }

        private int indexOfWordOrLetterInList(string word, List<FileAtribuiteModel> listOfIndexOfAFileAtribuites)
        {
            for (int i = 0; i < listOfIndexOfAFileAtribuites.Count; i++)
            {
                if (word == listOfIndexOfAFileAtribuites[i].Value) return i;
            }

            return -1;
        }

        int CountValue(List<FileAtribuiteModel> List)
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
