using ApplicationWPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationWPF.ViewModels
{
    public class RequestViewModel : ViewModelBase
    {
        private ABONEMENT_REQUEST _request;
        private ABONEMENTS _abonement;
        private CLIENTS _clients;

        public RequestViewModel(ABONEMENT_REQUEST request, ABONEMENTS abonement, CLIENTS client) 
        {
            _request = request;
            _abonement = abonement;
            _clients = client;

            Request_id = _request.ID_REQUEST;
            Date_request = _request.DATA_REQUEST;
			Price = _abonement.PRICE_ABONEMENT;
			Amount = _abonement.AMOUNT_OF_VISITS;
			Type = _abonement.TYPE_OF_ABONEMENT;
			Last_name = _clients.LASTNAME_CLIENT;
            First_name = _clients.FIRSTNAME_CLIENT;
            Middle_name = _clients.MIDDLENAME_CLIENT;
        }

        public int Request_id { get; set; }
        public DateTime? Date_request { get; set; }
		public string Last_name { get; set; }
		public string First_name { get; set; }
		public string Middle_name { get; set; }
        public decimal Price { get; set; }
        public int Amount { get; set; }
        public string Type { get; set; }
	}
}
