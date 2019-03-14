using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SalesWebMvc.Models;
using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Services.Exceptions;

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
        public async Task<List<Seller>> FindAllAsync()
        {
            // Objeto _context (do tipo SalesWebMvcContext) acessa o banco, pegando dados da tabela Seller, e joga para uma lista
            return await _context.Seller.ToListAsync();
        }

        // Método para retornar um vendedor(Seller)
        public async Task<Seller> FindByIdAsync(int id)
        {
            // Objeto _context (do tipo SalesWebMvcContext) acessa o banco, pega o seller da tabela Seller de acordo com o id
            // Se não encontrar retorna nulo
            // Uso do Include é o eager loading, faz um join com department para que seja exibido na tela
            // Carrega outros objetos atrelados ao objeto principal(Join das tabelas)
            return await _context.Seller.Include(obj => obj.Department).FirstOrDefaultAsync( codid => codid.Id == id );
        }

        public async Task InsertAsync (Seller obj)
        {            
            _context.Add(obj); // Insere o Seller
            await _context.SaveChangesAsync(); // Insere e grava o novo Seller no banco de dados
        }

        // Remove a partir do id do Seller
        public async Task RemoveAsync(int id)
        {
            try
            {
                var obj = await _context.Seller.FindAsync(id); // Recupera o Seller de acordo com o id
                _context.Seller.Remove(obj); // Remove o Seller retorno em obj
                await _context.SaveChangesAsync(); // Remove e grava a remoção no banco de dados
            }
            catch (DbUpdateException e)
            {
                throw new IntegrityException(e.Message);
            }
        }

        public async Task UpdateAsync(Seller obj)
        {
            bool hasAny = await _context.Seller.AnyAsync(x => x.Id == obj.Id);
            // Se não existir o registro no banco gera uma exceção NotFoundException
            if (!hasAny) // Any testa se existe registro no banco
            {
                throw new NotFoundException ("Id not found");
            }
            try
            {
                _context.Update(obj); // Atualiza o Seller
                await _context.SaveChangesAsync(); // Atualiza e grava o Seller no banco de dados
            }
            // Se ocorrer exceçao do banco por conflito de concorrência no banco(DbUpdateConcurrencyException)
            // Recupera a exceção de banco e a relança como uma exceção de nível de serviço(separando as camadas)
            catch (DbUpdateConcurrencyException e)
            {
                throw new DbConcurrencyException(e.Message); // Lança a exceção em nível de serviço retorna(SellerController controla a exceção de serviço)
            }
        }
    }
}
