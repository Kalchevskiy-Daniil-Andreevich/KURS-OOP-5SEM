using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using ApplicationWPF.Commands;
using ApplicationWPF.Views;
using ApplicationWPF.Models;
using System.Collections.ObjectModel;

namespace ApplicationWPF.ViewModels
{ 
    class SheduleViewModel : ViewModelBase
    {
		private ObservableCollection<SHEDULE_TABLE> _sheduls;
		private SHEDULE_TABLE _selectedSheduls;
		private CLIENTS currentClient;
		private Visibility _buttonOpenMenuVisibility = Visibility.Visible;
		private Visibility _buttonCloseMenuVisibility = Visibility.Collapsed;

		public SHEDULE_TABLE SelectedSheduls
		{
			get => _selectedSheduls;
			set
			{
				_selectedSheduls = value;
				OnPropertyChanged(nameof(SelectedSheduls));
			}
		}

		public ObservableCollection<SHEDULE_TABLE> Sheduls
		{
			get => _sheduls;
			set
			{
				_sheduls = value;
				OnPropertyChanged(nameof(Sheduls));
			}
		}

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
		public ICommand ToApp { get; private set; }

		public SheduleViewModel()
        {

		}

		public SheduleViewModel(CLIENTS client)
		{
			currentClient = client;
			ExitCommand = new RelayCommand(parameter => ExitApp());
			ButtonOpenMenuCommand = new RelayCommand(parameter => ButtonOpenMenu_Click());
			ButtonCloseMenuCommand = new RelayCommand(parameter => ButtonCloseMenu_Click());
			ToApp = new RelayCommand(AppWin);

			Sheduls = new ObservableCollection<SHEDULE_TABLE>();
			LoadSheduls();
		}

		private void LoadSheduls()
		{
			using (var dbContext = new MyDbContext())
			{
				Sheduls = new ObservableCollection<SHEDULE_TABLE>(dbContext.SHEDULE_TABLE.ToList());
			}
		}

		private void AppWin(object parameter)
		{
			MainWindowViewModel mainWindowViewModel = new MainWindowViewModel(currentClient);
			MainWindow mainWindow = new MainWindow { DataContext = mainWindowViewModel };
			mainWindow.Show();
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
