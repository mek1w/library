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
    /// Логика взаимодействия для EditReaderWindow.xaml
    /// </summary>
    public partial class EditReaderWindow : Window
    {
        private Reader _editReader;

        public EditReaderWindow(Reader reader = null)
        {
            InitializeComponent();
            _editReader = reader;

            if (_editReader != null)
            {
                HeaderText.Text = "Редактирование читателя";
                FullNameBox.Text = _editReader.FullName;
                BirthDatePicker.SelectedDate = _editReader.DateOfBirth;
                PhoneBox.Text = _editReader.PhoneNumber;
                EmailBox.Text = _editReader.Email;
                PasswordBox.Password = _editReader.Password;
            }
            else
            {
                HeaderText.Text = "Добавление читателя";
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var db = new LibraryDBEntities())
                {
                    if (_editReader == null)
                    {
                        var newReader = new Reader();
                        newReader.FullName = FullNameBox.Text;
                        newReader.DateOfBirth = BirthDatePicker.SelectedDate.Value;
                        newReader.PhoneNumber = PhoneBox.Text;
                        newReader.Email = EmailBox.Text;
                        newReader.Password = PasswordBox.Password;
                        newReader.Role = 2;
                        db.Readers.Add(newReader);
                    }
                    else
                    {
                        var readerToUpdate = db.Readers.Find(_editReader.Id);
                        readerToUpdate.FullName = FullNameBox.Text;
                        readerToUpdate.DateOfBirth = BirthDatePicker.SelectedDate.Value;
                        readerToUpdate.PhoneNumber = PhoneBox.Text;
                        readerToUpdate.Email = EmailBox.Text;
                        readerToUpdate.Password = PasswordBox.Password;
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