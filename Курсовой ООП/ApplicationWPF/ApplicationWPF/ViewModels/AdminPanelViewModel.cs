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
    class AdminPanelViewModel : ViewModelBase
    {
		private ObservableCollection<ABONEMENTS> _abonements;
		private ABONEMENTS _selectedAbonement;
		private string _searchText;
		private EMPLOYEES employee;

		public ABONEMENTS SelectedAbonement
		{
			get => _selectedAbonement;
			set
			{
				_selectedAbonement = value;
				OnPropertyChanged(nameof(SelectedAbonement));
			}
		}

		public ObservableCollection<ABONEMENTS> Abonemements
		{
			get => _abonements;
			set
			{
				_abonements = value;
				OnPropertyChanged(nameof(Abonemements));
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
		public ICommand AddAbonementCommand { get; private set; }
		public ICommand EditAbonementCommand { get; private set; }
		public ICommand DeleteAbonementCommand { get; private set; }
		public ICommand ToAdminEmpl { get; private set; }
		public ICommand ToClientsApp { get; private set; }
		public ICommand Requests { get; private set; }


		public AdminPanelViewModel()
		{
			ExitCommand = new RelayCommand(parameter => ExitApp());
			ButtonOpenMenuCommand = new RelayCommand(parameter => ButtonOpenMenu_Click());
			ButtonCloseMenuCommand = new RelayCommand(parameter => ButtonCloseMenu_Click());
			SearchCommand = new RelayCommand(Search);
			AddAbonementCommand = new RelayCommand(AddAbonements);
			EditAbonementCommand = new RelayCommand(EditAbonements);
			DeleteAbonementCommand = new RelayCommand(DeleteAbonements);
			ToAdminEmpl = new RelayCommand(parameter => AdminEmpl());
			ToClientsApp = new RelayCommand(parameter => Adminclient());
			Requests = new RelayCommand(parameter => RequestApp());


			Abonemements = new ObservableCollection<ABONEMENTS>();
			LoadAbonements();
		}

		private void LoadAbonements()
		{
			using (var dbContext = new MyDbContext())
			{
				Abonemements = new ObservableCollection<ABONEMENTS>(dbContext.ABONEMENTS.ToList());
			}
		}

		private void AddAbonements(object parameter)
		{
			var addAbonementViewModel = new AddAbonementView();
			var addAbonemnetWindow = new AddAbonement
			{
				DataContext = addAbonementViewModel
			};
			addAbonemnetWindow.ShowDialog();
			LoadAbonements();
		}

		private void EditAbonements(object parameter)
		{
			if (_selectedAbonement != null)
			{
				var editTourViewModel = new ChangeAbonementViewModel(_selectedAbonement);
				var editAbonemnetWindow = new ChangeAbonement
				{
					DataContext = editTourViewModel
				};
				editAbonemnetWindow.ShowDialog();
				LoadAbonements();
			}
			else
			{
				MessageBox.Show("Выберите абонемент для редактирования");
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

		private void RequestApp()
		{
			ConfirmRequestViewModel confirmRequestViewModel = new ConfirmRequestViewModel(employee);
			AdminConfirmRequest req = new AdminConfirmRequest {DataContext = confirmRequestViewModel };
			req.Show();
			CloseWindow();
		}

		private void DeleteAbonements(object parameter)
		{
			if (_selectedAbonement != null)
			{
				using (var dbContext = new MyDbContext())
				{
					var existingAbonement = dbContext.ABONEMENTS.Find(_selectedAbonement.ID_ABONEMENT);
					if (existingAbonement != null)
					{
						dbContext.ABONEMENTS.Remove(existingAbonement);
						dbContext.SaveChanges();
						LoadAbonements();
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
				LoadAbonements();
			}
			else
			{
				using (var dbContext = new MyDbContext())
				{
					Abonemements = new ObservableCollection<ABONEMENTS>(
						dbContext.ABONEMENTS.Where(t => t.ID_ABONEMENT.ToString().Contains(_searchText) ||
												t.TYPE_OF_ABONEMENT.Contains(_searchText) ||

												 t.AMOUNT_OF_VISITS.ToString().Contains(_searchText) ||

												 t.PRICE_ABONEMENT.ToString().Contains(_searchText) ||

												 t.TYPE_OF_EXERCISE.Contains(_searchText) ||

												 t.ADDITIONAL_SERVICE.Contains(_searchText) ||

												 t.AMOUNT_OF_VISITS.ToString().Contains(_searchText)));
				}
			}
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


