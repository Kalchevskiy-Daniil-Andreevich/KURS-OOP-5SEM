using ApplicationWPF.Classes;
using ApplicationWPF.Commands;
using ApplicationWPF.Models;
using ApplicationWPF.Views;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace ApplicationWPF.ViewModels
{
    class AbonementsViewModel : ViewModelBase
	{
		private readonly MyDbContext _dbContext;
		private ObservableCollection<ABONEMENTS> _allAbonements;
		private ObservableCollection<ABONEMENTS> _filteredAbonements;
		private string _searchText;
		private AbonementFilter _selectedFilterOption;
		private ABONEMENTS _selectedAbonement;
		private ObservableCollection<ABONEMENTS> _abonements;
		private CLIENTS _clients;
		private ABONEMENTS _abonement;

		public AbonementsViewModel()
		{

		}

		public AbonementsViewModel(CLIENTS currentClient)
		{
			_currentClient = currentClient;
			_dbContext = new MyDbContext();
			InitializeCommands();
			LoadData();
			Abonemements = new ObservableCollection<ABONEMENTS>();
			LoadAbonements();
			CloseDetailsPopupCommand = new RelayCommand(parameter => CloseDetailsPopup());
			PriceRanges = new ObservableCollection<PriceRange>
			{
				new PriceRange(0, 100), 
				new PriceRange(101, 200),
				new PriceRange(150, 300),
				new PriceRange(300, 500),
			};
			AmountOfVisitsRanges = new ObservableCollection<VisitRange>
			{
				new VisitRange(0, 30), 
				new VisitRange(30, 60),
				new VisitRange(60, 90),
				new VisitRange(90, 150),
			};
		}

		public ObservableCollection<ABONEMENTS> FilteredAbonements
		{
			get => _filteredAbonements;
			set
			{
				_filteredAbonements = value;
				OnPropertyChanged();
			}
		}

		public ABONEMENTS SelectedAbonement
		{
			get => _selectedAbonement;
			set
			{
				_selectedAbonement = value;
				OnPropertyChanged();
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

		private ObservableCollection<PriceRange> _priceRanges;

		public ObservableCollection<PriceRange> PriceRanges
		{
			get => _priceRanges;
			set
			{
				_priceRanges = value;
				OnPropertyChanged();
			}
		}

		private PriceRange _selectedPriceRange;

		public PriceRange SelectedPriceRange
		{
			get => _selectedPriceRange;
			set
			{
				_selectedPriceRange = value;
				OnPropertyChanged();
			}
		}

		private VisitRange _selectedAmountOfVisitsRange;

		public VisitRange SelectedAmountOfVisitsRange
		{
			get => _selectedAmountOfVisitsRange;
			set
			{
				_selectedAmountOfVisitsRange = value;
				OnPropertyChanged();
			}
		}

		private ObservableCollection<VisitRange> _amountOfVisitsRanges;

		public ObservableCollection<VisitRange> AmountOfVisitsRanges
		{
			get => _amountOfVisitsRanges;
			set
			{
				_amountOfVisitsRanges = value;
				OnPropertyChanged();
			}
		}

		public ObservableCollection<AbonementFilter> FilterOptions { get; } = new ObservableCollection<AbonementFilter>
		{
			new AbonementFilter("По убыванию цены", AbonementSortingType.PriceDesc),
			new AbonementFilter("По возрастанию цены", AbonementSortingType.PriceAsc),
			new AbonementFilter("По убыванию посещений", AbonementSortingType.VisitDesc),
			new AbonementFilter("По возрастанию посещений", AbonementSortingType.VisitAsc),
		};

		public string SearchText
		{
			get => _searchText;
			set
			{
				_searchText = value;
				OnPropertyChanged();
			}
		}

		private string _filterText;

		public string FilterText
		{
			get => _filterText;
			set
			{
				_filterText = value;
				OnPropertyChanged();
			}
		}

		public AbonementFilter SelectedFilterOption
		{
			get => _selectedFilterOption;
			set
			{
				_selectedFilterOption = value;
				OnPropertyChanged();
			}
		}

		private Visibility _buttonOpenMenuVisibility = Visibility.Visible;
		private Visibility _buttonCloseMenuVisibility = Visibility.Collapsed;
		private CLIENTS _currentClient;

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

		private bool _isDetailsPopupOpen;
		public bool IsDetailsPopupOpen
		{
			get { return _isDetailsPopupOpen; }
			set
			{
				if (_isDetailsPopupOpen != value)
				{
					_isDetailsPopupOpen = value;
					OnPropertyChanged(nameof(IsDetailsPopupOpen));
				}
			}
		}

		private ABONEMENTS _selectedSubscription;
		public ABONEMENTS SelectedSubscription
		{
			get { return _selectedSubscription; }
			set
			{
				if (_selectedSubscription != value)
				{
					_selectedSubscription = value;
					IsDetailsPopupOpen = true;
					OnPropertyChanged(nameof(SelectedSubscription));
				}
			}
		}

		public ICommand SearchCommand { get; private set; }
		public ICommand ToApp { get; private set; }
		public ICommand ExitCommand { get; private set; }
		public ICommand ButtonOpenMenuCommand { get; private set; }
		public ICommand ButtonCloseMenuCommand { get; private set; }
		public ICommand ToProfiles { get; private set; }
		public ICommand ToMoreData { get; private set; }
		public ICommand CloseDetailsPopupCommand { get; }
		public ICommand ToRequests { get; private set; }
		public ICommand FilterCommand { get; private set; }


		private void InitializeCommands()
		{
			SearchCommand = new RelayCommand(parameter => SearchAbonements());
			ToApp = new RelayCommand(AppWin);
			ExitCommand = new RelayCommand(parameter => ExitApp());
			ButtonOpenMenuCommand = new RelayCommand(parameter => ButtonOpenMenu_Click());
			ButtonCloseMenuCommand = new RelayCommand(parameter => ButtonCloseMenu_Click());
			ToProfiles = new RelayCommand(parameter => ProfileApp());
			ToRequests = new RelayCommand(Request);
			FilterCommand = new RelayCommand(parameter => FilterAbonements());
		}

		private void CloseDetailsPopup()
		{
			IsDetailsPopupOpen = false;
		}

		private void LoadData()
		{
			_allAbonements = new ObservableCollection<ABONEMENTS>(_dbContext.ABONEMENTS);
			FilteredAbonements = _allAbonements;
			SelectedAbonement = _allAbonements.FirstOrDefault();
		}

		private void LoadAbonements()
		{
			using (var dbContext = new MyDbContext())
			{
				Abonemements = new ObservableCollection<ABONEMENTS>(dbContext.ABONEMENTS.ToList());
			}
		}

		private void SearchAbonements()
		{
			var query = _allAbonements.AsQueryable();

			if (!string.IsNullOrEmpty(SearchText))
			{
				query = query.Where(t =>
					t.ID_ABONEMENT.ToString().Contains(SearchText) ||
					t.TYPE_OF_ABONEMENT.Contains(SearchText) ||
					t.PRICE_ABONEMENT.ToString().Contains(SearchText) ||
					t.TYPE_OF_EXERCISE.Contains(SearchText) ||
					t.ADDITIONAL_SERVICE.Contains(SearchText) ||
					t.AMOUNT_OF_VISITS.ToString().Contains(SearchText)
				);
			}

			if (SelectedFilterOption != null)
			{
				switch (SelectedFilterOption.SortingType)
				{
					case AbonementSortingType.PriceAsc:
						query = query.OrderBy(t => t.PRICE_ABONEMENT);
						break;
					case AbonementSortingType.PriceDesc:
						query = query.OrderByDescending(t => t.PRICE_ABONEMENT);
						break;
					case AbonementSortingType.VisitAsc:
						query = query.OrderBy(t => t.AMOUNT_OF_VISITS);
						break;
					case AbonementSortingType.VisitDesc:
						query = query.OrderByDescending(t => t.AMOUNT_OF_VISITS);
						break;
				}
			}

			FilteredAbonements = new ObservableCollection<ABONEMENTS>(query.ToList());
			SelectedFilterOption = null;
		}

		private void AppWin(object parameter)
		{
			MainWindowViewModel mainWindowViewModel = new MainWindowViewModel(_currentClient);
			MainWindow mainWindow = new MainWindow { DataContext = mainWindowViewModel };
			mainWindow.Show();
			CloseWindow();
		}

		private void ProfileApp()
		{
			ProfileViewModel profileViewModel = new ProfileViewModel(_currentClient);
			Profile profile = new Profile {DataContext = profileViewModel};
			profile.Show();
			CloseWindow();
		}

		private void Request(object parameter)
		{
			RequestAbonViewModel requestAbonViewModel = new RequestAbonViewModel(_abonement, _currentClient);
			RequestAbon req = new RequestAbon { DataContext = requestAbonViewModel };
			req.Show();
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

		private void FilterAbonements()
		{
			var query = _allAbonements.AsQueryable();

			if (!string.IsNullOrEmpty(SearchText))
			{
				query = query.Where(t =>
				t.TYPE_OF_ABONEMENT.Contains(SearchText) ||
				t.PRICE_ABONEMENT.ToString().Contains(SearchText) ||
				t.TYPE_OF_EXERCISE.Contains(SearchText) ||
				t.ADDITIONAL_SERVICE.Contains(SearchText) ||
				t.AMOUNT_OF_VISITS.ToString().Contains(SearchText)
			);
			}	

			if (SelectedFilterOption != null)
			{
				switch (SelectedFilterOption.SortingType)
				{
					case AbonementSortingType.PriceAsc:
						query = query.OrderBy(t => t.PRICE_ABONEMENT);
						break;
					case AbonementSortingType.PriceDesc:
						query = query.OrderByDescending(t => t.PRICE_ABONEMENT);
						break;
					case AbonementSortingType.VisitAsc:
						query = query.OrderBy(t => t.AMOUNT_OF_VISITS);
						break;
					case AbonementSortingType.VisitDesc:
						query = query.OrderByDescending(t => t.AMOUNT_OF_VISITS);
						break;
				}
			}

			if (SelectedPriceRange != null)
			{
				query = query.Where(t =>
					t.PRICE_ABONEMENT >= SelectedPriceRange.MinPrice &&
					t.PRICE_ABONEMENT <= SelectedPriceRange.MaxPrice
				);
			}

			if (SelectedAmountOfVisitsRange != null)
			{
				query = query.Where(t =>
					t.AMOUNT_OF_VISITS >= SelectedAmountOfVisitsRange.MinAmountOfVisits &&
					t.AMOUNT_OF_VISITS <= SelectedAmountOfVisitsRange.MaxAmountOfVisits
				);
			}

			FilteredAbonements = new ObservableCollection<ABONEMENTS>(query.ToList());
			SelectedPriceRange = null;
			SelectedAmountOfVisitsRange = null;
		}

	}
}
