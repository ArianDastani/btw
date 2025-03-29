using System.ComponentModel.DataAnnotations;

namespace SharghPc.DataLayer.DTOs.Product
{
    public class ProductForIndexDto
    {
        public long Id { get; set; }

        [Display(Name = "نام محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(300, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string Title { get; set; }

        [Display(Name = "تصویر محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(300, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string ImageName { get; set; }

        [Display(Name = "قیمت محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int Price { get; set; }

        [Display(Name = "موجودی محصول")]
        public int Inventory { get; set; }

        public string? ViewConter { get; set; }
        public bool? IsDelete { get; set; }
        public DateTime? CreateDate { get; set; }


    }
}
