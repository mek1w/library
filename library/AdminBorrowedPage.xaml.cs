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
    /// Логика взаимодействия для AdminBorrowedPage.xaml
    /// </summary>
    public partial class AdminBorrowedPage : Page
    {
        public AdminBorrowedPage()
        {
            InitializeComponent();
            LoadBorrowed();
        }

        public void LoadBorrowed()
        {
            using (var db = new LibraryDBEntities())
            {
                var borrowed = db.BorrowedBooks
                                 .Include(b => b.Book)
                                 .Include(b => b.Reader)
                                 .ToList();

                BorrowedGrid.ItemsSource = borrowed;
            }
        }

        private void IssueBook_Click(object sender, RoutedEventArgs e)
        {
            var win = new IssueBookWindow();
            win.Owner = Window.GetWindow(this);
            if (win.ShowDialog() == true)
            {
                LoadBorrowed();
            }
        }

        private void ReturnBook_Click(object sender, RoutedEventArgs e)
        {
            var selected = BorrowedGrid.SelectedItem as BorrowedBook;
            if (selected == null)
            {
                MessageBox.Show("Выберите запись для возврата!");
                return;
            }

            try
            {
                using (var db = new LibraryDBEntities())
                {
                    var toUpdate = db.BorrowedBooks.Find(selected.Id);
                    if (toUpdate != null)
                    {
                        toUpdate.ReturnDate = DateTime.Now;
                        db.SaveChanges();
                    }
                }
                LoadBorrowed();
                MessageBox.Show("Книга успешно возвращена!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при возврате: " + ex.Message);
            }
        }
    }
}