using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharghPc.DataLayer.DTOs.Category
{
    public class EditCategoriesDto
    {
        public long ProductCategoryId { get; set; }
        public long? ParentId { get; set; }

        [Display(Name = "عنوان")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string? Title { get; set; }

        [Display(Name = "عنوان در URL")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string? UrlName { get; set; } 
    }
}
