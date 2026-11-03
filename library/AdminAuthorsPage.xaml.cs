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
    /// Логика взаимодействия для AdminAuthorsPage.xaml
    /// </summary>
    public partial class AdminAuthorsPage : Page
    {
        public AdminAuthorsPage()
        {
            InitializeComponent();
            LoadAuthors();
        }

        private void LoadAuthors()
        {
            using (var db = new LibraryDBEntities())
            {
                AuthorsGrid.ItemsSource = db.Authors.ToList();
            }
        }

        private void AddAuthor_Click(object sender, RoutedEventArgs e)
        {
            var win = new EditAuthorWindow(null);
            win.Owner = Window.GetWindow(this);
            if (win.ShowDialog() == true)
            {
                LoadAuthors();
            }
        }

        private void EditAuthor_Click(object sender, RoutedEventArgs e)
        {
            var selected = AuthorsGrid.SelectedItem as Author;
            if (selected == null)
            {
                MessageBox.Show("Выберите автора!");
                return;
            }

            var win = new EditAuthorWindow(selected);
            win.Owner = Window.GetWindow(this);
            if (win.ShowDialog() == true)
            {
                LoadAuthors();
            }
        }

        private void DeleteAuthor_Click(object sender, RoutedEventArgs e)
        {
            var selected = AuthorsGrid.SelectedItem as Author;
            if (selected == null)
            {
                MessageBox.Show("Выберите автора!");
                return;
            }

            if (MessageBox.Show("Удалить автора?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                using (var db = new LibraryDBEntities())
                {
                    var toDelete = db.Authors.Find(selected.Id);
                    if (toDelete != null)
                    {
                        db.Authors.Remove(toDelete);
                        db.SaveChanges();
                    }
                }
                LoadAuthors();
            }
        }
    }
}