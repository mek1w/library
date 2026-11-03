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

namespace library
{
    /// <summary>
    /// Логика взаимодействия для EditBookWindow.xaml
    /// </summary>
    public partial class EditBookWindow : Window
    {
        private Book _editBook;

        public EditBookWindow(Book book = null)
        {
            InitializeComponent();
            _editBook = book;

            if (_editBook != null)
            {
                HeaderText.Text = "Редактирование книги";
                TitleBox.Text = _editBook.Title;
                YearBox.Text = _editBook.PublicationYear.ToString();
                GenreBox.Text = _editBook.Genre;
                PagesBox.Text = _editBook.NumberOfPages.ToString();

                using (var db = new LibraryDBEntities())
                {
                    var author = db.Authors.Find(_editBook.AuthorId);
                    if (author != null) AuthorBox.Text = author.Name;
                }
            }
            else
            {
                HeaderText.Text = "Добавление книги";
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(TitleBox.Text) ||
                    string.IsNullOrWhiteSpace(YearBox.Text) ||
                    string.IsNullOrWhiteSpace(PagesBox.Text) ||
                    string.IsNullOrWhiteSpace(AuthorBox.Text))
                {
                    MessageBox.Show("Заполните все обязательные поля!");
                    return;
                }

                int year;
                int pages;
                if (!int.TryParse(YearBox.Text, out year) || !int.TryParse(PagesBox.Text, out pages))
                {
                    MessageBox.Show("Год и количество страниц должны быть числами!");
                    return;
                }

                using (var db = new LibraryDBEntities())
                {
                    string authorName = AuthorBox.Text.Trim();

                    var author = db.Authors.FirstOrDefault(a => a.Name == authorName);

                    if (author == null)
                    {
                        author = new Author();
                        author.Name = authorName;
                        author.DateOfBirth = DateTime.Now;
                        db.Authors.Add(author);
                        db.SaveChanges();
                    }

                    if (_editBook == null)
                    {
                        var newBook = new Book();
                        newBook.Title = TitleBox.Text;
                        newBook.AuthorId = author.Id;
                        newBook.PublicationYear = year;
                        newBook.Genre = GenreBox.Text;
                        newBook.NumberOfPages = pages;
                        db.Books.Add(newBook);
                    }
                    else
                    {
                        var bookToUpdate = db.Books.Find(_editBook.Id);
                        bookToUpdate.Title = TitleBox.Text;
                        bookToUpdate.AuthorId = author.Id;
                        bookToUpdate.PublicationYear = year;
                        bookToUpdate.Genre = GenreBox.Text;
                        bookToUpdate.NumberOfPages = pages;
                    }
                    db.SaveChanges();
                }
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}