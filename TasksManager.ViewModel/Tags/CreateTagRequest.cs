using System.ComponentModel.DataAnnotations;

namespace TasksManager.ViewModel.Tags
{
    public class CreateTagRequest
    {
        [Required]
        [MaxLength(64)]
        public string Name { get; set; }
    }
}
