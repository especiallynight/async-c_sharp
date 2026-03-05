using EmployeeManagement.Data;
using EmployeeManagement.Models;
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
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    /// 


    public partial class MainWindow : Window
    {
        private DBContext _db;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += async (s, e) => await LoadDataAsync();
        }
        private async Task LoadDataAsync()
        {
            try
            {
                _db = new DBContext();
                await _db.Database.EnsureCreatedAsync();
                await _db.Employees.LoadAsync();
                EmployeesDataGrid.ItemsSource = _db.Employees.Local.ToObservableCollection();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}");
            }
        }
        private async void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var newEmp = new Employee
                {
                    LastName = "",
                    FirstName = "",
                    Patronymic = "",
                    BirthDate = DateTime.Now,
                    Department = "",
                    Address = "",
                    AboutMe = ""
                };

                await _db.Employees.AddAsync(newEmp);
                await _db.SaveChangesAsync();

                EmployeesDataGrid.Items.Refresh();

                MessageBox.Show("Запись успешно добавлена!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении: {ex.Message}");
            }
        }
        private async void EditButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedEmployee = EmployeesDataGrid.SelectedItem as Employee;
                if (selectedEmployee == null)
                {
                    MessageBox.Show("Выберите сотрудника для редактирования", "Внимание",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var employeeCopy = new Employee
                {
                    Id = selectedEmployee.Id,
                    LastName = selectedEmployee.LastName,
                    FirstName = selectedEmployee.FirstName,
                    Patronymic = selectedEmployee.Patronymic,
                    BirthDate = selectedEmployee.BirthDate,
                    Department = selectedEmployee.Department,
                    Address = selectedEmployee.Address,
                    AboutMe = selectedEmployee.AboutMe
                };

                var editWindow = new EditEmployeeWindow(employeeCopy);
                if (editWindow.ShowDialog() == true)
                {
                    selectedEmployee.LastName = employeeCopy.LastName;
                    selectedEmployee.FirstName = employeeCopy.FirstName;
                    selectedEmployee.Patronymic = employeeCopy.Patronymic;
                    selectedEmployee.BirthDate = employeeCopy.BirthDate;
                    selectedEmployee.Department = employeeCopy.Department;
                    selectedEmployee.Address = employeeCopy.Address;
                    selectedEmployee.AboutMe = employeeCopy.AboutMe;

                    await _db.SaveChangesAsync();
                    EmployeesDataGrid.Items.Refresh();

                    MessageBox.Show("Запись успешно обновлена!", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при редактировании: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedEmployee = EmployeesDataGrid.SelectedItem as Employee;
                if (selectedEmployee == null)
                {
                    MessageBox.Show("Выберите сотрудника для удаления", "Внимание",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                var result = MessageBox.Show($"Удалить сотрудника {selectedEmployee.LastName} {selectedEmployee.FirstName}?",
                    "Подтверждение удаления",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    _db.Employees.Remove(selectedEmployee);
                    await _db.SaveChangesAsync();
                    EmployeesDataGrid.Items.Refresh();

                    MessageBox.Show("Запись успешно удалена!", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }    
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении: {ex.Message}");
            }
        }
    }
}
