using EstateAgency.Models;
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
using System.Windows.Shapes;

namespace EstateAgency.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddEditClientWindow.xaml
    /// </summary>
    public partial class AddEditClientWindow : Window
    {
        private User _user;
        private Entities _entities;

        public AddEditClientWindow(User user, Entities entities)
        {
            InitializeComponent();

            _user = user;
            _entities = entities;
            
            //Вывод первоначальных данных, если идет редактирование
            if (_user.Id != 0)
            {
                FirstNameInput.Text = _user.FirstName;
                MiddleNameInput.Text = _user.LastName;
                LastNameInput.Text = _user.LastName;
                PhoneInput.Text = _user.Phone;
                EmailInput.Text = _user.Email;
            }
        }

        private void Apply_Click(object sender, RoutedEventArgs e)
        {
            //Проверка, чтобы были заполнены хотя бы или телефон, или почта
            if (PhoneInput.Text == string.Empty &&
                EmailInput.Text == string.Empty)
            {
                MessageBox.Show("Необходимо заполнить хотя бы или телефон, или почту",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            //Проверка почты
            var emailRegex = new Regex(@"^\w+@\w+\.\w+$");
            if (!emailRegex.IsMatch(EmailInput.Text))
            {
                MessageBox.Show("Некорректная почта",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            //Сохранение в БД
            _user.FirstName = FirstNameInput.Text;
            _user.MiddleName = MiddleNameInput.Text;
            _user.LastName = LastNameInput.Text;
            _user.Phone = PhoneInput.Text;
            _user.Email = EmailInput.Text;

            if (_user.Id == 0)
            {
                var id = _entities.User.Max(x => x.Id) + 1;
                _user.Id = id;
                _user.UserTypeId = 3;
                _entities.User.Add(_user);
            }

            _entities.SaveChanges();

            DialogResult = true;
        }

        private void Deny_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
