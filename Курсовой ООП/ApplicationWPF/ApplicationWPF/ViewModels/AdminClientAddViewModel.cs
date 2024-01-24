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
	class AdminClientAddViewModel : ViewModelBase
	{
		private int _idlog;
		private string _first;
		private string _last;
		private string _middle;
		private int _age;
		private string _gender;
		private string _phone;

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

		public string First
		{
			get => _first;
			set
			{
				_first = value;
				OnPropertyChanged(nameof(First));
			}
		}

		public string Last
		{
			get => _last;
			set
			{
				_last = value;
				OnPropertyChanged(nameof(Last));
			}
		}

		public string Middle
		{
			get => _middle;
			set
			{
				_middle = value;
				OnPropertyChanged(nameof(Middle));
			}
		}

		public int Age
		{
			get => _age;
			set
			{
				if (value >= 0)
				{
					_age = value;
					OnPropertyChanged(nameof(Age));
				}
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
		public ICommand ExitCommand { get; private set; }
		public ICommand AddClientCommand { get; private set; }
		public ICommand ToClientsApp { get; private set; }

		public AdminClientAddViewModel()
        {
			ExitCommand = new RelayCommand(parameter => ExitApp());
			AddClientCommand = new RelayCommand(parameter => AddClient());
		}

		private void AddClient()
		{
			try
			{
				using (var dbContext = new MyDbContext())
				{
					var newClient = new CLIENTS
					{
						ID_LOGPAS = _idlog,
						LASTNAME_CLIENT = _last,
						FIRSTNAME_CLIENT = _first,
						MIDDLENAME_CLIENT = _middle,
						AGE_CLIENT = _age,
						GENDER_CLIENT = _gender,
						PHONENAME_CLIENT = _phone,
					};

					dbContext.CLIENTS.Add(newClient);
					dbContext.SaveChanges();
					MessageBox.Show("Клиент успешно добавлен!");
					CloseWindow();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка при добавлении клиента: {ex.Message}");
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
