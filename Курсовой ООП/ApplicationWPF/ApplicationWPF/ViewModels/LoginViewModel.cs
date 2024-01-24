using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using ApplicationWPF.Commands;
using ApplicationWPF.Models;
using ApplicationWPF.Views;

namespace ApplicationWPF.ViewModels
{
	public class LoginViewModel : ViewModelBase, IDataErrorInfo
	{
		private string _userName;
		private string _password;
		private readonly MyDbContext _dbContext = new MyDbContext();
		private bool _isPasswordVisible;

		public string UserName
		{
			get => _userName;
			set
			{
				_userName = value;
				OnPropertyChanged(nameof(UserName));
				((RelayCommand)LoginCommand).RaiseCanExecuteChanged();
			}
		}

		public string Password
		{
			get => _password;
			set
			{
				_password = value;
				OnPropertyChanged(nameof(Password));
				((RelayCommand)LoginCommand).RaiseCanExecuteChanged();
			}
		}

		public bool IsPasswordVisible
		{
			get { return _isPasswordVisible; }
			set
			{
				if (_isPasswordVisible != value)
				{
					_isPasswordVisible = value;
					OnPropertyChanged(nameof(IsPasswordVisible));
				}
			}
		}
		public string this[string columnName]
		{
			get
			{
				string error = null;

				switch (columnName)
				{
					case nameof(UserName):
						if (string.IsNullOrWhiteSpace(UserName))
							error = "Имя пользователя не может быть пустым.";
						break;

					case nameof(Password):
						if (string.IsNullOrWhiteSpace(Password))
							error = "Пароль не может быть пустым.";
						break;
				}

				return error;
			}
		}

		public string Error => null;
		public ICommand LoginCommand { get; }
		public ICommand ExitCommand { get; private set; }
		public ICommand ToRegister {  get; private set; }

		public LoginViewModel()
		{
			LoginCommand = new RelayCommand(Login, CanLogin);
			ExitCommand = new RelayCommand(parameter => ExitApp());
			ToRegister = new RelayCommand(parameter => RegisterApp());
		}

		private bool CanLogin(object parameter)
		{
			return !string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrWhiteSpace(Password);
		}

		private void Login(object parameter)
		{
			bool isAuthenticated = false;
			string? userType = null;
			CLIENTS client = null;

			try
			{
				var auth = _dbContext.LOGPAS
					.FirstOrDefault(lp => lp.LOGIN_VALUE == _userName && lp.PASSWORD_VALUE == _password);

				if (auth != null)
				{
					isAuthenticated = true;
					client = _dbContext.CLIENTS.FirstOrDefault(c => c.ID_LOGPAS == auth.ID_lOGPAS);
				}

			if (isAuthenticated)
			{
				if (auth.USER_TYPE == "client")
				{
					MainWindowViewModel mainWindowViewModel = new MainWindowViewModel(client);
					MainWindow main = new MainWindow { DataContext = mainWindowViewModel};
					main.Show();
					CloseWindow();
				}
				else if (auth.USER_TYPE == "admin")
				{
					AdministratorPanel administratorPanel = new AdministratorPanel();
					administratorPanel.Show();
					CloseWindow();
				}
				else
				{
					MessageBox.Show("Unknown user type. Cannot open the main window.");
				}
			}
				else
				{
					MessageBox.Show("Не удалось войти. Пожалуйста, проверьте ваши учетные данные.");
				}
			}
			catch (Exception)
			{
				MessageBox.Show("Ошибка во время аутентификации. Повторите попытку.");
			}
		}

private void RegisterApp()
		{
			Registration registration = new Registration();
			registration.Show();
		}

		private void ExitApp()
		{
			Application.Current.Shutdown();
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
