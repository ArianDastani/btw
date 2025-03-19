namespace SharghPc.DataLayer.DTOs.Contact
{
    public class LoadContactDto
    {
        public long? Id { get; set; }

        public string? Title { get; set; }

        public string? Value { get; set; }
        public string? Link { get; set; }

        public long? ContactTypeId { get; set; }

    }
}
