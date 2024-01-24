using ApplicationWPF.Commands;
using ApplicationWPF.Models;
using ApplicationWPF.Views;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Media3D;

namespace ApplicationWPF.ViewModels
{
	public class RegistrationViewModel : ViewModelBase
	{
		private string _lastname;
		private string _firstname;
		private string _middlename;
		private string _age;
		private string _gender;
		private string _phone;
		private string _username;
		private string _password;

		private readonly MyDbContext _dbContext = new MyDbContext();

		public string LastName
		{
			get => _lastname;
			set
			{
				if (!string.IsNullOrEmpty(value) && value.Length <= 16)
				{
					_lastname = value;
					OnPropertyChanged(nameof(LastName));
					((RelayCommand)RegistrationCommand).RaiseCanExecuteChanged();
				}
			}
		}

		public string FirstName
		{
			get => _firstname;
			set
			{
				if (!string.IsNullOrEmpty(value) && value.Length <= 16)
				{
					_firstname = value;
					OnPropertyChanged(nameof(FirstName));
					((RelayCommand)RegistrationCommand).RaiseCanExecuteChanged();
				}
			}
		}

		public string MiddleName
		{
			get => _middlename;
			set
			{
				if (!string.IsNullOrEmpty(value) && value.Length <= 16)
				{
					_middlename = value;
					OnPropertyChanged(nameof(MiddleName));
					((RelayCommand)RegistrationCommand).RaiseCanExecuteChanged();
				}
			}
		}

		public string Age
		{
			get => _age;
			set
			{
				if (int.TryParse(value, out int age) && age >= 10 && age <= 100)
				{
					_age = value;
					OnPropertyChanged(nameof(Age));
					((RelayCommand)RegistrationCommand).RaiseCanExecuteChanged();
				}
			}
		}

		public string Gender
		{
			get => _gender; 
			set
			{
				if (value == "Мужской" || value == "Женский" || value == "Другое")
				{
					_gender = value;
					OnPropertyChanged(nameof(Gender));
					((RelayCommand)RegistrationCommand).RaiseCanExecuteChanged();
				}
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
					((RelayCommand)RegistrationCommand).RaiseCanExecuteChanged();
				}
			}
		}

		public string Username
		{
			get => _username;
			set
			{
					_username = value;
					OnPropertyChanged(nameof(Username));
					((RelayCommand)RegistrationCommand).RaiseCanExecuteChanged();
			}
		}

		public string Password
		{
			get => _password;
			set
			{
					_password = value;
					OnPropertyChanged(nameof(Password));
					((RelayCommand)RegistrationCommand).RaiseCanExecuteChanged();
				
			}
		}

		public ICommand RegistrationCommand { get; }
		public ICommand ExitCommand { get; private set; }
		public ICommand ToLogin { get; private set; }

		public RegistrationViewModel()
		{
			RegistrationCommand = new RelayCommand(Registration, CanRegistration);
			ToLogin = new RelayCommand(parameter => LoginApp());
			ExitCommand = new RelayCommand(parameter => ExitApp());
		}

		private bool IsUsernameUnique(string username)
		{
			return !_dbContext.LOGPAS.Any(lp => lp.LOGIN_VALUE == username);
		}

		private bool CanRegistration(object parameter)
		{
			return !string.IsNullOrEmpty(LastName) && !string.IsNullOrEmpty(FirstName) && !string.IsNullOrEmpty(MiddleName) &&
				   !string.IsNullOrEmpty(Age) && !string.IsNullOrEmpty(Gender) && !string.IsNullOrEmpty(Phone) &&
				   !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password);
		}

		private void Registration(object parameter)
		{
			bool isSuccess = false;
			try
			{
				if (!IsUsernameUnique(Username))
				{
					MessageBox.Show("Имя пользователя уже существует. Пожалуйста, выберите другое имя пользователя.");
					return;
				}
				var logpas = new LOGPAS
				{
					LOGIN_VALUE = _username,
					PASSWORD_VALUE = _password,
					USER_TYPE = "client"
				};

				_dbContext.LOGPAS.Add(logpas);
				_dbContext.SaveChanges();

				var idLogpas = _dbContext.LOGPAS
					.Where(a => a.LOGIN_VALUE == _username && a.PASSWORD_VALUE == _password)
					.Select(a => a.ID_lOGPAS)
					.FirstOrDefault();

				var client = new CLIENTS
				{
					ID_LOGPAS = idLogpas,
					FIRSTNAME_CLIENT = _firstname,
					LASTNAME_CLIENT = _lastname,
					MIDDLENAME_CLIENT = _middlename,
					AGE_CLIENT = int.Parse(_age),
					GENDER_CLIENT = _gender,
					PHONENAME_CLIENT = _phone
				};


				_dbContext.CLIENTS.Add(client);
				_dbContext.SaveChanges();

				isSuccess = true;
			}
			catch (Exception)
			{
				MessageBox.Show("Invalid credentials. Please try again.");
			}

			if (isSuccess)
			{
				MessageBox.Show("Registration successful! ");
			}
			else
			{
				MessageBox.Show("Registration failed. Please check your credentials.");
			}
		}

		private void LoginApp()
		{
			Login login = new Login();
			login.Show();
			CloseWindow();
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
