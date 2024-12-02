using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace JsonEditor
{
    public partial class BookManager : Window
    {
        public Book Book { get; private set; } // To store the book being edited or added

        public BookManager(Book book = null)
        {
            InitializeComponent();

            if (book != null)
            {
                // Populate fields if editing an existing book
                Book = book;
                TitleTextBox.Text = book.Title;
                AuthorTextBox.Text = book.Author;
                GenreTextBox.Text = book.Genre;
                ReleaseDatePicker.SelectedDate = book.ReleaseDate;
                PublisherTextBox.Text = book.Publisher;
            }
            else
            {
                Book = new Book();
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Save data from the form to the Book object
            Book.Title = TitleTextBox.Text;
            Book.Author = AuthorTextBox.Text;
            Book.Genre = GenreTextBox.Text;
            Book.ReleaseDate = ReleaseDatePicker.SelectedDate ?? default;
            Book.Publisher = PublisherTextBox.Text;

            DialogResult = true; // Indicate success
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false; // Indicate cancellation
            Close();
        }
    }
}
