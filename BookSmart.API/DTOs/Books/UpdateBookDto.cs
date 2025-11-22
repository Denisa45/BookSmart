using System.ComponentModel.DataAnnotations;

namespace BookSmart.API.DTOs.Books
{
    public class UpdateBookDto
    {
        [Required(ErrorMessage = "Title is required.")]
        [StringLength(150, MinimumLength = 2,
            ErrorMessage = "Title must be between 2 and 150 characters.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Author is required.")]
        [StringLength(100, MinimumLength = 2,
            ErrorMessage = "Author name must be between 2 and 100 characters.")]
        public string Author { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Total copies must be at least 1.")]
        public int TotalCopies { get; set; }
    }
}
