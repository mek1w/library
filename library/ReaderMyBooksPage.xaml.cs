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
    /// Логика взаимодействия для ReaderMyBooksPage.xaml
    /// </summary>
    public partial class ReaderMyBooksPage : Page
    {
        private int _readerId;

        public ReaderMyBooksPage(int readerId)
        {
            InitializeComponent();
            _readerId = readerId;
            LoadMyBooks();
        }

        private void LoadMyBooks()
        {
            using (var db = new LibraryDBEntities())
            {
                var myBooks = db.BorrowedBooks.Include(b => b.Book).Where(b => b.ReaderId == _readerId).ToList();

                MyBooksGrid.ItemsSource = myBooks;
            }
        }
    }
}