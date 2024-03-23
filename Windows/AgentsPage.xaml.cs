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
    /// Логика взаимодействия для AgentsPage.xaml
    /// </summary>
    public partial class AgentsPage : Page
    {
        private Entities _entities = new Entities();

        public AgentsPage()
        {
            InitializeComponent();

            InitializeComponent();

            //Вывод всех риэлторов в дата-грид
            Agents.ItemsSource = _entities.User
                .Where(x => x.UserTypeId == 2)
                .ToList();
        }

        //Вывод риэлторов в соответствии с искомым именем
        private void FindInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            var text = FindInput.Text;
            if (text == string.Empty)
            {
                Agents.ItemsSource = _entities.User
                    .Where(x => x.UserTypeId == 2)
                    .ToList();
            }
            else
            {
                Agents.ItemsSource = _entities.User
                    .Where(x => x.UserTypeId == 2 &&
                           (
                           x.FirstName.Contains(text) ||
                           x.MiddleName.Contains(text) ||
                           x.LastName.Contains(text)
                           ))
                    .ToList();
            }
        }

        //Открытие окна добавления риэлтора
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var addEditAgentWindow = new AddEditAgentWindow(new User(), _entities);
            if (addEditAgentWindow.ShowDialog() == true)
            {
                FindInput_TextChanged(null, null);
            }
        }

        //Открытие окна редактирования риэлтора
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var agent = Agents.SelectedItem as User;

            if (agent == null)
            {
                MessageBox.Show("Сначала выберите агента",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var addEditAgentWindow = new AddEditAgentWindow(agent, _entities);
            if (addEditAgentWindow.ShowDialog() == true)
            {
                FindInput_TextChanged(null, null);
            }
        }

        //Удаление риэлтора
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var agent = Agents.SelectedItem as User;

            if (agent == null)
            {
                MessageBox.Show("Сначала выберите агента",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (agent.Demand.Count == 0 &&
                agent.Demand1.Count == 0 &&
                agent.Offer.Count == 0 &&
                agent.Offer1.Count == 0)
            {
                if (MessageBox.Show("Вы уверены?",
                   "Внимание",
                   MessageBoxButton.YesNo,
                   MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    _entities.User.Remove(agent);
                    _entities.SaveChanges();

                    FindInput_TextChanged(null, null);
                }
            }
            else
            {
                MessageBox.Show("Нельзя удалить этого агента из-за связей в БД",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
