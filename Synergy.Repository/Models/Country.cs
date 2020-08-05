using System.Collections;
using System.Collections.Generic;

namespace Synergy.Repository.Models
{
  public    class Country
    {
        public Country()
        {
            CustomerAccounts = new HashSet<CustomerAccount>();
        }
       
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public string CountryShortCode { get; set; }
        public string DailingCode { get; set; }
        public string CountryImageName { get; set; }
        public ICollection<CustomerAccount> CustomerAccounts { get; set; }      
    }
}
