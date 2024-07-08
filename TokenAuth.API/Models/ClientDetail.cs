using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TokenAuth.API.Models
{
    public class ClientDetail
    {
        public int Id { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string ClientName { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}