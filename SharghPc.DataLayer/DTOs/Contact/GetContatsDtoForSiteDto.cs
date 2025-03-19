namespace SharghPc.DataLayer.DTOs.Contact
{
    public class GetContatsDtoForSiteDto
    {
        public string? Title { get; set; }
        public string Value { get; set; } = null!;
        public long? ContactTypeId { get; set; }
        public string? Link { get; set; }

    }
}
