using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SalesWebMvc.Models;

namespace SalesWebMvc.Services
{
    public class SellerService
    {
        private readonly SalesWebMvcContext _context; // Dependência

        public SellerService( SalesWebMvcContext context ) // Injeção de dependência SalesWebMvcContext
        {
            _context = context;
        }

        // Método para retornar todos os vendedores(Sellers)
        public List<Seller> FindAll()
        {
            // Objeto _context (do tipo SalesWebMvcContext) acessa o banco, pegando dados da tabela Seller, e joga para uma lista
            return _context.Seller.ToList();
        }

        public void Insert (Seller obj)
        {
            _context.Add(obj); // Insere o Seller
            _context.SaveChanges(); // Insere e grava o novo Seller no banco de dados
        }

        public void Delete(Seller obj)
        {
            _context.Remove(obj); // Remove o Seller
            _context.SaveChanges(); // Remove e grava o novo Seller no banco de dados
        }

        public void Update(Seller obj)
        {
            _context.Update(obj); // Atualiza o Seller
            _context.SaveChanges(); // Atualiza e grava o novo Seller no banco de dados
        }
    }
}
