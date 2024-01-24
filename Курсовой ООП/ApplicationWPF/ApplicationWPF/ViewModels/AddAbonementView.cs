using ApplicationWPF.Commands;
using ApplicationWPF.Models;
using ApplicationWPF.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;


namespace ApplicationWPF.ViewModels
{
	class AddAbonementView : ViewModelBase
	{
		private decimal _priceAbon;
		private int _countVisit;
		private int _idshedule;
		private byte[] _image;
		private string _typeAbon;
		private string _typeExec;
		private string _service;

		public decimal PriceAbon
		{
			get => _priceAbon;
			set
			{
				if (value >= 0)
				{
					_priceAbon = value;
					OnPropertyChanged(nameof(PriceAbon));
				}
			}
		}

		public int CountVisit
		{
			get => _countVisit;
			set
			{
				if (value >= 0)
				{
					_countVisit = value;
					OnPropertyChanged(nameof(CountVisit));
				}
			}
		}

		public byte[] Image
		{
			get => _image;
			set
			{
				_image = value;
				OnPropertyChanged(nameof(Image));
			}
		}

		public string TypeAbon
		{
			get => _typeAbon;
			set
			{ 
				_typeAbon = value;
				OnPropertyChanged(nameof(TypeAbon)); 
			}
		}

		public string TypeExec
		{
			get => _typeExec;
			set
			{
				_typeExec = value;
				OnPropertyChanged(nameof(TypeExec));
			}
		}

		public string Service
		{
			get => _service;
			set
			{
				_service = value;
				OnPropertyChanged(nameof(Service));
			}
		}

		public int ID_SHEDULE
		{
			get => _idshedule;
			set
			{
				if (value >= 0)
				{
					_idshedule = value;
					OnPropertyChanged(nameof(ID_SHEDULE));
				}
			}
		}

		public ICommand ExitCommand { get; private set; }
		public ICommand AddAbonementCommand { get; private set; }
		public ICommand SelectImageCommand { get; private set; }

		public AddAbonementView()
		{
			ExitCommand = new RelayCommand(parameter => ExitApp());
			AddAbonementCommand = new RelayCommand(AddAbons);
			SelectImageCommand = new RelayCommand(SelectImage);
		}

		private void ExitApp()
		{
			CloseWindow();
		}

		private void AddAbons(object parameter)
		{
			try
			{
				using (var dbContext = new MyDbContext())
				{
					var newAbon = new ABONEMENTS
					{
						PRICE_ABONEMENT = _priceAbon,
						AMOUNT_OF_VISITS = _countVisit,
						IMG_ABONEMENT = _image,
						TYPE_OF_ABONEMENT = _typeAbon,
						TYPE_OF_EXERCISE = _typeExec,
						ADDITIONAL_SERVICE = _service,
						ID_SHEDULE = _idshedule
					};

					dbContext.ABONEMENTS.Add(newAbon);
					dbContext.SaveChanges();
					MessageBox.Show("Абонемент успешно добавлен!");
					CloseWindow();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка при добавлении абонемента: {ex.Message}");
			}
		}



		private void SelectImage(object parameter)
		{
			var openFileDialog = new OpenFileDialog
			{
				Title = "Выберите изображение",
				Filter = "Image files (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg|All files (*.*)|*.*"
			};

			if (openFileDialog.ShowDialog() == true)
			{
				var imagePath = openFileDialog.FileName;
				_image = File.ReadAllBytes(imagePath);
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
