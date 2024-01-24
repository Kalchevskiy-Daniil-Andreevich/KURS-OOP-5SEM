using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using ApplicationWPF.Commands;
using System.Collections.ObjectModel;
using ApplicationWPF.Models;
using ApplicationWPF.Views;

namespace ApplicationWPF.ViewModels
{
    class AdminEmployeeViewModel : ViewModelBase
    {
		private ObservableCollection<EMPLOYEES> _employees;
		private EMPLOYEES _selectedEmployee;
		private string _searchText;

		public EMPLOYEES SelectedEmployee
		{
			get => _selectedEmployee;
			set
			{
				_selectedEmployee = value;
				OnPropertyChanged(nameof(SelectedEmployee));
			}
		}

		public ObservableCollection<EMPLOYEES> Employees
		{
			get => _employees;
			set
			{
				_employees = value;
				OnPropertyChanged(nameof(Employees));
			}
		}

		public string SearchText
		{
			get => _searchText;
			set { _searchText = value; OnPropertyChanged(nameof(SearchText)); }
		}

		private Visibility _buttonOpenMenuVisibility = Visibility.Visible;
		private Visibility _buttonCloseMenuVisibility = Visibility.Collapsed;

		public Visibility ButtonOpenMenuVisibility
		{
			get => _buttonOpenMenuVisibility;
			set
			{
				_buttonOpenMenuVisibility = value;
				OnPropertyChanged();
			}
		}

		public Visibility ButtonCloseMenuVisibility
		{
			get => _buttonCloseMenuVisibility;
			set
			{
				_buttonCloseMenuVisibility = value;
				OnPropertyChanged();
			}
		}

		public AdminEmployeeViewModel()
		{
			ExitCommand = new RelayCommand(parameter => ExitApp());
			ButtonOpenMenuCommand = new RelayCommand(parameter => ButtonOpenMenu_Click());
			ButtonCloseMenuCommand = new RelayCommand(parameter => ButtonCloseMenu_Click());
			ToAdmiAbon = new RelayCommand(parameter => ToAdminAb());
			DeleteEmployeetCommand = new RelayCommand(DeleteEmployees);
			SearchCommand = new RelayCommand(Search);
			AddEmployeesCommand = new RelayCommand(AddEmployess);
			EditEmployeesCommand = new RelayCommand(EditAbonements);
			ToClientsApp = new RelayCommand(parameter => Adminclient());
			ToRequestApp = new RelayCommand(parameter => Adminrequest());

			Employees = new ObservableCollection<EMPLOYEES>();
			LoadEmployees();
		}

		private void LoadEmployees()
		{
			using (var dbContext = new MyDbContext())
			{
				Employees = new ObservableCollection<EMPLOYEES>(dbContext.EMPLOYEES.ToList());
			}
		}

		public ICommand ExitCommand { get; private set; }
		public ICommand ButtonOpenMenuCommand { get; private set; }
		public ICommand ButtonCloseMenuCommand { get; private set; }
		public ICommand ToAdmiAbon { get; private set; }
		public ICommand DeleteEmployeetCommand { get; private set; }
		public ICommand SearchCommand { get; private set; }
		public ICommand AddEmployeesCommand { get; private set; }
		public ICommand EditEmployeesCommand { get; private set; }
		public ICommand ToClientsApp { get; private set; }
		public ICommand ToRequestApp { get; private set; }

		private void ToAdminAb()
		{
			AdministratorPanel administratorPanel = new AdministratorPanel();
			administratorPanel.Show();
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

		private void Adminclient()
		{
			AdminClient adminClient = new AdminClient();
			adminClient.ShowDialog();
			CloseWindow();
		}

		private void Adminrequest()
		{
			AdminConfirmRequest adminConfirmRequest = new AdminConfirmRequest();
			adminConfirmRequest.ShowDialog();
			CloseWindow();
		}

		private void ExitApp()
		{
			Login login = new Login();
			login.Show();
			CloseWindow();
		}

		private void ButtonOpenMenu_Click()
		{
			ButtonOpenMenuVisibility = Visibility.Collapsed;
			ButtonCloseMenuVisibility = Visibility.Visible;
		}

		private void ButtonCloseMenu_Click()
		{
			ButtonOpenMenuVisibility = Visibility.Visible;
			ButtonCloseMenuVisibility = Visibility.Collapsed;
		}

		private void AddEmployess(object parameter)
		{
			var addEmployeeViewModel = new AddEmployeeViewModel();
			var addEmployeeWindow = new AddEmployee
			{
				DataContext = addEmployeeViewModel
			};
			addEmployeeWindow.ShowDialog();
			LoadEmployees();
		}

		private void EditAbonements(object parameter)
		{
			if (_selectedEmployee != null)
			{
				var editEmployeeViewModel = new ChangeEmployeeViewModel(_selectedEmployee);
				var editEmployeeWindow = new ChangeEmployee
				{
					DataContext = editEmployeeViewModel
				};
				editEmployeeWindow.ShowDialog();
				LoadEmployees();
			}
			else
			{
				MessageBox.Show("Выберите абонемент для редактирования");
			}
		}

		private void DeleteEmployees(object parameter)
		{
			if (_selectedEmployee != null)
			{
				using (var dbContext = new MyDbContext())
				{
					var existingEmployee = dbContext.EMPLOYEES.Find(_selectedEmployee.ID_EMPLOYEE);
					if (existingEmployee != null)
					{
						dbContext.EMPLOYEES.Remove(existingEmployee);
						dbContext.SaveChanges();
						LoadEmployees();
					}
				}
			}
			else
			{
				MessageBox.Show("Выберите абонемент для удаления");
			}
		}

		private void Search(object parameter)
		{
			if (string.IsNullOrWhiteSpace(_searchText))
			{
				LoadEmployees();
			}
			else
			{
				using (var dbContext = new MyDbContext())
				{
					Employees = new ObservableCollection<EMPLOYEES>(
						dbContext.EMPLOYEES.Where(t => t.ID_EMPLOYEE.ToString().Contains(_searchText) ||
												t.LASTNAME_EMPLOYEE.Contains(_searchText) ||

												 t.FIRSTNAME_EMPLOYEE.Contains(_searchText) ||

												 t.PHONENAME_EMPLOYEE.Contains(_searchText) ||

												 t.SALARY_EMPLOYEE.ToString().Contains(_searchText)));
				}
			}
		}

	}
}
