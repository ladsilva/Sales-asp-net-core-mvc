using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Services;
using SalesWebMvc.Models;
using SalesWebMvc.Models.ViewModels;
using SalesWebMvc.Services.Exceptions;
using System.Diagnostics;

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

        public async Task<IActionResult> Index()
        {
            // Operação retorna uma lista contendo todos os Sellers do banco, recuperados pelo service SellerService
            var list = await _sellerService.FindAllAsync();
            return View(list);
        }

        // GET
        public async Task<IActionResult> Create()
        {
            var listdepartments = await _departmentService.FindAllAsync(); // Recuper os departments do banco
            var viewmodel = new SellerFormViewModel { Departments = listdepartments }; // Instancia viewmodel do tipo SellerFormViewModel com os dados de departments do banco
            return View(viewmodel); // Passa para tela, os dados de departments já instanciados
        }

        // POST
        [HttpPost] // Anotation - informando post
        [ValidateAntiForgeryToken] // Previne que sejam feitos ataques csrf, evita o envio de dados maliciosos durante a sessão de autenticação
        public async Task<IActionResult> Create(Seller seller)
        {
            // Validação caso o javascript não esteja habilitado
            if (!ModelState.IsValid)
            {
                var departments = await _departmentService.FindAllAsync();
                var selerviewmodel = new SellerFormViewModel { Seller = seller, Departments = departments };
                return View(selerviewmodel);
            }

            await _sellerService.InsertAsync(seller); // Chama o método Insert do Service(SellerService)
            // Redireciona a tela Index, contendo os dados dos Sellers
            return RedirectToAction(nameof(Index)); //Nameof, melhora a manutenção,não exige que seja feita alteração neste ponto, caso o titulo do método index seja alterado
        }

        // GET
        public async Task<IActionResult> Delete(int? id) // int id é opcional uso do "?"
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not provided"});
            }

            // Recupera o Seller do banco através do id
            var obj = await _sellerService.FindByIdAsync(id.Value);

            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found" });
            }
            // Se objeto encontrado, retorna a página com os dados do objeto
            return View(obj);
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _sellerService.RemoveAsync(id); // Chama método para remover dado do Sellerservice
            return RedirectToAction(nameof(Index)); // Redireciona para a tela Index dos Sellers
        }

        //GET
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });
            }

            // Recupera o Seller do banco através do id
            var obj = await _sellerService.FindByIdAsync(id.Value);

            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found" });
            }
            // Se objeto encontrado, retorna a página com os dados do objeto
            return View(obj);
        }

        //GET
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });
            }

            // Recupera o Seller do banco através do id em obj
            var obj = await _sellerService.FindByIdAsync(id.Value);

            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found" });
            }

            // Recupera lista de departments para seleção
            List<Department> listdepartments = await _departmentService.FindAllAsync();

            // Instancia objeto sellerformviewmodel
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
        public async Task<IActionResult> Edit(int id, Seller seller)
        {
            // Validação caso o javascript não esteja habilitado
            if (!ModelState.IsValid)
            {
                var departments = await _departmentService.FindAllAsync();
                var selerviewmodel = new SellerFormViewModel { Seller = seller, Departments = departments };
                return View(selerviewmodel);
            }
            if (id != seller.Id) // Testa se o id é igual ao id do objeto Seller
            {
                return RedirectToAction(nameof(Error), new { message = "Id mismatch" });
            }
            try
            {
                await _sellerService.UpdateAsync(seller); // Chama método para atualizar dado do Sellerservice
                return RedirectToAction(nameof(Index)); // Redireciona para a tela Index dos Sellers
            }
            catch (NotFoundException e)
            {
                return RedirectToAction(nameof(Error), e.Message);
            }
            catch(DbConcurrencyException e)
            {
                return RedirectToAction(nameof(Error), e.Message);
            }
        }

        // GET
        public IActionResult Error(string message)
        {
            var viewmodel = new ErrorViewModel

            // ? torna o id opcional, ?? operador de coalescencia nula
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, // Recupera o id interno da requisição
                Message = message
            };

            return View(viewmodel);
        }
    }
}