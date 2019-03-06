﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SalesWebMvc.Models;
using Microsoft.EntityFrameworkCore;

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

        // Método para retornar um vendedor(Seller)
        public Seller FindById(int id)
        {
            // Objeto _context (do tipo SalesWebMvcContext) acessa o banco, pega o seller da tabela Seller de acordo com o id
            // Se não encontrar retorna nulo
            // Uso do Include é o eager loading, faz um join com department para que seja exibido na tela
            // Carrega outros objetos atrelados ao objeto principal(Join das tabelas)
            return _context.Seller.Include(obj => obj.Department).FirstOrDefault( codid => codid.Id == id );
        }

        public void Insert (Seller obj)
        {            
            _context.Add(obj); // Insere o Seller
            _context.SaveChanges(); // Insere e grava o novo Seller no banco de dados
        }

        // Remove a partir do id do Seller
        public void Remove(int id)
        {
            var obj = _context.Seller.Find(id); // Recupera o Seller de acordo com o id
            _context.Seller.Remove(obj); // Remove o Seller retorno em obj
            _context.SaveChanges(); // Remove e grava a remoção no banco de dados
        }

        public void Update(Seller obj)
        {
            _context.Update(obj); // Atualiza o Seller
            _context.SaveChanges(); // Atualiza e grava o novo Seller no banco de dados
        }
    }
}
