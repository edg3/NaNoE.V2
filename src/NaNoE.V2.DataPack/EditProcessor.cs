using System.Collections.Generic;
using System.IO;
using System.Linq;
using PlatformSpellCheck;

namespace NaNoE.V2.DataPack
{
    /// <summary>
    /// Static function that runs with the edit suggestions for us
    /// </summary>
    public class EditProcessor
    {
        /// <summary>
        /// Static reference, helps with the data binding and updating
        /// </summary>
        private static EditProcessor _instance;
        public static EditProcessor Instance
        {
            get { return _instance; }
        }

        /// <summary>
        /// Private data it needs
        /// </summary>
        private SpellChecker _spellChecker;
        private List<string> _ignorables = new List<string>() { "i", "i'm", "i'll" };

        /// <summary>
        /// When created it makes the static reference, and loads the 'edits.txt'
        /// Note: ';' isn't allowed anywhere besides to split the 3 elements in EditOption data type
        /// </summary>
        public EditProcessor()
        {
            _instance = this;
            _spellChecker = new SpellChecker();

            EditOptions = new List<EditOption>();

            if (File.Exists("edits.txt"))
            {
                using (var f = File.OpenRead("edits.txt"))
                {
                    using (var reader = new StreamReader(f))
                    {
                        string line = "";
                        while ((line = reader.ReadLine()) != null)
                        {
                            var splt = line.Split(';');
                            EditOptions.Add(new EditOption(splt[0], splt[1], splt[2]));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Process the given text in full
        /// </summary>
        /// <param name="text">Paragraph to check</param>
        /// <returns>List of edit suggestions</returns>
        public List<string> Process(string text)
        {
            List<string> answer = new List<string>();

            // Remove unneeded formating
            var splt = text.Split(' ');
            for (int i = 0; i < splt.Length; ++i)
            {
                splt[i] = splt[i].Replace(",", "")
                                 .Replace(".", "")
                                 .Replace(";", "")
                                 .Replace("\"", "");
                if (splt[i][0] == '\'') splt[i].Remove(0, 1);
                if ((splt[i])[splt[i].Length - 1] == '\'') splt[i].Remove(splt[i].Length - 1, 1);
            }

            // Go through each word
            for (int j = 0; j < splt.Length; ++j)
            {
                if (splt[j].Length > 0) Check(splt[j], answer, j + 1);
            }

            return answer;
        }

        /// <summary>
        /// Reference check used for if we are, or arent, in the edit view
        /// </summary>
        private string _position = "";
        public string Position
        {
            get { return _position; }
            set { _position = value; }
        }

        /// <summary>
        /// List of options to check for 'edits'
        /// </summary>
        public List<EditOption> EditOptions { get; private set; }

        /// <summary>
        /// Check word an compile a set of answers for said word
        /// </summary>
        /// <param name="v">The word to check</param>
        /// <param name="answer">List to add suggestion answers to as needed</param>
        /// <param name="wordNum">The position of the word number in the original paragraph (see above)</param>
        private void Check(string v, List<string> answer, int wordNum)
        {
            // Spell check
            if (_spellChecker.Check(v).Count() > 0)
            {
                if (!_ignorables.Contains(v))
                {
                    answer.Add(wordNum + "] Spell Check: " + v);
                }
            }

            // Only spell check unless in Edit mode.
            if (_position != "edit") return;

            // Linq use to find elements that match either full words or end of words
            var answersA = (from item in EditOptions
                            where item.Opt == v || (item.Opt[0] == '-' && v.EndsWith(item.SubOptimal))
                            select item).FirstOrDefault();

            // Note: this may only give one reasuly, however that should be fine as it would be edited regardless of it being in multiple options
            //  - e.g. You make the word 'thing' as an Opt, it flags for that AND for '-ing'
            if (null != answersA) answer.Add(wordNum + "] " + v + ": " + answersA.Detail);
        }
    }
}
