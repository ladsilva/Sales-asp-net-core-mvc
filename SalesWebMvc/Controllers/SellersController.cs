using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using SalesWebMvc.Services;
using SalesWebMvc.Models;
using SalesWebMvc.Models.ViewModels;
using SalesWebMvc.Services.Exceptions;

namespace SalesWebMvc.Controllers
{
    public class SellersController : Controller
    {
        private readonly SellerService _sellerService;
        private readonly DepartmentService _departmentService;

        public SellersController(SellerService sellerService, DepartmentService departmentService)// Injeção de dependência
        {
            _sellerService = sellerService;
            _departmentService = departmentService;
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
            var listdepartments = _departmentService.FindAll(); // Recuper os departments do banco
            var viewmodel = new SellerFormViewModel { Departments = listdepartments }; // Instancia viewmodel do tipo SellerFormViewModel com os dados de departments do banco
            return View(viewmodel); // Passa para tela, os dados de departments já instanciados
        }

        // POST
        [HttpPost] // Anotation - informando post
        [ValidateAntiForgeryToken] // Previne que sejam feitos ataques csrf, evita o envio de dados maliciosos durante a sessão de autenticação
        public IActionResult Create(Seller seller)
        {
            _sellerService.Insert(seller); // Chama o método Insert do Service(SellerService)
            // Redireciona a tela Index, contendo os dados dos Sellers
            return RedirectToAction(nameof(Index)); //Nameof, melhora a manutenção,não exige que seja feita alteração neste ponto, caso o titulo do método index seja alterado
        }

        // GET
        public IActionResult Delete(int? id) // int id é opcional uso do "?"
        {
            if (id == null)
            {
                return NotFound();
            }

            // Recupera o Seller do banco através do id
            var obj = _sellerService.FindById(id.Value);

            if (obj == null)
            {
                return NotFound();
            }
            // Se objeto encontrado, retorna a página com os dados do objeto
            return View(obj);
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            _sellerService.Remove(id); // Chama método para remover dado do Sellerservice
            return RedirectToAction(nameof(Index)); // Redireciona para a tela Index dos Sellers
        }

        //GET
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Recupera o Seller do banco através do id
            var obj = _sellerService.FindById(id.Value);

            if (obj == null)
            {
                return NotFound();
            }
            // Se objeto encontrado, retorna a página com os dados do objeto
            return View(obj);
        }

        //GET
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Recupera o Seller do banco através do id em obj
            var obj = _sellerService.FindById(id.Value);

            if (obj == null)
            {
                return NotFound();
            }

            // Recupera lista de departments para seleção
            List<Department> listdepartments = _departmentService.FindAll();

            // 
            SellerFormViewModel viewmodel = new SellerFormViewModel
            {
                Seller = obj, // Abaste os dados do Seller com o objeto obj recuperado pelo id(acima)
                Departments = listdepartments
            }; // Instancia viewmodel do tipo SellerFormViewModel com os dados de departments do banco
            return View(viewmodel);

        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Seller seller)
        {
            if (id != seller.Id) // Testa se o id é igual ao id do objeto Seller
            {
                return BadRequest();
            }
            try
            {
                _sellerService.Update(seller); // Chama método para atualizar dado do Sellerservice
                return RedirectToAction(nameof(Index)); // Redireciona para a tela Index dos Sellers
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch(DbConcurrencyException)
            {
                return BadRequest();
            }
        }
    }
}