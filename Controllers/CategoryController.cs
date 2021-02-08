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
    public class CategoryController : ControllerBase
    {
        private MyDBContext myDbContext;

        public CategoryController(MyDBContext context)
        {
            myDbContext = context;
        }
        [HttpGet]
        public IList<Category> Get()
        {
            return (this.myDbContext.Categories.ToList());
        }
        [EnableCors("AllowSpecificOrigin")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Category> PostCategory([FromBody] Category category)
        {
            try
            {
                this.myDbContext.Categories.Add(category);
                this.myDbContext.SaveChanges();
                Console.WriteLine("CategoryCreated!");

                return new CreatedResult($"/categories/{category.Name.ToLower()}", category);
            }
            catch (Exception e)
            {
                return ValidationProblem(e.Message);
            }
        }
        [HttpPatch]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Category>> PatchCategoryAsync([FromRoute]
        int id, [FromBody] JsonPatchDocument<Category> patch)
        {
            Console.WriteLine("IN OUR Patch!!!!!!!!!!");
            try
            {

                var categoryDb = await this.myDbContext.Categories.FindAsync(id);
                if (categoryDb == null) return NotFound();

                patch.ApplyTo(categoryDb);

                if (!ModelState.IsValid || !TryValidateModel(categoryDb))
                    return ValidationProblem(ModelState);
                Console.WriteLine(id);

                this.myDbContext.SaveChanges();

                return Ok(categoryDb);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                return ValidationProblem(e.Message);
            }
        }

        [EnableCors("AllowSpecificOrigin")]
        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Category>> DeleteCategory(int id)
        {

            var category = await this.myDbContext.Categories.FindAsync(id);


            if (category == null)
            {
                return NotFound();
            }

            this.myDbContext.Categories.Remove(category);
            await this.myDbContext.SaveChangesAsync();

            return category;
        }
    }
}
