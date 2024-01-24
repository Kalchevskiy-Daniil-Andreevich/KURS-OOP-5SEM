using ApplicationWPF.Commands;
using ApplicationWPF.Models;
using ApplicationWPF.Views;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ApplicationWPF.ViewModels
{
	class MainWindowViewModel : ViewModelBase
	{
		private readonly MyDbContext _dbContext;
		private CLIENTS _currentClient;
		public MainWindowViewModel()
		{
			InitializeCommands();
		}

		public MainWindowViewModel(CLIENTS currentClient)
		{
			_currentClient = currentClient;
			_dbContext = new MyDbContext();
			InitializeCommands();
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
		public ICommand ToAppAbon { get; private set; }
		public ICommand ButtonOpenMenuCommand { get; private set; }
		public ICommand ButtonCloseMenuCommand { get; private set; }
		public ICommand ToProfile { get; private set; }
		public ICommand ToShedule { get; private set; }

		private void InitializeCommands()
		{
			ExitCommand = new RelayCommand(parameter => ExitApp());
			ToAppAbon = new RelayCommand(parameter => AbonWin());
			ButtonOpenMenuCommand = new RelayCommand(parameter => ButtonOpenMenu_Click());
			ButtonCloseMenuCommand = new RelayCommand(parameter => ButtonCloseMenu_Click());
			ToProfile = new RelayCommand(parameter => ProfileApp());
			ToShedule = new RelayCommand(parameter => SheduleApp());
		}

		private void AbonWin()
		{
			AbonementsViewModel abonementsViewModel = new AbonementsViewModel(_currentClient);
			Abonements abonements = new Abonements {DataContext = abonementsViewModel};
			
			abonements.Show();
			CloseWindow();
		}

		private void ProfileApp()
		{
			ProfileViewModel profileViewModel = new ProfileViewModel(_currentClient);
			Profile profile = new Profile{DataContext = profileViewModel};

			profile.Show();
			CloseWindow();
		}

		private void SheduleApp()
		{
			SheduleViewModel sheduleViewModel = new SheduleViewModel(_currentClient);
			Shedule shedule = new Shedule { DataContext = sheduleViewModel };
			shedule.Show();
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
	}
}
