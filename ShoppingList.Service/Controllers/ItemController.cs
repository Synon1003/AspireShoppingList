using Microsoft.AspNetCore.Mvc;
using ShoppingList.Core.Domain.Entities;
using ShoppingList.Core.Domain.RepositoryContracts;
using ShoppingList.Core.Dtos;
using ShoppingList.Core.Mappings;

namespace ShoppingList.Service.Controllers
{
    [ApiController]
    [Route("items")]
    public class ItemController : ControllerBase
    {
        private readonly IRepository<Item> _repository;
        private readonly ILogger<ItemController> _logger;

        public ItemController(IRepository<Item> repository, ILogger<ItemController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet()]
        public async Task<IActionResult> GetAllAsync()
        {
            var items = await _repository.GetAllAsync();
            return Ok(items.Select(item => item.ToDto()));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            Item? item = await _repository.GetByIdAsync(id);
            if (item == null)
            {
                _logger.LogWarning("Attempted to get non-existing item with Id={Id}", id);
                return NotFound();
            }

            return Ok(item.ToDto());
        }


        [HttpPost]
        public async Task<IActionResult> AddAsync(CreateItemDto itemDto)
        {
            var item = new Item
            {
                Id = Guid.NewGuid(),
                Name = itemDto.Name,
                Description = itemDto.Description,
                Price = itemDto.Price,
                Status = ItemStatus.NotPurchased,
                UpdatedAt = DateTime.UtcNow,

            };

            await _repository.InsertAsync(item);
            _logger.LogInformation("Created <Item Id={Id} Name={Name} Status={Status}>", item.Id, item.Name, item.Status);

            return CreatedAtAction(nameof(GetAsync), new { id = item.Id }, item.ToDto());
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchAsync(Guid id)
        {
            Item? existingItem = await _repository.GetByIdAsync(id);
            if (existingItem == null)
            {
                _logger.LogWarning("Attempted to change status of non-existing item with Id={Id}", id);
                return NotFound();
            }

            existingItem.Status = existingItem.Status == ItemStatus.NotPurchased ?
                ItemStatus.Purchased : ItemStatus.NotPurchased;
            existingItem.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(existingItem);
            _logger.LogInformation("Status changed <Item Id={Id} Name={Name} Status={Status}>",
                existingItem.Id, existingItem.Name, existingItem.Status);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            Item? existingItem = await _repository.GetByIdAsync(id);
            if (existingItem == null)
            {
                _logger.LogWarning("Attempted to delete non-existing item with Id={Id}", id);
                return NotFound();
            }

            await _repository.RemoveAsync(id);
            _logger.LogInformation("Deleted <Item Id={Id} Name={Name} Status={Status}>",
                 existingItem.Id, existingItem.Name, existingItem.Status);

            return NoContent();
        }
    }
}
