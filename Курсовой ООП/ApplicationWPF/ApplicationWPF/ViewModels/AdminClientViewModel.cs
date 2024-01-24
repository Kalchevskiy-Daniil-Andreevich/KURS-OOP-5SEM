using ApplicationWPF.Commands;
using ApplicationWPF.Models;
using ApplicationWPF.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ApplicationWPF.ViewModels
{
	class AdminClientViewModel : ViewModelBase
	{
		private ObservableCollection<CLIENTS> _clients;
		private CLIENTS _selectedClients;
		private string _searchText;

		public CLIENTS SelectedClients
		{
			get => _selectedClients;
			set
			{
				_selectedClients = value;
				OnPropertyChanged(nameof(SelectedClients));
			}
		}

		public ObservableCollection<CLIENTS> Clients
		{
			get => _clients;
			set
			{
				_clients = value;
				OnPropertyChanged(nameof(Clients));
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

			public ICommand ExitCommand { get; private set; }
			public ICommand ButtonOpenMenuCommand { get; private set; }
			public ICommand ButtonCloseMenuCommand { get; private set; }
			public ICommand SearchCommand { get; private set; }
			public ICommand DeleteClientsCommand { get; private set; }
			public ICommand AddClientsCommand { get; private set; }
			public ICommand EditClientCommand { get; private set; }
			public ICommand ToEmployees { get; private set; }
			public ICommand ToAbonements { get; private set; }
			public ICommand ToRequestApp { get; private set; }


		public AdminClientViewModel()
        {

			ExitCommand = new RelayCommand(parameter => ExitApp());
			ButtonOpenMenuCommand = new RelayCommand(parameter => ButtonOpenMenu_Click());
			ButtonCloseMenuCommand = new RelayCommand(parameter => ButtonCloseMenu_Click());
			SearchCommand = new RelayCommand(Search);
			DeleteClientsCommand = new RelayCommand(DeleteClients);
			AddClientsCommand = new RelayCommand(AddClients);
			EditClientCommand = new RelayCommand(EditClients);
			ToEmployees = new RelayCommand(parameter => EmployeeApp());
			ToAbonements = new RelayCommand(parameter => AbonementApp());
			ToRequestApp = new RelayCommand(parameter => Adminrequest());


			Clients = new ObservableCollection<CLIENTS>();
			LoadClients();
		}

		private void LoadClients()
		{
			using (var dbContext = new MyDbContext())
			{
				Clients = new ObservableCollection<CLIENTS>(dbContext.CLIENTS.ToList());
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

		private void EmployeeApp()
		{
			AdminEmployee adminEmployee = new AdminEmployee();
			adminEmployee.Show();
			CloseWindow();
		}

		private void AbonementApp()
		{
			AdministratorPanel administratorPanel = new AdministratorPanel();
			administratorPanel.Show();
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

		private void AddClients(object parameter)
		{
			var addClientViewModel = new AdminClientAddViewModel();
			var addClientWindow = new AdminClientAdd
			{
				DataContext = addClientViewModel
			};
			addClientWindow.ShowDialog();
			LoadClients();
		}

		private void EditClients(object parameter)
		{
			if (_selectedClients != null)
			{
				var editClientViewModel = new ChangeClientViewModel(_selectedClients);
				var editClientWindow = new ChangeClient
				{
					DataContext = editClientViewModel
				};
				editClientWindow.ShowDialog();
				LoadClients();
			}
			else
			{
				MessageBox.Show("Выберите клиента для редактирования");
			}
		}

		private void Search(object parameter)
		{
			if (string.IsNullOrWhiteSpace(_searchText))
			{
				LoadClients();
			}
			else
			{
				using (var dbContext = new MyDbContext())
				{
						Clients = new ObservableCollection<CLIENTS>(
						dbContext.CLIENTS.Where(t => t.ID_LOGPAS.ToString().Contains(_searchText) ||
												t.LASTNAME_CLIENT.Contains(_searchText) ||

												 t.FIRSTNAME_CLIENT.Contains(_searchText) ||

												 t.MIDDLENAME_CLIENT.Contains(_searchText) ||

												 t.AGE_CLIENT.ToString().Contains(_searchText) ||

												 t.GENDER_CLIENT.Contains(_searchText) ||

												 t.PHONENAME_CLIENT.Contains(_searchText)));
				}
			}
		}
		private void DeleteClients(object parameter)
		{
			if (_selectedClients != null)
			{
				using (var dbContext = new MyDbContext())
				{
					var existingClient = dbContext.CLIENTS.Find(_selectedClients.ID_CLIENT);
					if (existingClient != null)
					{
						dbContext.CLIENTS.Remove(existingClient);
						dbContext.SaveChanges();
						LoadClients();
					}
				}
			}
			else
			{
				MessageBox.Show("Выберите клиента для удаления");
			}
		}
	}
}
