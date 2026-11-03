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
    /// Логика взаимодействия для ReaderWindow.xaml
    /// </summary>
    public partial class ReaderWindow : Window
    {
        public int CurrentReaderId { get; private set; }

        public ReaderWindow(string fullName, int readerId)
        {
            InitializeComponent();
            CurrentReaderId = readerId;
            WelcomeText.Text = $"Привет,\n{fullName}!";

            CheckOverdueBooks();

            ReaderFrame.Navigate(new ReaderCatalogPage(CurrentReaderId));
        }

        private void CheckOverdueBooks()
        {
            using (var db = new LibraryDBEntities())
            {
                DateTime today = DateTime.Now;
                int overdueCount = db.BorrowedBooks
                                     .Where(b => b.ReaderId == CurrentReaderId && b.ReturnDate < today)
                                     .Count();

                if (overdueCount > 0)
                {
                    MyBooksButton.Content = $"Мои книги ({overdueCount})";
                    MyBooksButton.Background = Brushes.LightCoral;
                }
                else
                {
                    MyBooksButton.Content = "Мои книги";
                    MyBooksButton.Background = null;
                }
            }
        }

        private void ShowCatalog_Click(object sender, RoutedEventArgs e)
        {
            ReaderFrame.Navigate(new ReaderCatalogPage(CurrentReaderId));
        }

        private void ShowMyBooks_Click(object sender, RoutedEventArgs e)
        {
            ReaderFrame.Navigate(new ReaderMyBooksPage(CurrentReaderId));
            CheckOverdueBooks();
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            var login = new MainWindow();
            Application.Current.MainWindow = login;
            login.Show();
            Close();
        }
    }
}