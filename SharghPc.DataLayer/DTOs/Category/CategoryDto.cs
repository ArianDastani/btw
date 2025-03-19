
using System.ComponentModel.DataAnnotations;

namespace SharghPc.DataLayer.DTOs.Category
{
    public class CategoryDto
    {
        public long Id { get; set; }
        public long? ParentId { get; set; }

        [Display(Name = "عنوان")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string Title { get; set; }

        [Display(Name = "عنوان در URL")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string UrlName { get; set; }

        
    }
}
