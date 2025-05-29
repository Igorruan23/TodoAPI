using AutoMapper;
using TodoAPI.Dtos;
using TodoAPI.Models;

namespace TodoAPI.Mappings
{
    public class TodoItemProfile:Profile
    {
        public TodoItemProfile()
        {
            CreateMap<ToDoItemModel, TodoItemDTO>();
            CreateMap<TodoItemDTO, ToDoItemModel>();
        }
    }
}
