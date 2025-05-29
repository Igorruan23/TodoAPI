using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TodoAPI.Authenication;

namespace TodoAPI.Models
{
    public class ToDoItemModel
    {
        [Key]
        public int ItemId { get; set; } 

        [Required(ErrorMessage ="Nome do Item e necessário")]
        [Column(TypeName = "nvarchar(100)")]
        public string ItemName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Descrição do Item e necessário")]
        [Column(TypeName = "nvarchar(500)")]
        public string Description { get; set; }


        [Required(ErrorMessage = "Item Status e necessário")]
        [Column(TypeName = "bit")]
        public bool ItemStatus { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }
    }
}
