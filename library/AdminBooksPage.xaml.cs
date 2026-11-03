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
    /// Логика взаимодействия для AdminBooksPage.xaml
    /// </summary>
    public partial class AdminBooksPage : Page
    {
        public AdminBooksPage()
        {
            InitializeComponent();
            LoadGenres();
            LoadBooks();
        }

        private void LoadGenres()
        {
            using (var db = new LibraryDBEntities())
            {
                var genres = db.Books.Select(b => b.Genre).Distinct().OrderBy(g => g).ToList();
                genres.Insert(0, "Все жанры");
                GenreFilter.ItemsSource = genres;
                GenreFilter.SelectedIndex = 0;
            }
        }

        public void LoadBooks()
        {
            string search = SearchBox.Text.Trim();
            string selectedGenre = GenreFilter.SelectedItem as string;

            using (var db = new LibraryDBEntities())
            {
                var query = db.Books.Include(b => b.Author).AsQueryable();

                if (!string.IsNullOrWhiteSpace(search))
                {
                    query = query.Where(b => b.Title.Contains(search));
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
                AdminBooksGrid.ItemsSource = result;

                if (result.Count == 0 && (!string.IsNullOrWhiteSpace(search) || selectedGenre != "Все жанры"))
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
            GenreFilter.SelectedIndex = 0;
            SortBox.SelectedIndex = 0;
            LoadBooks();
        }

        private void AddBook_Click(object sender, RoutedEventArgs e)
        {
            var win = new EditBookWindow(null);
            win.Owner = Window.GetWindow(this);
            if (win.ShowDialog() == true)
            {
                LoadGenres();
                LoadBooks();
            }
        }

        private void EditBook_Click(object sender, RoutedEventArgs e)
        {
            var selected = AdminBooksGrid.SelectedItem as Book;
            if (selected == null)
            {
                MessageBox.Show("Выберите книгу!");
                return;
            }

            var win = new EditBookWindow(selected);
            win.Owner = Window.GetWindow(this);
            if (win.ShowDialog() == true)
            {
                LoadGenres();
                LoadBooks();
            }
        }

        private void DeleteBook_Click(object sender, RoutedEventArgs e)
        {
            var selected = AdminBooksGrid.SelectedItem as Book;
            if (selected == null)
            {
                MessageBox.Show("Выберите книгу!");
                return;
            }

            if (MessageBox.Show("Удалить книгу?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                using (var db = new LibraryDBEntities())
                {
                    var toDelete = db.Books.Find(selected.Id);
                    if (toDelete != null)
                    {
                        db.Books.Remove(toDelete);
                        db.SaveChanges();
                    }
                }
                LoadGenres();
                LoadBooks();
            }
        }
    }
}