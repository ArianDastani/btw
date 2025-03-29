using SharghPc.DataLayer.Entites.Account;
using SharghPc.DataLayer.Entites.Common;

namespace SharghPc.DataLayer.Entites.Roles
{
    public class Roles : BaseEntity
    {
        #region Properties
        public string RoleName { get; set; } = null!;

        #endregion

        #region Relations

        public ICollection<User> Users { get; set; }

        #endregion
    }
}
