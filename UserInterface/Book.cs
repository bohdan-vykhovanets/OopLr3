using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonEditor
{
    public class Book
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Publisher { get; set; }
        
        public Book() { }
        public Book(string title, string author, string genre, DateTime yearOfRelease, string publisher)
        {
            Title = title;
            Author = author;
            Genre = genre;
            ReleaseDate = yearOfRelease;
            Publisher = publisher;
        }
    }
}
