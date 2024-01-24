using ApplicationWPF.Commands;
using ApplicationWPF.Models;
using ApplicationWPF.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows;

namespace ApplicationWPF.ViewModels
{
    class MoreDetailsViewModel : ViewModelBase
    {
        private ABONEMENTS _abon;
		private CLIENTS _currentUser;
		public string typeAbon => _abon.TYPE_OF_ABONEMENT;
        public decimal price => _abon.PRICE_ABONEMENT;
        public string typeExec => _abon.TYPE_OF_EXERCISE;
        public string aditiol => _abon.ADDITIONAL_SERVICE;
        public int amountVis => _abon.AMOUNT_OF_VISITS;

		public ICommand ShowBookTourPage { get; private set; }

		public BitmapImage ImageSource { get; set; }

		private BitmapImage ByteArrayToBitmapImage(byte[] byteArray)
		{
			if (byteArray == null || byteArray.Length == 0)
				return null;

			var image = new BitmapImage();
			using (var stream = new MemoryStream(byteArray))
			{
				image.BeginInit();
				image.CacheOption = BitmapCacheOption.OnLoad;
				image.StreamSource = stream;
				image.EndInit();
			}

			return image;
		}

		public MoreDetailsViewModel()
		{
		}

		public MoreDetailsViewModel(ABONEMENTS abonement, CLIENTS currentUser)
		{
			_currentUser = currentUser;
			_abon = abonement;
			ImageSource = ByteArrayToBitmapImage(abonement.IMG_ABONEMENT);
			//ShowBookTourPage = new RelayCommand(ConfirmBooking);
		}

		//private void ConfirmBooking(object parameter)
		//{
		//	BookTourViewModel confirmBookingViewModel = new BookTourViewModel(_tour, _currentUser);
		//	BookTour confirmBookingPage = new BookTour { DataContext = confirmBookingViewModel };
		//	confirmBookingPage.ShowDialog();
		//	CloseWindow();
		//}

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
