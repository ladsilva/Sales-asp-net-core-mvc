using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SalesWebMvc.Services;

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
    }
}