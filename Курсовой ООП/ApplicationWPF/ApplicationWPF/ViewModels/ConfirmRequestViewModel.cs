using ApplicationWPF.Commands;
using ApplicationWPF.Models;
using ApplicationWPF.Views;
using Microsoft.EntityFrameworkCore;
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
	class ConfirmRequestViewModel : ViewModelBase
	{
		private ObservableCollection<RequestViewModel> _allAbonements;
		private RequestViewModel _selectedRequest;
		private string _searchText;
		private EMPLOYEES currentEmployee;

		public ConfirmRequestViewModel() { }
		public ConfirmRequestViewModel(EMPLOYEES employees)
		{
			currentEmployee = employees;
			ExitCommand = new RelayCommand(parameter => ExitApp());
			ButtonOpenMenuCommand = new RelayCommand(parameter => ButtonOpenMenu_Click());
			ButtonCloseMenuCommand = new RelayCommand(parameter => ButtonCloseMenu_Click());
			SearchCommand = new RelayCommand(Search);
			ApproveRequestCommand = new RelayCommand(ApproveRequest);
			RejectRequestCommand = new RelayCommand(RejectRequest);
			ToClientsApp = new RelayCommand(parameter => Adminclient());
			ToAdminEmpl = new RelayCommand(parameter => AdminEmpl());
			ToAbonements = new RelayCommand(parameter => AbonementApp());

			Requests = new ObservableCollection<RequestViewModel>();
			LoadRequest();
		}
		public RequestViewModel SelectedRequest
		{
			get => _selectedRequest;
			set
			{
				_selectedRequest = value;
				OnPropertyChanged(nameof(SelectedRequest));
			}
		}

		public ObservableCollection<RequestViewModel> Requests
		{
			get => _allAbonements;
			set { _allAbonements = value; OnPropertyChanged(nameof(Requests)); }
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
		public ICommand ApproveRequestCommand { get; private set; }
		public ICommand RejectRequestCommand { get; private set; }
		public ICommand ToClientsApp { get; private set; }
		public ICommand ToAdminEmpl { get; private set; }
		public ICommand ToAbonements { get; private set; }

		private void LoadRequest()
		{
			using (var dbContext = new MyDbContext())
			{
				var bookingViewModels = dbContext.ABONEMENT_REQUEST
					.Include(t => t.ABONEMENTS)
					.Include(c => c.CLIENTS)
					.ToList()
					.Select(b => new RequestViewModel(b, b.ABONEMENTS, b.CLIENTS));

				Requests = new ObservableCollection<RequestViewModel>(bookingViewModels);
			}
		}

		private void Search(object parameter)
		{
			if (string.IsNullOrWhiteSpace(_searchText))
			{
				LoadRequest();
			}
			else
			{
				using (var dbContext = new MyDbContext())
				{
					Requests = new ObservableCollection<RequestViewModel>(
						_allAbonements.Where(t => t.Request_id.ToString().Contains(_searchText) ||
												 t.Date_request.ToString().Contains(_searchText) ||
												 t.Last_name.ToString().Contains(_searchText) ||
												 t.First_name.ToString().Contains(_searchText) ||
												 t.Middle_name.Contains(_searchText) ||
												 t.Price.ToString().Contains(_searchText) ||
												 t.Amount.ToString().Contains(_searchText) ||
												 t.Type.Contains(_searchText)).ToList());
				}
			}
		}

		private void ApproveRequest(object parameter)
		{
			if (_selectedRequest != null)
			{
				using (var dbContext = new MyDbContext())
				{
					var existingBooking = dbContext.ABONEMENT_REQUEST.Find(_selectedRequest.Request_id);
					if (existingBooking != null)
					{
						DateTime endDate = DateTime.Now.AddMonths(1);
						var newSale = new SALEOFABONEMENTS
						{
							DATA_BEGIN = DateTime.Now,
							DATA_END = endDate,
							ID_CLIENT = existingBooking.ID_CLIENT,
							ID_ABONEMENT = existingBooking.ID_ABONEMENT
						};

						dbContext.SALEOFABONEMENTS.Add(newSale);
						dbContext.SaveChanges();

						dbContext.ABONEMENT_REQUEST.Remove(existingBooking);
						dbContext.SaveChanges();
						LoadRequest();
					}
				}
			}
			else
			{
				MessageBox.Show("Выберите абонемент для зоморозки");
			}
		}

		private void RejectRequest(object parameter)
		{
			if (_selectedRequest != null)
			{
				using (var dbContext = new MyDbContext())
				{
					var existingBooking = dbContext.ABONEMENT_REQUEST.Find(_selectedRequest.Request_id);
					if (existingBooking != null)
					{
						dbContext.ABONEMENT_REQUEST.Remove(existingBooking);
						dbContext.SaveChanges();
						LoadRequest();
					}
				}
			}
			else
			{
				MessageBox.Show("Выберите абонемент для удаления");
			}
		}

		private void AdminEmpl()
		{
			AdminEmployee adminEmployee = new AdminEmployee();
			adminEmployee.Show();
			CloseWindow();
		}

		private void Adminclient()
		{
			AdminClient adminClient = new AdminClient();
			adminClient.ShowDialog();
			CloseWindow();
		}

				private void AbonementApp()
		{
			AdministratorPanel administratorPanel = new AdministratorPanel();
			administratorPanel.Show();
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
