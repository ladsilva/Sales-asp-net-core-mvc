using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SalesWebMvc.Services;
using SalesWebMvc.Models;

namespace SalesWebMvc.Controllers
{
    public class SellersController : Controller
    {
        private readonly SellerService _sellerService;

        public SellersController(SellerService sellerService)// Injeção de dependência
        {
            _sellerService = sellerService;
        }

        public IActionResult Index()
        {
            // Operação retorna uma lista contendo todos os Sellers do banco, recuperados pelo service SellerService
            var list = _sellerService.FindAll();
            return View(list);
        }

        // GET
        public IActionResult Create()
        {
            return View();
        }

        // POST
        [HttpPost] // Anotation - informando post
        [ValidateAntiForgeryToken] // Previne que sejam feitos ataques csrf, evita o envio de dados maliciosos durante a sessão de autenticação
        public IActionResult Create(Seller seller )
        {
            _sellerService.Insert(seller); // Chama o método Insert do Service(SellerService)
            // Retorna a tela Index, contendo os dados dos Sellers
            return RedirectToAction(nameof(Index)); //Nameof, melhora a manutenção,não exige que seja feita alteração neste ponto, caso o titulo do método index seja alterado
        }

        // POST
        [HttpPost] // Anotation - informando post
        [ValidateAntiForgeryToken] // Previne que sejam feitos ataques csrf, evita o envio de dados maliciosos durante a sessão de autenticação
        public IActionResult Delete(Seller seller)
        {
            _sellerService.Delete(seller); // Chama o método Delete do Service(SellerService)
            // Retorna a tela Index, contendo os dados dos Sellers
            return RedirectToAction(nameof(Index)); //Nameof, melhora a manutenção,não exige que seja feita alteração neste ponto, caso o titulo do método index seja alterado
        }
    }
}