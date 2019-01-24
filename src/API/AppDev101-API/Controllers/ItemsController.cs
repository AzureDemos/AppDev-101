using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppDev101_API.Models;
using AppDev101_API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppDev101_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {

        private readonly IDocumentDBRepository<Item> Respository;
        public ItemsController(IDocumentDBRepository<Item> Respository)
        {
            this.Respository = Respository;
        }


        [HttpGet]
        public async Task<IEnumerable<Item>> GetAll()
        {
            var items = await Respository.GetItemsAsync(d => !d.Completed);
            return items;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetItem(string id)
        {
            var items = await Respository.GetItemsAsync(x => x.Id == id);
            var item =  items.FirstOrDefault();
            if (item == null)
                return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Item value)
        {
            var item = await Respository.CreateItemAsync(value);
            return Ok(item.Id);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(string id, [FromBody] Item value)
        {
            await Respository.UpdateItemAsync(id, value);
            return Ok();

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            await Respository.DeleteItemAsync(id);
            return Ok();
        }
    }
}
