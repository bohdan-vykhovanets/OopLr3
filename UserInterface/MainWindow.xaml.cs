using Microsoft.Win32;
using System.Text;
using System.Text.Json;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace JsonEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Book>? _books = new();
        private List<Book> _filteredBooks = new();
        private string _currentFilePath = "";

        public MainWindow()
        {
            InitializeComponent();

            this.Closing += MainWindow_Closing;
        }

        private void MainWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            var result = MessageBox.Show(
                "Are you sure you want to exit?",
                "Exit Confirmation",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );

            if (result != MessageBoxResult.Yes)
            {
                e.Cancel = true;
            }
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            // Create an OpenFileDialog
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*",
                Title = "Open JSON File"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                _currentFilePath = openFileDialog.FileName;

                try
                {
                    string jsonContent = File.ReadAllText(_currentFilePath);
                    _books = JsonSerializer.Deserialize<List<Book>>(jsonContent);

                    if (_books != null)
                    {
                        BooksDataGrid.ItemsSource = _books;
                    }
                    else
                    {
                        MessageBox.Show("No books found in the JSON file.", "Empty File", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                catch (JsonException ex)
                {
                    MessageBox.Show($"Invalid JSON format: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {

            if (string.IsNullOrEmpty(_currentFilePath))
            {
                MessageBox.Show("No file is currently open. Please open a file first.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show("Are you sure you want to overwrite the file?", "Confirm Save", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes) return;

            try
            {
                _books = new List<Book>((IEnumerable<Book>)BooksDataGrid.ItemsSource);

                string jsonContent = JsonSerializer.Serialize(_books, new JsonSerializerOptions
                {
                    WriteIndented = true
                });

                File.WriteAllText(_currentFilePath, jsonContent);

                MessageBox.Show("File saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while saving the file: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            if (_books == null || !_books.Any())
            {
                MessageBox.Show("No books available to search. Please load a file first.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string searchTerm = SearchTextBox.Text.Trim();
            if (string.IsNullOrEmpty(searchTerm))
            {
                MessageBox.Show("Please enter a search term.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string selectedAttribute = (SearchByComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

            // Filter the book list based on the selected attribute
            _filteredBooks = selectedAttribute switch
            {
                "Title" => _books.Where(book => book.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList(),
                "Author" => _books.Where(book => book.Author.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList(),
                "Genre" => _books.Where(book => book.Genre.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList(),
                _ => _books
            };

            // Update the DataGrid
            BooksDataGrid.ItemsSource = _filteredBooks;
        }

        private void ClearSearch_Click(object sender, RoutedEventArgs e)
        {
            // Clear search filters and restore the original list
            SearchTextBox.Clear();
            BooksDataGrid.ItemsSource = _books;
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            About about = new About();
            about.Show();
        }

        private void AddBook_Click(object sender, RoutedEventArgs e)
        {
            BookManager bookWindow = new BookManager();
            if (bookWindow.ShowDialog() == true)
            {
                _books.Add(bookWindow.Book); // Add the new book to the list
                BooksDataGrid.ItemsSource = null; // Refresh DataGrid
                BooksDataGrid.ItemsSource = _books;
            }
        }
        private void EditBook_Click(object sender, RoutedEventArgs e)
        {
            if (BooksDataGrid.SelectedItem is Book selectedBook)
            {
                BookManager bookWindow = new BookManager(selectedBook);
                if (bookWindow.ShowDialog() == true)
                {
                    BooksDataGrid.ItemsSource = null; // Refresh DataGrid
                    BooksDataGrid.ItemsSource = _books;
                }
            }
            else
            {
                MessageBox.Show("Please select a book to edit.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void DeleteBook_Click(object sender, RoutedEventArgs e)
        {
            if (BooksDataGrid.SelectedItem is Book selectedBook)
            {
                var result = MessageBox.Show($"Are you sure you want to delete '{selectedBook.Title}'?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    _books.Remove(selectedBook); // Remove the selected book from the list
                    BooksDataGrid.ItemsSource = null; // Refresh DataGrid
                    BooksDataGrid.ItemsSource = _books;
                }
            }
            else
            {
                MessageBox.Show("Please select a book to delete.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

    }
}