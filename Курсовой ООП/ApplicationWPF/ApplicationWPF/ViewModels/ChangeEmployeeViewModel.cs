using ApplicationWPF.Commands;
using ApplicationWPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ApplicationWPF.ViewModels
{
	class ChangeEmployeeViewModel : ViewModelBase
	{
		private readonly MyDbContext _dbContext = new MyDbContext();
		private EMPLOYEES _selectedEmployees;

		public int IdLog
		{
			get => _selectedEmployees.ID_LOGPAS;
			set
			{
				if (_selectedEmployees != null && value >= 0)
				{
					_selectedEmployees.ID_LOGPAS = value;
					OnPropertyChanged(nameof(IdLog));
				}
				else
				{
					MessageBox.Show("Не может быть отрицательного числа для данного поля!.");
				}
			}
		}

		public string LastNam
		{
			get => _selectedEmployees.LASTNAME_EMPLOYEE;
			set
			{
				if (_selectedEmployees != null)
				{
					_selectedEmployees.LASTNAME_EMPLOYEE = value;
					OnPropertyChanged(nameof(LastNam));
				}
			}
		}

		public string FirstNam
		{
			get => _selectedEmployees.FIRSTNAME_EMPLOYEE;
			set
			{
				if (_selectedEmployees != null)
				{
					_selectedEmployees.FIRSTNAME_EMPLOYEE = value;
					OnPropertyChanged(nameof(FirstNam));
				}
			}
		}

		public string Phone
		{
			get => _selectedEmployees.PHONENAME_EMPLOYEE;
			set
			{
				if (Regex.IsMatch(value, @"^\+375\d{9}$"))
				{
					_selectedEmployees.PHONENAME_EMPLOYEE = value;
					OnPropertyChanged(nameof(Phone));
				}
			}
		}

		public int Salary
		{
			get => _selectedEmployees.SALARY_EMPLOYEE;
			set
			{
				if (_selectedEmployees != null && value >= 0)
				{
					_selectedEmployees.SALARY_EMPLOYEE = value;
					OnPropertyChanged(nameof(Salary));
				}
				else
				{
					MessageBox.Show("Не может быть отрицательного числа для данного поля!.");
				}
			}
		}

		public ICommand ExitCommand { get; private set; }
		public ICommand EditAbonementCommand { get; private set; }

        public ChangeEmployeeViewModel()
        {
            
        }

        public ChangeEmployeeViewModel(EMPLOYEES selectedEmployees)
        {
			_selectedEmployees = selectedEmployees;
			ExitCommand = new RelayCommand(parameter => ExitApp());
			EditAbonementCommand = new RelayCommand(parameter => EditEmployee());
			IdLog = _selectedEmployees.ID_LOGPAS;
			LastNam = _selectedEmployees.LASTNAME_EMPLOYEE;
			FirstNam = _selectedEmployees.FIRSTNAME_EMPLOYEE;
			Phone = _selectedEmployees.PHONENAME_EMPLOYEE;
			Salary = _selectedEmployees.SALARY_EMPLOYEE; 
		}

		private void ExitApp()
		{
			CloseWindow();
		}

		private void EditEmployee()
		{
			try
			{
				var Employee = _dbContext.EMPLOYEES.Find(_selectedEmployees.ID_EMPLOYEE);

				if (Employee != null)
				{
					Employee.ID_LOGPAS = _selectedEmployees.ID_LOGPAS;
					Employee.LASTNAME_EMPLOYEE = _selectedEmployees.LASTNAME_EMPLOYEE;
					Employee.FIRSTNAME_EMPLOYEE = _selectedEmployees.FIRSTNAME_EMPLOYEE;
					Employee.PHONENAME_EMPLOYEE = _selectedEmployees.PHONENAME_EMPLOYEE;
					Employee.SALARY_EMPLOYEE = _selectedEmployees.SALARY_EMPLOYEE;
					_dbContext.SaveChanges();
					MessageBox.Show("Сотрудник успешно изменен!");
					CloseWindow();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка при изменении сотрудника: {ex.Message}");
			}
		}

		private void CloseWindow()
		{
			foreach (Window window in App.Current.Windows)
			{
				if (window.DataContext == this)
				{
					window.Close();
					break;
				}
			}
		}
	}
}
