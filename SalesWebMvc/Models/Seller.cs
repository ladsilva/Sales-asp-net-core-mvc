using System;
using System.Collections.Generic;
using System.Linq;

namespace SalesWebMvc.Models
{
    public class Seller
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public double BaseSalary { get; set; }
        public Department Department { get; set; }
        public int DepartmentId { get; set; }// Integridade relacional, não nula para o departamento, (tipo int(struct) não permite nulo, faz com que o compilador obrigue a informar um dado para o campo)
        public ICollection<SalesRecord> Sales { get; set; } = new List<SalesRecord>();

        // Construtores
        public Seller()
        {
        }

        public Seller(int id, string name, string email, DateTime birthDate, double baseSalary, Department department)
        {
            Id = id;
            Name = name;
            Email = email;
            BirthDate = birthDate;
            BaseSalary = baseSalary;
            Department = department;
        }

        // Método para adicionar uma venda
        public void AddSales(SalesRecord sale)
        {
            Sales.Add(sale);
        }

        // Método para adicionar uma venda
        public void RemoveSales(SalesRecord sale)
        {
            Sales.Remove(sale);
        }

        // Total de vendas dado um período
        public double TotalSales(DateTime initial, DateTime final)
        {            
            return  Sales.Where(x => x.Date >= initial && x.Date <= final).Sum(x => x.Amount);            
        }
    }
}
