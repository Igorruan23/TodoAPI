using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TodoAPI.Dtos;
using TodoAPI.Models;

namespace TodoAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public TodoItemController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        // GET: api/ToDoItem
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItemDTO>>> GetTodoItems()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var isAdmin = User.IsInRole("Admin");

            var items = isAdmin
         ? await context.ToDoItems.ToListAsync()
         : await context.ToDoItems.Where(i => i.UserId == userId).ToListAsync();


            var dto = mapper.Map<List<TodoItemDTO>>(items);
            return Ok(dto);

        }

        // GET: api/ToDoItem/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItemDTO>> GetTodoItemModel(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var isAdmin = User.IsInRole("Admin");

            var todoItem = await context.ToDoItems.FindAsync(id);

            if (todoItem == null) return NotFound();
            if (!isAdmin && todoItem.UserId != userId) return Forbid();

            return mapper.Map<TodoItemDTO>(todoItem);
        }

        //PUT: api/ToDoItem/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItemModel(int id, TodoItemDTO dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var isAdmin = User.IsInRole("Admin");

            if (id != dto.ItemId)
            {
                return BadRequest();
            }

            var existingItem = await context.ToDoItems.AsNoTracking().FirstOrDefaultAsync(t => t.ItemId == id);
            if (existingItem == null)
            {
                return NotFound();
            }
            if (!isAdmin && existingItem.UserId != userId)
            {
                return Forbid();
            }

            var updateModel = mapper.Map<ToDoItemModel>(dto);
            updateModel.UserId = existingItem.UserId; // Preserve the original UserId

            context.Entry(updateModel).State = EntityState.Modified;


            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ToDoItemModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        // POST: api/ToDoItem
        [HttpPost]
        public async Task<ActionResult<TodoItemDTO>> PostTodoItemModel(TodoItemDTO dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized();

            var todoItem = mapper.Map<ToDoItemModel>(dto);
            todoItem.UserId = userId;

            context.ToDoItems.Add(todoItem);
            await context.SaveChangesAsync();

            var resultDto = mapper.Map<TodoItemDTO>(todoItem);
            return CreatedAtAction(nameof(GetTodoItemModel), new { id = todoItem.ItemId }, resultDto);
        }

        //DELETE: api/ToDoItem/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItemModel(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var isAdmin = User.IsInRole("Admin");

            var todoItemModel = await context.ToDoItems.FindAsync(id);
            if (todoItemModel == null)
            {
                return NotFound();
            }
            if (!isAdmin && todoItemModel.UserId != userId) return Forbid();
            context.ToDoItems.Remove(todoItemModel);
            await context.SaveChangesAsync();
            return NoContent();
        }

        private bool ToDoItemModelExists(int id)
        {
            return context.ToDoItems.Any(e => e.ItemId == id);
        }
    }
}
