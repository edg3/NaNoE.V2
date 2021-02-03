using NaNoE.V2.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;

namespace NaNoE.V2.Windows.Popups
{
    /// <summary>
    /// Interaction logic for RepetitionWindow.xaml
    /// 
    /// Note: Used a clumsy approach just to make sure we have a feature implementation
    /// </summary>
    public partial class RepetitionWindow : Window
    {
        public ObservableCollection<string> IgnoredWords { get; set; }

        public RepetitionWindow()
        {
            IgnoredWords = new ObservableCollection<string>();

            InitializeComponent();

            if (!File.Exists("rep_ignore.txt"))
            {
                using (var f = File.OpenWrite("rep_ignore.txt"))
                {
                    using (var r = new StreamWriter(f))
                    {
                        r.WriteLine("and");
                    }
                }
                IgnoredWords.Add("and");
            }
            else
            {
                using (var f = File.OpenRead("rep_ignore.txt"))
                {
                    using (var r = new StreamReader(f))
                    {
                        string line = null;
                        while ((line = r.ReadLine()) != null)
                        {
                            if (line.Length > 0) IgnoredWords.Add(line);
                        }
                    }
                }
            }

            lstIgnored.ItemsSource = IgnoredWords;
        }

        private void butIgnore_Click(object sender, RoutedEventArgs e)
        {
            if (txtWord.Text.Length < 1)
            {
                MessageBox.Show("Please put something in to ignore. Note, this is meant for single punctuated words.");
                return;
            }

            IgnoredWords.Add(txtWord.Text);
            txtWord.Text = "";

            BackupIgnores();
        }

        private void BackupIgnores()
        {
            File.Delete("rep_ignore.txt");
            using (var f = File.OpenWrite("rep_ignore.txt"))
            {
                using (var w = new StreamWriter(f))
                {
                    foreach (var line in IgnoredWords)
                        w.WriteLine(line);
                }
            }
        }

        private void butRemoveIgnore_Click(object sender, RoutedEventArgs e)
        {
            if (null == lstIgnored.SelectedItem)
            {
                MessageBox.Show("Please select what you'd like to remove the ignore from...");
            }
            else
            {
                var word = lstIgnored.SelectedItem;
                IgnoredWords.Remove(word.ToString());
                BackupIgnores();
            }
        }

        public Dictionary<string, int> WordCounts;

        private void butRun_Click(object sender, RoutedEventArgs e)
        {
            grdRunning.Visibility = Visibility.Visible;
            WordCounts = new Dictionary<string, int>();

            char[] spac = new char[1] { '\n' };

            var wordData = DataConnection.Instance.GetWordData();
            foreach (var line in wordData)
            {
                var seperated = line.Split('.');
                foreach (var sentence in seperated)
                {
                    var words = sentence.Split(' ');
                    foreach (var word in words)
                    {
                        var minWord = word
                            .Replace(',', ' ')
                            .Replace('"', ' ')
                            .Replace(';', ' ')
                            .Replace('\'', ' ')
                            .Replace('!', ' ')
                            .Replace('?', ' ');
                        minWord = minWord.ToLower();
                        minWord = new string(minWord.ToCharArray().Where(c => !Char.IsWhiteSpace(c)).ToArray());
                        // Ignore single char words, like 'a'
                        if (minWord.Length > 1)
                        {
                            if (!IgnoredWords.Contains(minWord))
                            {
                                if (WordCounts.ContainsKey(minWord))
                                {
                                    WordCounts[minWord]++;
                                }
                                else
                                {
                                    WordCounts.Add(minWord, 1);
                                }
                            }
                        }
                    }
                }
            }

            ObservableCollection<string> answer = new ObservableCollection<string>();

            foreach (var item in WordCounts.Keys)
            {
                answer.Add(WordCounts[item].ToString("000000000") + "\t" + item);
            }

            var answerList = answer.OrderBy(a => -int.Parse(a.Split('\t')[0])).ToList();

            lstFindings.ItemsSource = answerList;

            grdRunning.Visibility = Visibility.Hidden;
            MessageBox.Show("Count run complete.");
        }
    }
}
