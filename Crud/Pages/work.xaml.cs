using Crud.Classes;
using Crud.Models;
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

namespace Crud.Pages
{
    /// <summary>
    /// Логика взаимодействия для work.xaml
    /// </summary>
    public partial class work : Page
    {
        public work()
        {
            InitializeComponent();
            BDWorkers.ItemsSource = crudEntities1.GetContext().workers.ToList();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            manager.MainFrame.Navigate(new AddWorkers((sender as Button).DataContext as workers));
        }

        private void Add(object sender, RoutedEventArgs e)
        {
            manager.MainFrame.Navigate(new AddWorkers(null));
        }

        private void Delete(object sender, RoutedEventArgs e)
        {
            var workersDell = BDWorkers.SelectedItems.Cast<workers>().ToList();

            if (MessageBox.Show($"Вы точно хотите удалить следующие {workersDell.Count()} элементов?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    crudEntities1.GetContext().workers.RemoveRange(workersDell);
                    crudEntities1.GetContext().SaveChanges();
                    MessageBox.Show("Данные удалены!");

                    BDWorkers.ItemsSource = crudEntities1.GetContext().workers.ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }
    }
}
