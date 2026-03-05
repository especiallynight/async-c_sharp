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

namespace EmployeeManagement
{
    /// <summary>
    /// Логика взаимодействия для EditEmployeeWindow.xaml
    /// </summary>
    public partial class EditEmployeeWindow : Window
    {
        private Employee _employee;
        public EditEmployeeWindow(Employee employee)
        {
            InitializeComponent();
            _employee = employee;

            LastNameTextBox.Text = employee.LastName;
            FirstNameTextBox.Text = employee.FirstName;
            PatronymicTextBox.Text = employee.Patronymic;
            BirthDatePicker.SelectedDate = employee.BirthDate;
            DepartmentTextBox.Text = employee.Department;
            AddressTextBox.Text = employee.Address;
            AboutMeTextBox.Text = employee.AboutMe;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            _employee.LastName = LastNameTextBox.Text;
            _employee.FirstName = FirstNameTextBox.Text;
            _employee.Patronymic = PatronymicTextBox.Text;
            _employee.BirthDate = BirthDatePicker.SelectedDate ?? DateTime.Now;
            _employee.Department = DepartmentTextBox.Text;
            _employee.Address = AddressTextBox.Text;
            _employee.AboutMe = AboutMeTextBox.Text;

            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
