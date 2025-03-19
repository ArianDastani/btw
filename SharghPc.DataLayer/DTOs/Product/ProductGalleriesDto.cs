namespace SharghPc.DataLayer.DTOs.Product
{
    public class ProductGalleriesDto
    {
        public long Id { get; set; }
        public long? ProductId { get; set; }

        public string? ImageName { get; set; }
    }

    public enum AddProductImageGalleryResult{
        Success,
        Error,
        NotFound
    }
}
