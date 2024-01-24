using ApplicationWPF.Commands;
using ApplicationWPF.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Win32;
using System.Windows;
using System.IO;

namespace ApplicationWPF.ViewModels
{
    class ChangeAbonementViewModel : ViewModelBase
    {
		private readonly MyDbContext _dbContext = new MyDbContext();
		private ABONEMENTS _selectedAbonement;
		private byte[] _image;

		public decimal PriceAbon
		{
			get => _selectedAbonement.PRICE_ABONEMENT;
			set
			{
				if (_selectedAbonement != null && value >= 0)
				{
					_selectedAbonement.PRICE_ABONEMENT = value;
					OnPropertyChanged(nameof(PriceAbon));
				}
				else
				{
					MessageBox.Show("Не может быть отрицательного числа для данного поля!");
				}
			}
		}

		public int CountVisit
		{
			get => _selectedAbonement.AMOUNT_OF_VISITS;
			set
			{
				if (_selectedAbonement != null && value >= 0)
				{
					_selectedAbonement.AMOUNT_OF_VISITS = value;
					OnPropertyChanged(nameof(CountVisit));
				}
				else
				{
					MessageBox.Show("Не может быть отрицательного числа для данного поля!.");
				}
			}
		}

		public byte[] Image
		{
			get => _selectedAbonement.IMG_ABONEMENT;
			set
			{
				if (_selectedAbonement != null)
				{
					_selectedAbonement.IMG_ABONEMENT = value;
					OnPropertyChanged(nameof(Image));
				}
			}
		}

		public string TypeAbon
		{
			get => _selectedAbonement.TYPE_OF_ABONEMENT;
			set
			{
				if (_selectedAbonement != null)
				{
					_selectedAbonement.TYPE_OF_ABONEMENT = value;
					OnPropertyChanged(nameof(TypeAbon));
				}
			}
		}

		public string TypeExec
		{
			get => _selectedAbonement.TYPE_OF_EXERCISE;
			set
			{
				if (_selectedAbonement != null)
				{
					_selectedAbonement.TYPE_OF_EXERCISE = value;
					OnPropertyChanged(nameof(TypeExec));
				}
			}
		}

		public string Service
		{
			get => _selectedAbonement.ADDITIONAL_SERVICE;
			set
			{
				if (_selectedAbonement != null)
				{
					_selectedAbonement.ADDITIONAL_SERVICE = value;
					OnPropertyChanged(nameof(Service));
				}
			}
		}

		public int ID_SHEDULE
		{
			get => _selectedAbonement.ID_SHEDULE;
			set
			{
				if (_selectedAbonement != null && value >= 0)
				{
					_selectedAbonement.ID_SHEDULE = value;
					OnPropertyChanged(nameof(ID_SHEDULE));
				}
				else
				{
					MessageBox.Show("Не может быть отрицательного числа для данного поля!.");
				}
			}
		}

		public ICommand ExitCommand { get; private set; }
		public ICommand EditAbonementCommand { get; private set; }
		public ICommand SelectImageCommand { get; private set; }


		public ChangeAbonementViewModel()
		{

		}

		public ChangeAbonementViewModel(ABONEMENTS selectedAbonement)
		{
			_selectedAbonement = selectedAbonement;
			ExitCommand = new RelayCommand(parameter => ExitApp());
			EditAbonementCommand = new RelayCommand(EditAbonement);
			SelectImageCommand = new RelayCommand(SelectImage);
			PriceAbon = _selectedAbonement.PRICE_ABONEMENT;
			CountVisit = _selectedAbonement.AMOUNT_OF_VISITS;
			Image = _selectedAbonement.IMG_ABONEMENT;
			TypeAbon = _selectedAbonement.TYPE_OF_ABONEMENT;
			TypeExec = _selectedAbonement.TYPE_OF_EXERCISE;
			Service = _selectedAbonement.ADDITIONAL_SERVICE;
		}

		public void EditAbonement(object parameter)
		{
			try
			{
				var Abonement = _dbContext.ABONEMENTS.Find(_selectedAbonement.ID_ABONEMENT);
		

				if (Abonement != null)
				{
					var scheduleExists = _dbContext.SHEDULE_TABLE.Any(s => s.ID_SHEDULE == _selectedAbonement.ID_SHEDULE);
					Abonement.PRICE_ABONEMENT = _selectedAbonement.PRICE_ABONEMENT;
					Abonement.AMOUNT_OF_VISITS = _selectedAbonement.AMOUNT_OF_VISITS;
					Abonement.IMG_ABONEMENT = _selectedAbonement.IMG_ABONEMENT;
					Abonement.TYPE_OF_ABONEMENT = _selectedAbonement.TYPE_OF_ABONEMENT;
					Abonement.TYPE_OF_EXERCISE = _selectedAbonement.TYPE_OF_EXERCISE;
					Abonement.ADDITIONAL_SERVICE = _selectedAbonement.ADDITIONAL_SERVICE;
					Abonement.ID_SHEDULE = _selectedAbonement.ID_SHEDULE;
					if (_image != null)
						Abonement.IMG_ABONEMENT = _image;
					_dbContext.SaveChanges();
					MessageBox.Show("Абонемент успешно изменен!");
					CloseWindow();

				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка при изменении абонемента: {ex.Message}");
			}
		}

		private void ExitApp()
		{
			CloseWindow();
		}

		public void SelectImage(object parameter)
		{
			var openFileDialog = new OpenFileDialog
			{
				Title = "Выберите новое изображение",
				Filter = "Image files (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg|All files (*.*)|*.*"
			};

			if (openFileDialog.ShowDialog() == true)
			{
				var imagePath = openFileDialog.FileName;
				_image = File.ReadAllBytes(imagePath);
				OnPropertyChanged(nameof(Image));
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
