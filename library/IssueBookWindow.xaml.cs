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
    /// Логика взаимодействия для IssueBookWindow.xaml
    /// </summary>
    public partial class IssueBookWindow : Window
    {
        public IssueBookWindow()
        {
            InitializeComponent();
            LoadComboBoxes();
        }

        private void LoadComboBoxes()
        {
            using (var db = new LibraryDBEntities())
            {
                BookCombo.ItemsSource = db.Books.ToList();
                ReaderCombo.ItemsSource = db.Readers.ToList();
            }
        }

        private void Issue_Click(object sender, RoutedEventArgs e)
        {
            if (BookCombo.SelectedItem == null || ReaderCombo.SelectedItem == null || IssueDatePicker.SelectedDate == null)
            {
                MessageBox.Show("Заполните все поля!");
                return;
            }

            try
            {
                using (var db = new LibraryDBEntities())
                {
                    var borrow = new BorrowedBook();
                    borrow.BookId = ((Book)BookCombo.SelectedItem).Id;
                    borrow.ReaderId = ((Reader)ReaderCombo.SelectedItem).Id;
                    borrow.BorrowDate = IssueDatePicker.SelectedDate.Value;
                    borrow.ReturnDate = ReturnDatePicker.SelectedDate;

                    db.BorrowedBooks.Add(borrow);
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