using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lingo_WebApp
{
    public class LingoWord
    {
        public List<LingoLetter> CorrectWord { get; } = new List<LingoLetter>();

        public List<LingoLetter> GuessedWord { get; } = new List<LingoLetter>();

        public LingoWord(string word)
        {
            if (word.Length != 5) throw new ArgumentException("This Lingo board only supports 5 letter words");
            word = word.ToLowerInvariant();
            foreach (char character in word)
            {
                CorrectWord.Add(new LingoLetter(character));
            }
        }

        public void SetGuessedWord(string word)
        {
            if (word.Length != 5) throw new ArgumentException("This Lingo board only supports 5 letter words");
            GuessedWord.Clear();
            word = word.ToLowerInvariant();
            foreach (char character in word)
            {
                GuessedWord.Add(new LingoLetter(character));
            }
        }
    }

    public class LingoLetter
    {
        public Status LetterStatus { get; set; }
        public char character { get; set; }
        public LingoLetter(char letter)
        {
            character = letter;
            LetterStatus = Status.unknown;
        }
    }

    public enum Status
    {
        unknown, wrongLocation, Correct
    }
    public static class extensions
    {
        public static void Check(this LingoWord lingoWord, string word)
        {
            if (string.IsNullOrEmpty(word)) return;

            word = word.ToLowerInvariant();
            try
            {
                lingoWord.SetGuessedWord(word);

                List<char> correctLetters = new List<char>();
                foreach (LingoLetter letter in lingoWord.CorrectWord)
                {
                    if (word.Contains(letter.character))
                    {
                        correctLetters.Add(letter.character);
                    }
                }

                lingoWord.GuessedWord.ForEach(l => l.LetterStatus = Status.unknown);

                int i = 0;
                foreach (char character in word)
                {
                    if (correctLetters.Contains(character))
                    {
                        lingoWord.GuessedWord[i].LetterStatus = Status.wrongLocation;
                    }
                    if (character == lingoWord.CorrectWord[i].character)
                    {
                        lingoWord.GuessedWord[i].LetterStatus = Status.Correct;
                    }
                    i++;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }
    }
}
