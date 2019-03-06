using System.Collections.Generic;

namespace SalesWebMvc.Models.ViewModels
{
    public class SellerFormViewModel // Classe para receber e recupear os departments
    {
        public Seller Seller { get; set; } // Possui um Seller
        public ICollection<Department> Departments { get; set; } // Lista de departments
    }
}
