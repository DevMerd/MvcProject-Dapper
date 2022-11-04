using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingCartMvc.Models;
using System.Data;
using System.Data.SqlClient;

namespace ShoppingCartMvc.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IConfiguration _configuration;


        public ProductController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        //GET
        public async Task<ActionResult<IEnumerable<Product>>> Index()
        {
            using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            {
                var sql = "SELECT * FROM Products";
                var products = await connection.QueryAsync<Product>(sql);
                //IEnumerable<Product> products = await SelectAllProducts(connection); 
                return View(products);
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<ActionResult<List<Product>>> Create(Product prdct)
        {
            if (ModelState.IsValid)
            {
                using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
                await connection.ExecuteAsync("INSERT INTO Products (Name, Quantity, Price) VALUES (@Name, @Quantity, @Price)", prdct);
                return RedirectToAction("Index");
            }
            return View(prdct);
        }

        //UPDATE
        public async Task<IActionResult> Edit(int? id)
        {
            using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            {
                var sql = "SELECT * FROM Products WHERE ProductId = @Id";
                var products = await connection.QueryFirstOrDefaultAsync<Product>(sql, new { Id = id });
                return View(products);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<List<Product>>> Edit(Product prdct)
        {
            if (ModelState.IsValid)
            {
                using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
                var sql = "SELECT * FROM Products WHERE ProductId = @Id";
                var products = await connection.QueryFirstOrDefaultAsync<Product>(sql, new { Id = prdct.ProductId });
                products.Name = prdct.Name;
                products.Quantity = prdct.Quantity;
                products.Price = prdct.Price;
                await connection.ExecuteAsync("UPDATE Products SET Name=@Name, Quantity=@Quantity, Price=@Price WHERE ProductId=@ProductId", products);
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("ERROR", "The product information requested to be registered is incorrect.");
            }
            return View(prdct);
        }

        // DELETE
        public async Task<IActionResult> Delete(int? id)
        {
            using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            {
                var sql = "SELECT * FROM Products WHERE ProductId = @Id";
                var products = await connection.QueryFirstOrDefaultAsync<Product>(sql, new { Id = id });
                return View(products);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePOST(int? id)
        {
            using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            var sql = "SELECT * FROM Products WHERE ProductId = @Id";
            var products = await connection.QueryFirstOrDefaultAsync<Product>(sql, new { Id = id });
            await connection.ExecuteAsync("DELETE FROM Products WHERE ProductId = @Id ", products);

            return RedirectToAction("Index");
        }
    }
}