using System;
using System.Linq;
using System.Collections.Generic;
namespace SalesWebMvc.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Seller> Sellers { get; set; } = new List<Seller>();

        // Construtores
        public Department()
        {
        }

        public Department(int id, string name)
        {
            Id = id;
            Name = name;
        }

        // Método para adicionar vendedor
        public void AddSeller (Seller seller)
        {
            Sellers.Add(seller);
        }

        // Total de vendas de um departamento dado um período
        public double TotalSales(DateTime initial, DateTime final)
        {
            // Sum retorna uma Lista de vendedores(Sellers), a partir desta lista é chamado o método TotalSales do vendedor(Seller)
            return Sellers.Sum(seller => seller.TotalSales(initial, final)); 
        }
    }
}
