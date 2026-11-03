using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            bool hasErrors = false;

            string fullName = FullNameBox.Text.Trim();
            string email = EmailBox.Text.Trim();
            string password = PasswordBox.Password;
            string confirmPassword = ConfirmPasswordBox.Password;

            if (fullName.Length < 5 || fullName.Length > 100)
            {
                MessageBox.Show("ФИО должно быть от 5 до 100 символов");
                hasErrors = true;
            }

            if (string.IsNullOrWhiteSpace(email) || email.Contains(" ") ||
                !Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                MessageBox.Show("Неверный формат электронной почты");
                hasErrors = true;
            }

            if (password.Length < 8 || password.Length > 128 ||
                !password.Any(char.IsUpper) || !password.Any(char.IsLower) || !password.Any(char.IsDigit))
            {
                MessageBox.Show("Пароль должен быть от 8 до 128 символов, содержать заглавную, строчную букву и цифру");
                hasErrors = true;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Пароли не совпадают");
                hasErrors = true;
            }

            if (hasErrors) return;

            using (var db = new LibraryDBEntities())
            {
                bool emailExists = db.Readers.Any(u => u.Email == email);
                if (emailExists)
                {
                    MessageBox.Show("Пользователь с такой почтой уже существует");
                    return;
                }

                var newReader = new Reader();
                newReader.FullName = fullName;
                newReader.Email = email;
                newReader.Password = password;
                newReader.Role = 2;
                newReader.DateOfBirth = DateTime.Now;
                newReader.PhoneNumber = "Не указан";

                db.Readers.Add(newReader);
                db.SaveChanges();
            }

            MessageBox.Show("Регистрация успешна!");
            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}