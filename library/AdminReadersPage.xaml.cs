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
    /// Логика взаимодействия для AdminReadersPage.xaml
    /// </summary>
    public partial class AdminReadersPage : Page
    {
        public AdminReadersPage()
        {
            InitializeComponent();
            LoadReaders();
        }

        private void LoadReaders()
        {
            using (var db = new LibraryDBEntities())
            {
                ReadersGrid.ItemsSource = db.Readers.ToList();
            }
        }

        private void AddReader_Click(object sender, RoutedEventArgs e)
        {
            var win = new EditReaderWindow(null);
            win.Owner = Window.GetWindow(this);
            if (win.ShowDialog() == true)
            {
                LoadReaders();
            }
        }

        private void EditReader_Click(object sender, RoutedEventArgs e)
        {
            var selected = ReadersGrid.SelectedItem as Reader;
            if (selected == null)
            {
                MessageBox.Show("Выберите читателя!");
                return;
            }

            var win = new EditReaderWindow(selected);
            win.Owner = Window.GetWindow(this);
            if (win.ShowDialog() == true)
            {
                LoadReaders();
            }
        }
        private void DeleteReader_Click(object sender, RoutedEventArgs e)
        {
            var selected = ReadersGrid.SelectedItem as Reader;
            if (selected == null)
            {
                MessageBox.Show("Выберите читателя!");
                return;
            }

            if (MessageBox.Show("Удалить читателя?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    using (var db = new LibraryDBEntities())
                    {
                        var borrowsToDelete = db.BorrowedBooks.Where(b => b.ReaderId == selected.Id).ToList();
                        if (borrowsToDelete.Any())
                        {
                            db.BorrowedBooks.RemoveRange(borrowsToDelete);
                        }

                        var toDelete = db.Readers.Find(selected.Id);
                        if (toDelete != null)
                        {
                            db.Readers.Remove(toDelete);
                        }

                        db.SaveChanges();
                    }

                    LoadReaders();
                    MessageBox.Show("Читатель успешно удален!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при удалении: " + ex.Message);
                }
            }
        }


    }
}
    