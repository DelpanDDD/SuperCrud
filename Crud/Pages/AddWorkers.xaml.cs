using Crud.Classes;
using Crud.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
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
    /// Логика взаимодействия для AddWorkers.xaml
    /// </summary>
    public partial class AddWorkers : Page
    {
        public OpenFileDialog ofd = new OpenFileDialog();
        private string newsourthpath = string.Empty;
        string path = "";
        private bool flag = false;
        private workers currentworker = new workers();
        //public DateTime datetoday = DateTime.Now;
        public AddWorkers(workers sellectedWorkers)
        {
            InitializeComponent();
            if (sellectedWorkers != null)
            {
                currentworker = sellectedWorkers;
            }
            DataContext = currentworker;
        }

        private void Save(object sender, RoutedEventArgs e)
        {

            //currentworker.data = datetoday;
            //currentworker.time = datetoday.TimeOfDay;

            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(currentworker.fio))
                errors.AppendLine("Укажите ФИО сотрудника");
            if (string.IsNullOrWhiteSpace(currentworker.login))
                errors.AppendLine("Укажите логин сотрудника");
            if (string.IsNullOrWhiteSpace(currentworker.password))
                errors.AppendLine("Укажите пароль сотрудника");
            if (currentworker.password.Length < 5)
                errors.AppendLine("Пароль меньше 5 символов");
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            if (currentworker.id == 0)
            {
                crudEntities1.GetContext().workers.Add(currentworker);
            }
            DbContextTransaction dbContextTransaction = null;

            try
            {
                if (currentworker.id == 0)
                {
                    crudEntities1.GetContext().workers.Add(currentworker);
                }

                dbContextTransaction = crudEntities1.GetContext().Database.BeginTransaction();

                crudEntities1.GetContext().SaveChanges();

                MessageBox.Show("Информация сохранена!");
                dbContextTransaction.Commit();
                manager.MainFrame.GoBack();
            }
            catch (DbUpdateException ex)
            {
                if (dbContextTransaction != null)
                {
                    dbContextTransaction.Rollback();
                }

                var innerException = ex.InnerException;
                while (innerException != null)
                {
                    MessageBox.Show($"Внутреннее исключение: {innerException.Message}");
                    innerException = innerException.InnerException;
                }

                MessageBox.Show("Ошибка при сохранении изменений. Дополнительные сведения в внутреннем исключении.");
            }
            catch (Exception ex)
            {
                if (dbContextTransaction != null)
                {
                    dbContextTransaction.Rollback();
                }

                MessageBox.Show($"Ошибка при обновлении записей. Дополнительные сведения: {ex.Message}");
            }
            finally
            {
                dbContextTransaction?.Dispose();
            }
        }

        private void Foto(object sender, RoutedEventArgs e)
        {
            string Source = Environment.CurrentDirectory;
            if (ofd.ShowDialog() == true)
            {
                flag = true;
                string sourthpath = ofd.SafeFileName;
                newsourthpath = Source.Replace("/bin/Debug", "/img/") + sourthpath;
                PreviewImage.Source = new BitmapImage(new Uri(ofd.FileName));
                path = ofd.FileName;
                currentworker.photo = $"/img/{ofd.SafeFileName}";
            }
        }
    }
}
