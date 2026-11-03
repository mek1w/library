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
using System.Windows.Shapes;

namespace library
{
    /// <summary>
    /// Логика взаимодействия для EditAuthorWindow.xaml
    /// </summary>
 
        public partial class EditAuthorWindow : Window
        {
            private Author _editAuthor;

            public EditAuthorWindow(Author author = null)
            {
                InitializeComponent();
                _editAuthor = author;

                if (_editAuthor != null)
                {
                    HeaderText.Text = "Редактирование автора";
                    NameBox.Text = _editAuthor.Name;
                    BirthDatePicker.SelectedDate = _editAuthor.DateOfBirth;
                }
                else
                {
                    HeaderText.Text = "Добавление автора";
                }
            }

            private void Save_Click(object sender, RoutedEventArgs e)
            {
                try
                {
                    using (var db = new LibraryDBEntities())
                    {
                        if (_editAuthor == null)
                        {
                            var newAuthor = new Author();
                            newAuthor.Name = NameBox.Text;
                            newAuthor.DateOfBirth = BirthDatePicker.SelectedDate.Value;
                            db.Authors.Add(newAuthor);
                        }
                        else
                        {
                            var authorToUpdate = db.Authors.Find(_editAuthor.Id);
                            authorToUpdate.Name = NameBox.Text;
                            authorToUpdate.DateOfBirth = BirthDatePicker.SelectedDate.Value;
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
