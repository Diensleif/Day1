using EstateAgency.Models;
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

namespace EstateAgency.Windows
{
    /// <summary>
    /// Логика взаимодействия для ClientsPage.xaml
    /// </summary>
    public partial class ClientsPage : Page
    {
        private Entities _entities = new Entities();

        public ClientsPage()
        {
            InitializeComponent();

            //Вывод всех клиентов в дата-грид
            Clients.ItemsSource = _entities.User
                .Where(x => x.UserTypeId == 3)
                .ToList();
        }

        //Вывод клиентов в соответствии с искомым именем
        private void FindInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            var text = FindInput.Text;
            if (text == string.Empty)
            {
                Clients.ItemsSource = _entities.User
                    .Where(x => x.UserTypeId == 3)
                    .ToList();
            }
            else
            {
                Clients.ItemsSource = _entities.User
                    .Where(x => x.UserTypeId == 3 &&
                           (
                           x.FirstName.Contains(text) ||
                           x.MiddleName.Contains(text) ||
                           x.LastName.Contains(text)
                           ))
                    .ToList();
            }
        }

        //Открытие окна добавления клиента
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var addEditClientWindow = new AddEditClientWindow(new User(), _entities);
            if (addEditClientWindow.ShowDialog() == true)
            {
                FindInput_TextChanged(null, null);
            }
        }

        //Открытие окна редактирования клиента
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var client = Clients.SelectedItem as User;

            if (client == null)
            {
                MessageBox.Show("Сначала выберите клиента",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var addEditClientWindow = new AddEditClientWindow(client, _entities);
            if (addEditClientWindow.ShowDialog() == true)
            {
                FindInput_TextChanged(null, null);
            }
        }

        //Удаление клиента
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var client = Clients.SelectedItem as User;

            if (client == null)
            {
                MessageBox.Show("Сначала выберите клиента",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (client.Demand.Count == 0 &&
                client.Demand1.Count == 0 &&
                client.Offer.Count == 0 &&
                client.Offer1.Count == 0)
            {
                if (MessageBox.Show("Вы уверены?",
                   "Внимание",
                   MessageBoxButton.YesNo,
                   MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    _entities.User.Remove(client);
                    _entities.SaveChanges();

                    FindInput_TextChanged(null, null);
                }
            }
            else
            {
                MessageBox.Show("Нельзя удалить этого клиента из-за связей в БД",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
