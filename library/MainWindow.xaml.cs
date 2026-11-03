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
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string email = EmailBox.Text.Trim();
            string password = PasswordBox.Password;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Введите email и пароль!");
                return;
            }

            using (var db = new LibraryDBEntities())
            {
                var user = db.Readers.FirstOrDefault(u => u.Email == email && u.Password == password);

                if (user == null)
                {
                    MessageBox.Show("Неверный email или пароль");
                    return;
                }

                if (user.Role == 1) 
                {
                    var admin = new AdminWindow();
                    Application.Current.MainWindow = admin;
                    admin.Show();
                }
                else 
                {
                    var reader = new ReaderWindow(user.FullName, user.Id);
                    Application.Current.MainWindow = reader;
                    reader.Show();
                }
                Close();
            }
        }

        private void OpenRegister_Click(object sender, RoutedEventArgs e)
        {
            var reg = new RegisterWindow();
            reg.Owner = this;
            reg.ShowDialog();
        }
    }
}
