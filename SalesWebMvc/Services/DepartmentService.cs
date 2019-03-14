using System;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using SalesWebMvc.Models;

namespace SalesWebMvc.Services
{
    public class DepartmentService
    {
        private readonly SalesWebMvcContext _context; // Dependência de SalesWebMVcContext - acesso ao banco

        public DepartmentService(SalesWebMvcContext context) // Injeção de dependência
        {
            _context = context;
        }

        // Recupera todos os departaments de forma assicrona, evitando que o compilador aguarde a execução do acesso ao banco
        public async Task<List<Department>> FindAllAsync() //Task-forma assyncrona
        {
            return await _context.Department.OrderBy(ord => ord.Name).ToListAsync() ; // Recupera departments ordenados do banco por nome, em forma de lista
            // tolistassync-lista assincrona await, informa ao compilador que a execução do acesso ao banco é assincrona
        }
    }
}
