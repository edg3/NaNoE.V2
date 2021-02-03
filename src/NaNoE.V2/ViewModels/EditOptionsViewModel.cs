using NaNoE.V2.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace NaNoE.V2.ViewModels
{
    /// <summary>
    /// Edit Options View Model
    /// </summary>
    class EditOptionsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public EditOptionsViewModel()
        {
            _remove = new CommandBase(new Action(_run_remove));
            _addOption = new CommandBase(new Action(_run_addOption));
            _removeIgnore = new CommandBase(new Action(_run_removeIgnore));
            _addIgnore = new CommandBase(new Action(_run_addIgnore));
            _removePhrase = new CommandBase(new Action(_run_removePhrase));
            _addPhrase = new CommandBase(new Action(_run_addPhrase));
        }

        /// <summary>
        /// List of options
        /// </summary>
        public List<EditOption> Options
        {
            get { return EditProcessor.Instance.EditOptions; }
        }

        /// <summary>
        /// List of words ignored
        /// </summary>
        public List<string> IgnoredOptions
        {
            get { return EditProcessor.Instance.Ignored; }
        }

        /// <summary>
        /// List of phrases we want to fix
        /// </summary>
        public List<string> PhraseOptions
        {
            get { return EditProcessor.Instance.PhraseOptions; }
        }

        /// <summary>
        /// Edit option
        /// e.g. 'this' will be shown as an edit suggestion for all words that are 'this'
        /// e.g. '-tion' will be show as an edit duggestion for all words that are ending with 'tion', like 'suggestion' would be flagged
        /// </summary>
        private string _opt = "";
        public string Opt
        {
            get { return _opt; }
            set
            {
                _opt = value;
                if (null != PropertyChanged) PropertyChanged(this, new PropertyChangedEventArgs("Opt"));
            }
        }

        /// <summary>
        /// Ignore option
        /// e.g. 'family' ignores the '-ly' word 'family''
        /// </summary>
        private string _ignoreOpt = "";
        public string IgnoreOpt
        {
            get { return _ignoreOpt; }
            set
            {
                _ignoreOpt = value;
                if (null != PropertyChanged) PropertyChanged(this, new PropertyChangedEventArgs("IgnoreOpt"));
            }
        }

        /// <summary>
        /// Detail shown as edit
        /// e.g. 'Use -ing words less'
        /// </summary>
        private string _detail = "";
        public string Detail
        {
            get { return _detail; }
            set
            {
                _detail = value;
                if (null != PropertyChanged) PropertyChanged(this, new PropertyChangedEventArgs("Detail"));
            }
        }

        /// <summary>
        /// The message to show when the edit detail is double clicked in the list
        /// - Seperate lines with '|' character for the popup box
        /// </summary>
        private string _message = "";
        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                if (null != PropertyChanged) PropertyChanged(this, new PropertyChangedEventArgs("Message"));
            }
        }

        /// <summary>
        /// Selected edit suggestion
        /// </summary>
        private EditOption _selected = null;
        public EditOption Selected
        {
            get { return _selected; }
            set
            {
                _selected = value;
                if (null != PropertyChanged) PropertyChanged(this, new PropertyChangedEventArgs("Selected"));
            }
        }

        /// <summary>
        /// Removed selected edit option
        /// </summary>
        private void _run_remove()
        {
            EditProcessor.Instance.EditOptions.Remove(Selected);
            Selected = null;
            VisibleList.Items.Refresh();
        }

        /// <summary>
        /// Removal command
        /// </summary>
        private ICommand _remove;
        public ICommand Remove
        {
            get { return _remove; }
        }

        /// <summary>
        /// Run Add edit option
        /// </summary>
        private void _run_addOption()
        {
            if ((Opt.Trim().Length > 0) && (Detail.Trim().Length > 0) && (Message.Trim().Length > 0))
            {
                EditProcessor.Instance.EditOptions.Add(
                        new EditOption(
                            Opt.Trim().ToLower(),
                            Detail.Trim(),
                            Message.Trim()
                            )
                    );
                Opt = "";
                Detail = "";
                Message = "";
                if (null != PropertyChanged) PropertyChanged(this, new PropertyChangedEventArgs("Options"));
                VisibleList.Items.Refresh();
            }
        }

        /// <summary>
        /// Add edit option
        /// </summary>
        private ICommand _addOption;
        public ICommand AddOption
        {
            get { return _addOption; }
        }

        /// <summary>
        /// Run add Ignore option
        /// </summary>
        private void _run_addIgnore()
        {
            var trimmed = IgnoreOpt.Trim();
            if (trimmed.Length > 0)
            {
                EditProcessor.Instance.Ignored.Add(trimmed.ToLower());
                IgnoreOpt = "";
                if (null != PropertyChanged) PropertyChanged(this, new PropertyChangedEventArgs("IgnoredOptions"));
                VisibleIgnoredList.Items.Refresh();
            }
        }

        /// <summary>
        /// Add ignore option
        /// </summary>
        private ICommand _addIgnore;
        public ICommand AddIgnore
        {
            get { return _addIgnore; }
        }

        /// <summary>
        /// For visibility update
        /// </summary>
        public ListBox VisibleList { get; internal set; }

        /// <summary>
        /// Selected ignore word
        /// </summary>
        private string _selectedIgnore = null;
        public string SelectedIgnore
        {
            get { return _selectedIgnore; }
            set
            {
                _selectedIgnore = value;
                if (null != PropertyChanged) PropertyChanged(this, new PropertyChangedEventArgs("SelectedIgnore"));
            }
        }

        /// <summary>
        /// Selected phrase option
        /// </summary>
        private string _selectedPhrase = null;
        public string SelectedPhrase
        {
            get { return _selectedPhrase; }
            set
            {
                _selectedPhrase = value;
                if (null != PropertyChanged) PropertyChanged(this, new PropertyChangedEventArgs("SelectedPhrase"));
            }
        }

        /// <summary>
        /// Run Remove Ignore
        /// </summary>
        private void _run_removeIgnore()
        {
            if (null != SelectedIgnore)
            {
                EditProcessor.Instance.Ignored.Remove(SelectedIgnore);
                SelectedIgnore = null;
                VisibleIgnoredList.Items.Refresh();
            }
        }

        /// <summary>
        /// Remove ignore
        /// </summary>
        private ICommand _removeIgnore;
        public ICommand RemoveIgnore
        {
            get { return _removeIgnore; }
        }

        /// <summary>
        /// Run Remove Phrase
        /// </summary>
        private void _run_removePhrase()
        {
            if (null != SelectedPhrase)
            {
                EditProcessor.Instance.PhraseOptions.Remove(SelectedPhrase);
                SelectedPhrase = null;
                VisiblePhraseList.Items.Refresh();
            }
        }

        /// <summary>
        /// Remove Phrase
        /// </summary>
        private ICommand _removePhrase;
        public ICommand RunRemovePhrase
        {
            get { return _removePhrase; }
        }

        /// <summary>
        /// Phrase to look for
        /// </summary>
        private string _phrase = "";
        public string Phrase
        {
            get { return _phrase; }
            set
            {
                _phrase = value;
                if (null != PropertyChanged) PropertyChanged(this, new PropertyChangedEventArgs("Phrase"));
            }
        }

        /// <summary>
        /// Suggestion for phrase to look for
        /// </summary>
        private string _phraseSuggests = "";
        public string PhraseSuggests
        {
            get { return _phraseSuggests; }
            set
            {
                _phraseSuggests = value;
                if (null != PropertyChanged) PropertyChanged(this, new PropertyChangedEventArgs("PhraseSuggests"));
            }
        }

        /// <summary>
        /// Run Add Phrase
        /// </summary>
        private void _run_addPhrase()
        {
            EditProcessor.Instance.PhraseOptions.Add(Phrase + ";" + PhraseSuggests);
            Phrase = "";
            PhraseSuggests = "";
            VisiblePhraseList.Items.Refresh();
        }

        /// <summary>
        /// Add phrase command
        /// </summary>
        private ICommand _addPhrase;
        public ICommand RunAddPhrase
        {
            get { return _addPhrase; }
        }

        /// <summary>
        /// Ignored listbox
        /// </summary>
        public ListBox VisibleIgnoredList { get; internal set; }

        /// <summary>
        /// Phrases listbox
        /// </summary>
        public ListBox VisiblePhraseList { get; internal set; }
    }
}
