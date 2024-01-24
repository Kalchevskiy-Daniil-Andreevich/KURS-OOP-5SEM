using ApplicationWPF.Commands;
using ApplicationWPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ApplicationWPF.ViewModels
{
	class ChangeClientViewModel : ViewModelBase
	{
		private readonly MyDbContext _dbContext = new MyDbContext();
		private CLIENTS _selectedClients;

		public int IdLog
		{
			get => _selectedClients.ID_LOGPAS;
			set
			{
				if (_selectedClients != null && value >= 0)
				{
					_selectedClients.ID_LOGPAS = value;
					OnPropertyChanged(nameof(IdLog));
				}
				else
				{
					MessageBox.Show("Не может быть отрицательного числа для данного поля!.");
				}
			}
		}

		public string First
		{
			get => _selectedClients.FIRSTNAME_CLIENT;
			set
			{
				if (_selectedClients != null)
				{
					_selectedClients.FIRSTNAME_CLIENT = value;
					OnPropertyChanged(nameof(First));
				}
			}
		}

		public string Last
		{
			get => _selectedClients.LASTNAME_CLIENT;
			set
			{
				if (_selectedClients != null)
				{
					_selectedClients.LASTNAME_CLIENT = value;
					OnPropertyChanged(nameof(Last));
				}
			}
		}

		public string Middle
		{
			get => _selectedClients.MIDDLENAME_CLIENT;
			set
			{
				if (_selectedClients != null)
				{

					_selectedClients.MIDDLENAME_CLIENT = value;
					OnPropertyChanged(nameof(Middle));
				}
			}
		}

		public int Age
		{
			get => _selectedClients.AGE_CLIENT;
			set
			{
				if (_selectedClients != null && value >= 0)
				{
					_selectedClients.AGE_CLIENT = value;
					OnPropertyChanged(nameof(Age));
				}
				else
				{
					MessageBox.Show("Не может быть отрицательного числа для данного поля!.");
				}
			}
		}

		public string Gender
		{
			get => _selectedClients.GENDER_CLIENT;
			set
			{
				if (_selectedClients != null)
				{
					_selectedClients.GENDER_CLIENT = value;
					OnPropertyChanged(nameof(Gender));
				}
			}
		}

		public string Phone
		{
			get => _selectedClients.PHONENAME_CLIENT;
			set
			{
				if (Regex.IsMatch(value, @"^\+375\d{9}$"))
				{
					_selectedClients.PHONENAME_CLIENT = value;
					OnPropertyChanged(nameof(Phone));
				}
			}
		}

		public ICommand ExitCommand { get; private set; }
		public ICommand EditAbonementCommand { get; private set; }


		public ChangeClientViewModel()
        {
		}
        public ChangeClientViewModel(CLIENTS selectedClient)
        {
			_selectedClients = selectedClient;
			ExitCommand = new RelayCommand(parameter => ExitApp());
			EditAbonementCommand = new RelayCommand(parameter => EditClient());
			IdLog = _selectedClients.ID_LOGPAS;
			First = _selectedClients.FIRSTNAME_CLIENT;
			Last = _selectedClients.LASTNAME_CLIENT;
			Middle = _selectedClients.MIDDLENAME_CLIENT;
			Age = _selectedClients.AGE_CLIENT;
			Gender = _selectedClients.GENDER_CLIENT;
			Phone = _selectedClients.PHONENAME_CLIENT;
		}

		private void ExitApp()
		{
			CloseWindow();
		}

		private void EditClient()
		{
			try
			{
				var Client = _dbContext.CLIENTS.Find(_selectedClients.ID_CLIENT);

				if (Client != null)
				{
					Client.ID_LOGPAS = _selectedClients.ID_LOGPAS;
					Client.FIRSTNAME_CLIENT = _selectedClients.FIRSTNAME_CLIENT;
					Client.LASTNAME_CLIENT = _selectedClients.LASTNAME_CLIENT;
					Client.MIDDLENAME_CLIENT = _selectedClients.MIDDLENAME_CLIENT;
					Client.AGE_CLIENT = _selectedClients.AGE_CLIENT;
					Client.GENDER_CLIENT = _selectedClients.GENDER_CLIENT;
					Client.PHONENAME_CLIENT = _selectedClients.PHONENAME_CLIENT;
					_dbContext.SaveChanges();
					MessageBox.Show("Клиент успешно изменен!");
					CloseWindow();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка при изменении клиента: {ex.Message}");
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
