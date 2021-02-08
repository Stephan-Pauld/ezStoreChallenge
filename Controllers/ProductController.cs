using ezStore.DBContexts;
using ezStore.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ezStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private MyDBContext myDbContext;

        public ProductController(MyDBContext context)
        {
            myDbContext = context;
        }

        [HttpGet]
        
        public List<Product> Get()
        {
            return (this.myDbContext.Products.ToList());
         }

        // DELETE: api/Products/5
        [EnableCors("AllowSpecificOrigin")]
        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Product>> DeleteProducts(int id)
        {

            var products = await this.myDbContext.Products.FindAsync(id);
          

            if (products == null)
            {
                return NotFound();
            }

            this.myDbContext.Products.Remove(products);
            await this.myDbContext.SaveChangesAsync();

            return products;
        }

        private bool ProductExists(int id)
        {
            return this.myDbContext.Products.Any(e => e.Id == id);
        }

        [EnableCors("AllowSpecificOrigin")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Product> PostProduct([FromBody] Product product)
        {
            try
            {
                Console.WriteLine(product);
                this.myDbContext.Products.Add(product);
                this.myDbContext.SaveChanges();
                Console.WriteLine("ProductCreated!");

                return new CreatedResult($"/products/{product.Name.ToLower()}", product);
            }
            catch (Exception e)
            {
                Console.WriteLine("Whoops");

                return ValidationProblem(e.Message);
            }
        }

        [HttpPatch]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Product>> PatchProductAsync([FromRoute]
        int id, [FromBody] JsonPatchDocument<Product> patch)
        {
                Console.WriteLine("IN OUR Patch!!!!!!!!!!");
            try
            {

                var productDb = await this.myDbContext.Products.FindAsync(id);
                if (productDb == null) return NotFound();

                patch.ApplyTo(productDb);

                if (!ModelState.IsValid || !TryValidateModel(productDb))
                    return ValidationProblem(ModelState);
                Console.WriteLine(id);

                this.myDbContext.SaveChanges();

                return Ok(productDb);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                return ValidationProblem(e.Message);
            }
        }
    }
}
