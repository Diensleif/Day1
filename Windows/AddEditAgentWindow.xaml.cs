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
    /// Логика взаимодействия для AddEditAgentWindow.xaml
    /// </summary>
    public partial class AddEditAgentWindow : Window
    {
        private User _user;
        private Entities _entities;

        public AddEditAgentWindow(User user, Entities entities)
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
                DealShareInput.Text = _user.DealShare.ToString();
            }
        }

        private void Apply_Click(object sender, RoutedEventArgs e)
        {
            //Проверка комиссии
            int dealShare;
            try
            {
                dealShare = Convert.ToInt32(DealShareInput.Text);
                if (dealShare <= 0 || dealShare >= 100)
                {
                    MessageBox.Show("Комиссия должна лежать в диапазоне от 1 до 99",
                        "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            catch
            {
                MessageBox.Show("Некорректная комиссия",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            //Сохранение в БД
            _user.FirstName = FirstNameInput.Text;
            _user.MiddleName = MiddleNameInput.Text;
            _user.LastName = LastNameInput.Text;
            _user.DealShare = dealShare;

            if (_user.Id == 0)
            {
                var id = _entities.User.Max(x => x.Id) + 1;
                _user.Id = id;
                _user.UserTypeId = 2;
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
