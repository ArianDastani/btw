using SharghPc.DataLayer.Entites.Common;

namespace SharghPc.DataLayer.Entites.Site
{
    public class Contact : BaseEntity
    {
        public string? Title { get; set; }

        public string Value { get; set; } = null!;
        public string? Link { get; set; }

        public long? ContactTypeId { get; set; }

        public virtual ContactType? ContactType { get; set; }
    }
}
