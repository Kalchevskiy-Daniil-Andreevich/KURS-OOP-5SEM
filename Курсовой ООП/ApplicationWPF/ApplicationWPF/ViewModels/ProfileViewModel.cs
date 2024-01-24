using ApplicationWPF.Commands;
using ApplicationWPF.Models;
using ApplicationWPF.Views;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace ApplicationWPF.ViewModels
{
    public class ProfileViewModel : ViewModelBase
    {
		private readonly MyDbContext _dbContext = new MyDbContext();
		private CLIENTS currentUser;
		private string _lastName;
		private string _firstName;
		private string _middleName;
		private string _phone;
		private int _age;
		private string _gender;
		private ObservableCollection<ABONEMENT_REQUEST> _requst;
		private ObservableCollection<SALEOFABONEMENTS> _sales;
		public string LastName
		{
			get => _lastName;
			set
			{
				_lastName = value;
				OnPropertyChanged(nameof(LastName));
			}
		}
		public string FirstName
		{
			get => _firstName;
			set
			{
				_firstName = value;
				OnPropertyChanged(nameof(FirstName));
			}
		}
		public string MiddleName
		{
			get => _middleName;
			set
			{
				_middleName = value;
				OnPropertyChanged(nameof(MiddleName));
			}
		}
		public string Phone
		{
			get => _phone;
			set
			{
				_phone = value;
				OnPropertyChanged(nameof(Phone));
			}
		}
		public int Age
		{
			get => _age;
			set
			{
				_age = value;
				OnPropertyChanged(nameof(Age));
			}
		}
		public string Gender
		{
			get => _gender;
			set
			{
				_gender = value;
				OnPropertyChanged(nameof(Gender));
			}
		}

		public ObservableCollection<ABONEMENT_REQUEST> Requests
		{
			get => _requst;
			set
			{
				_requst = value;
				OnPropertyChanged(nameof(Requests));
			}
		}

		public ObservableCollection<SALEOFABONEMENTS> Sales
		{
			get => _sales;
			set
			{
				_sales = value;
				OnPropertyChanged(nameof(Sales));
			}
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

		public ProfileViewModel()
		{
			_dbContext = new MyDbContext();
			InitializeCommands();
		}



		public ICommand ExitCommand { get; private set; }
		public ICommand ButtonOpenMenuCommand { get; private set; }
		public ICommand ButtonCloseMenuCommand { get; private set; }
		public ICommand ToAppAbon { get; private set; }
		public ICommand ToApp { get; private set; }

		private void InitializeCommands()
		{
			ToApp = new RelayCommand(AppWin);
			ToAppAbon = new RelayCommand(parameter => AbonWin());
			ExitCommand = new RelayCommand(parameter => ExitApp());
			ButtonOpenMenuCommand = new RelayCommand(parameter => ButtonOpenMenu_Click());
			ButtonCloseMenuCommand = new RelayCommand(parameter => ButtonCloseMenu_Click());
			CancelRequestCommand = new RelayCommand(parameter => CancelRequest(), parameter => CanCancelRequest());
		}

		private void AbonWin()
		{
			AbonementsViewModel abonementsViewModel = new AbonementsViewModel(currentUser);
			Abonements abonements = new Abonements { DataContext = abonementsViewModel };
			abonements.Show();
			CloseWindow();
		}

		private void AppWin(object parameter)
		{
			MainWindowViewModel mainWindowViewModel = new MainWindowViewModel(currentUser);
			MainWindow mainWindow = new MainWindow {DataContext = mainWindowViewModel };
			mainWindow.Show();
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

		public ProfileViewModel(CLIENTS user)
		{
			currentUser = user;
			UpdateUserData(user);
			LoadRequsts(user.ID_CLIENT);
			LoadSales(user.ID_CLIENT);
			InitializeCommands();
		}

		public void UpdateUserData(CLIENTS user)
		{
			LastName = user.LASTNAME_CLIENT;
			FirstName = user.FIRSTNAME_CLIENT;
			MiddleName = user.MIDDLENAME_CLIENT;
			Phone = user.PHONENAME_CLIENT;
			Age = user.AGE_CLIENT;
			Gender = user.GENDER_CLIENT;
		}

		public ICommand CancelRequestCommand { get; private set; }


		private ABONEMENT_REQUEST _selectedRequest;
		private bool _isCheckBoxChecked;

		public ABONEMENT_REQUEST SelectedRequest
		{
			get => _selectedRequest;
			set
			{
				_selectedRequest = value;
				OnPropertyChanged(nameof(SelectedRequest));
				((RelayCommand)CancelRequestCommand).RaiseCanExecuteChanged();
			}
		}
		public bool IsCheckBoxChecked
		{
			get => _isCheckBoxChecked;
			set
			{
				_isCheckBoxChecked = value;
				OnPropertyChanged(nameof(IsCheckBoxChecked));
				((RelayCommand)CancelRequestCommand).RaiseCanExecuteChanged();
			}
		}

		private void CancelRequest()
		{
			if (SelectedRequest != null && IsCheckBoxChecked)
			{
				try
				{
					_dbContext.ABONEMENT_REQUEST.Remove(SelectedRequest);
					_dbContext.SaveChanges();
					LoadRequsts(currentUser.ID_CLIENT);
				}
				catch (Exception ex)
				{
					MessageBox.Show($"Ошибка отмены заявки: {ex.Message}");
				}
			}
		}

		private bool CanCancelRequest()
		{
			return SelectedRequest != null && IsCheckBoxChecked;
		}

		private void LoadRequsts(int clientId)
		{
			try
			{
				Requests = new ObservableCollection<ABONEMENT_REQUEST>(_dbContext.ABONEMENT_REQUEST
									 .Where(b => b.ID_CLIENT == clientId)
									 .Include(b => b.ABONEMENTS)
									 .OrderByDescending(b => b.DATA_REQUEST)
									 .ToList());
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка загрузки запросов: {ex.Message}\nВнутреннее исключение: {ex.InnerException?.Message}");
			}
		}

		private void LoadSales(int clientId)
		{
			try
			{
				_sales = new ObservableCollection<SALEOFABONEMENTS>(_dbContext.SALEOFABONEMENTS
									 .Where(s => s.ID_CLIENT == clientId)
									 .Include(b => b.ABONEMENTS)
									 .OrderByDescending(s => s.DATA_BEGIN)
									 .ToList());
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error loading decor: {ex.Message}");
			}
		} 
	}
}
