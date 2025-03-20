
using SharghPc.DataLayer.Entites.Common;

namespace SharghPc.DataLayer.Entites.Site
{
    public class ContactType : BaseEntity
    {
        public string ContactTypeName { get; set; } = null!;

        public virtual ICollection<Contact> Contacts { get; set; } = new List<Contact>();
    }
}
