using System.ComponentModel.DataAnnotations;

namespace Elixir.DATA.DTOs
{
    public class ArticleForm
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
    }
}