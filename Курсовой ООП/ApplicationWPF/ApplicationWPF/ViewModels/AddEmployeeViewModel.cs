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
	class AddEmployeeViewModel : ViewModelBase
	{
		private int _idlog;
		private string _last;
		private string _first;
		private string _phone;
		private int _salary;

		public int IdLog
		{
			get => _idlog;
			set
			{
				if (value >= 0)
				{
					_idlog = value;
					OnPropertyChanged(nameof(IdLog));
				}
			}
		}

		public string LastNam
		{
			get => _last;
			set
			{
				_last = value;
				OnPropertyChanged(nameof(LastNam));
			}
		}

		public string FirstNam
		{
			get => _first;
			set
			{
				_first = value;
				OnPropertyChanged(nameof(FirstNam));
			}
		}

		public string Phone
		{
			get => _phone;
			set
			{
				if (Regex.IsMatch(value, @"^\+375\d{9}$"))
				{
					_phone = value;
					OnPropertyChanged(nameof(Phone));
				}
			}
		}

		public int Salary
		{
			get => _salary;
			set
			{
				if (value >= 0)
				{
					_salary = value;
					OnPropertyChanged(nameof(Salary));
				}
			}
		}

		public ICommand ExitCommand { get; private set; }
		public ICommand AddEmployeeCommand { get; private set; }

		public AddEmployeeViewModel()
		{
			ExitCommand = new RelayCommand(parameter => ExitApp());
			AddEmployeeCommand = new RelayCommand(parameter => AddEmpl());
		}

		private void AddEmpl()
		{
			try
			{
				using (var dbContext = new MyDbContext())
				{
					var newEmpl = new EMPLOYEES
					{
						ID_LOGPAS = _idlog,
						LASTNAME_EMPLOYEE = _last,
						FIRSTNAME_EMPLOYEE = _first,
						PHONENAME_EMPLOYEE = _phone,
						SALARY_EMPLOYEE = _salary,
					};

					dbContext.EMPLOYEES.Add(newEmpl);
					dbContext.SaveChanges();
					MessageBox.Show("Сотрудник успешно добавлен!");
					CloseWindow();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка при добавлении сотрудника: {ex.Message}");
			}
		}
		private void ExitApp()
		{
			CloseWindow();
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
