﻿namespace NaNoE.V2.Data
{
    /// <summary>
    /// Novel Items
    /// </summary>
    public class NItem
    {
        /// <summary>
        /// Private references
        /// </summary>
        private ControlType _controlType;
        private int _id;
        private string _data;

        /// <summary>
        /// Type of control
        ///  e.g. Paragraphs
        /// </summary>
        public ControlType CType
        {
            get { return _controlType; }
        }

        /// <summary>
        /// ID in data
        /// </summary>
        public int ID
        {
            get { return _id; }
        }

        /// <summary>
        /// Data of Item
        ///  e.g. Paragraph has 'This is a sentence about cake. It can be multiple lines, you know, just like a normal paragraph.'
        /// </summary>
        public string Data
        {
            get { return _data; }
        }

        /// <summary>
        /// Creation of a novel item
        /// </summary>
        /// <param name="controlType">Type of the item (paragraph, note, bookmark, chapter)</param>
        /// <param name="id">The ID in the data</param>
        /// <param name="data">The text within that element</param>
        public NItem(ControlType controlType, int id, string data)
        {
            _controlType = controlType;
            _id = id;
            _data = data;
        }

        /// <summary>
        /// Set the data in an NItem, it makes sure it fits the formats we want for them
        ///  - Chapters you cant
        ///  - Bookmarks you can only go up to 25 length
        /// </summary>
        /// <param name="data">The updated data</param>
        /// <returns>Bool for if it changed in memory or not</returns>
        public bool SetData(string data)
        {
            if (_controlType == ControlType.Chapter)
            {
                return false;
            }
            else if (_controlType == ControlType.Bookmark)
            {
                if (data.Length > 25) return false;
            }

            _data = data;
            return true;
        }
    }

    /// <summary>
    /// Data type for the NItem type elements
    /// </summary>
    public enum ControlType
    {
        Chapter,
        Paragraph,
        Note,
        Bookmark
    }
}
