using System;
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

        // Recupera todos os departaments
        public List<Department> FindAll()
        {
            return _context.Department.OrderBy(ord => ord.Name).ToList(); // Recupera departments ordenados do banco por nome, em forma de lista
        }
    }
}
