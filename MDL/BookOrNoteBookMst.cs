using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace MDL
{

    public class BookOrNoteBookMst : IValidatableObject
    {
        public int PK_BookId { get; set; }

        [Required(ErrorMessage = "Type field is required.")]
        [StringLength(50, ErrorMessage = "Type cannot exceed 50 characters.")]
        public string Type { get; set; }

        [StringLength(200, ErrorMessage = "Book Name cannot exceed 200 characters.")]
        public string BookName { get; set; } = string.Empty;
        public string ClassName { get; set; } = string.Empty;
        public int? NoteBookPageCount { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, 99999.99, ErrorMessage = "Price must be between 0.01 and 99999.99.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }

        [Required(ErrorMessage = "Created By field is required.")]
        public int CreatedBy { get; set; }

        public string CreatedDateTime { get; set; }

        public int? UpdatedBy { get; set; }
        public string UpdatedDateTime { get; set; }
        public string Status { get; set; }
        public int CompID { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> errors = new List<ValidationResult>();

            if (Type == "Book")
            {
                if (string.IsNullOrWhiteSpace(BookName))
                {
                    errors.Add(new ValidationResult("Book Name is required when Type is Book.", new[] { "BookName" }));
                }
                if (string.IsNullOrWhiteSpace(ClassName))
                {
                    errors.Add(new ValidationResult("Class Name is required when Type is Book.", new[] { "ClassName" }));
                }
            }
            else if (Type == "Notebook")
            {
                if (NoteBookPageCount == null || NoteBookPageCount < 1 || NoteBookPageCount > 1000)
                {
                    errors.Add(new ValidationResult("Notebook Page Count must be between 1 and 1000 when Type is Notebook.", new[] { "NoteBookPageCount" }));
                }
            }

            return errors;
        }
    }
}
