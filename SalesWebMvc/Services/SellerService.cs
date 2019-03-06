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
    }
}
