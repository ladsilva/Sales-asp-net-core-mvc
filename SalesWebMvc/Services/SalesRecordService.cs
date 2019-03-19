using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SalesWebMvc.Models;
using Microsoft.EntityFrameworkCore;

namespace SalesWebMvc.Services
{
    public class SalesRecordService
    {
        private readonly SalesWebMvcContext _context; // Dependência de SalesWebMVcContext - acesso ao banco

        public SalesRecordService(SalesWebMvcContext context) // Injeção de dependência
        {
            _context = context;
        }

        // Busca salesrecord por data opcional
        public async Task<List<SalesRecord>> FindByDateAsync(DateTime? minDate, DateTime? maxDate)
        {
            var sales = from obj in _context.SalesRecord select obj;
            if (minDate.HasValue)
            {
                sales = sales.Where(x => x.Date >= minDate.Value);
            }
            
            if (maxDate.HasValue)
            {                
                sales = sales.Where(x => x.Date <= maxDate.Value);
            }

            return await sales
                .Include(slr => slr.Seller) // join Seller
                .Include(dep => dep.Seller.Department) // join Department
                .OrderByDescending(x => x.Date).ToListAsync(); 
                ;
        }

        // Busca agrupda de salesrecord por data opcional, com agrupamento por department
        // Tipo de retorno alterado para IGrouping<Department, SalesRecord>, devido ao groupby department
        public async Task<List<IGrouping<Department, SalesRecord>>> FindByDateGroupingAsync(DateTime? minDate, DateTime? maxDate)
        {
            var sales = from obj in _context.SalesRecord select obj;
            if (minDate.HasValue)
            {
                sales = sales.Where(x => x.Date >= minDate.Value);
            }

            if (maxDate.HasValue)
            {
                sales = sales.Where(x => x.Date <= maxDate.Value);
            }

            return await sales
                .Include(slr => slr.Seller) // join Seller
                .Include(dep => dep.Seller.Department) // join Department                
                .OrderByDescending(x => x.Date)
                .GroupBy(x => x.Seller.Department) // groupby por department,mudo o tipo de retorno para Igrouping<Department,SalesRecord>
                .ToListAsync();
            ;
        }

    }
}
