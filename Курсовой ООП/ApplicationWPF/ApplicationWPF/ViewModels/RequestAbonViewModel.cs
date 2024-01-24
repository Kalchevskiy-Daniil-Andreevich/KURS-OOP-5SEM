using ApplicationWPF.Commands;
using ApplicationWPF.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ApplicationWPF.ViewModels
{
    class RequestAbonViewModel : ViewModelBase
	{
		private ABONEMENTS _abonement;
		private CLIENTS _currentUser;
		private ABONEMENT_REQUEST _typeTrain;
		private ABONEMENT_REQUEST _typeOfService;
		private int _idAonement;

		private readonly MyDbContext _dbContext = new MyDbContext();

		private bool _isConfirmationChecked;
		public bool IsConfirmationChecked
		{
			get => _isConfirmationChecked;
			set
			{
				if (_isConfirmationChecked != value)
				{
					_isConfirmationChecked = value;
					OnPropertyChanged(nameof(IsConfirmationChecked));
				}
			}
		}


		public int IdAbonement
		{
			get => _idAonement;
			set
			{
				_idAonement = value;
				OnPropertyChanged();
			}
		}



		public ICommand ConfirmRequestCommand { get; }
		public ICommand ExitCommand { get; private set; }

		public RequestAbonViewModel()
        {
			
		}

        public RequestAbonViewModel(ABONEMENTS abonement, CLIENTS client)
        {
            _currentUser = client;
			_abonement = abonement;
			ConfirmRequestCommand = new RelayCommand(ConfirmRequest);
			ExitCommand = new RelayCommand(parameter => ExitApp());
		}

		private void ConfirmRequest(object parameter)
		{
			if (!IsConfirmationChecked)
			{
				MessageBox.Show("Подтвердите заявку перед продолжением.");
				return;
			}
			if (_currentUser == null)
			{
				MessageBox.Show("Текущий пользователь не определен");
				return;
			}

			var existingAbonement = _dbContext.ABONEMENTS.Find(IdAbonement);
			if (existingAbonement == null)
			{
				MessageBox.Show("Абонемент с указанным идентификатором не существует.");
				return;
			}

			if (_dbContext.ABONEMENT_REQUEST.Any(req => req.ID_ABONEMENT == IdAbonement))
			{
				MessageBox.Show("Абонемент с таким идентификатором уже оформлен.");
				return;
			}

			var request = new ABONEMENT_REQUEST
				{
					ID_CLIENT = _currentUser.ID_CLIENT,
					ID_ABONEMENT = IdAbonement,
					TYPE_OF_TRAINING = "fsdfsdf",
					TYPE_OF_SERVICE = "fdgdfgdfg",
					DATA_REQUEST = DateTime.Now
			    };

				_dbContext.ABONEMENT_REQUEST.Add(request);
				_dbContext.SaveChanges();
				MessageBox.Show("Абонемент успешно оформлен!");
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
