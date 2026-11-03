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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.Entity;

namespace library
{
    /// <summary>
    /// Логика взаимодействия для ReaderCatalogPage.xaml
    /// </summary>
    public partial class ReaderCatalogPage : Page
    {
        private int _readerId;

        public ReaderCatalogPage(int readerId)
        {
            InitializeComponent();
            _readerId = readerId;
            LoadFilters();
            LoadBooks();
        }

        private void LoadFilters()
        {
            using (var db = new LibraryDBEntities())
            {
                var authors = db.Authors.Select(a => a.Name).OrderBy(a => a).ToList();
                authors.Insert(0, "Все авторы");
                AuthorFilter.ItemsSource = authors;
                AuthorFilter.SelectedIndex = 0;

                var genres = db.Books.Select(b => b.Genre).Distinct().OrderBy(g => g).ToList();
                genres.Insert(0, "Все жанры");
                GenreFilter.ItemsSource = genres;
                GenreFilter.SelectedIndex = 0;
            }
        }

        private void LoadBooks()
        {
            string search = SearchBox.Text.Trim();
            string selectedAuthor = AuthorFilter.SelectedItem as string;
            string selectedGenre = GenreFilter.SelectedItem as string;

            using (var db = new LibraryDBEntities())
            {
                var query = db.Books.Include(b => b.Author).AsQueryable();

                if (!string.IsNullOrWhiteSpace(search))
                {
                    query = query.Where(b => b.Title.Contains(search));
                }

                if (!string.IsNullOrEmpty(selectedAuthor) && selectedAuthor != "Все авторы")
                {
                    query = query.Where(b => b.Author.Name == selectedAuthor);
                }

                if (!string.IsNullOrEmpty(selectedGenre) && selectedGenre != "Все жанры")
                {
                    query = query.Where(b => b.Genre == selectedGenre);
                }

                if (SortBox.SelectedIndex == 1)
                    query = query.OrderByDescending(b => b.PublicationYear);
                else if (SortBox.SelectedIndex == 2)
                    query = query.OrderBy(b => b.PublicationYear);
                else if (SortBox.SelectedIndex == 3)
                    query = query.OrderByDescending(b => b.NumberOfPages);
                else if (SortBox.SelectedIndex == 4)
                    query = query.OrderBy(b => b.NumberOfPages);
                else
                    query = query.OrderBy(b => b.Title);

                var result = query.ToList();
                BooksGrid.ItemsSource = result;

                if (result.Count == 0 && (!string.IsNullOrWhiteSpace(search) || selectedAuthor != "Все авторы" || selectedGenre != "Все жанры"))
                {
                    MessageBox.Show("Книги по вашему запросу не найдены.", "Поиск");
                }
            }
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (IsLoaded) LoadBooks();
        }

        private void FilterChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsLoaded) LoadBooks();
        }

        private void ResetFilter_Click(object sender, RoutedEventArgs e)
        {
            SearchBox.Clear();
            AuthorFilter.SelectedIndex = 0;
            GenreFilter.SelectedIndex = 0;
            SortBox.SelectedIndex = 0;
            LoadBooks();
        }

        private void BorrowBook_Click(object sender, RoutedEventArgs e)
        {
            var selectedBook = BooksGrid.SelectedItem as Book;
            if (selectedBook == null)
            {
                MessageBox.Show("Выберите книгу из списка!");
                return;
            }

            try
            {
                using (var db = new LibraryDBEntities())
                {
                    var newBorrow = new BorrowedBook();
                    newBorrow.BookId = selectedBook.Id;
                    newBorrow.ReaderId = _readerId;
                    newBorrow.BorrowDate = DateTime.Now;
                    newBorrow.ReturnDate = DateTime.Now.AddDays(14);

                    db.BorrowedBooks.Add(newBorrow);
                    db.SaveChanges();
                }
                MessageBox.Show("Книга успешно взята!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }
    }
}