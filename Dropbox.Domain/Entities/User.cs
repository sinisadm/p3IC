using System;
using System.Collections.Generic;

namespace Dropbox.Domain.Entities
{
    public partial class User
    {
        public User()
        {
            Devices = new HashSet<UserDevice>();
        }
        public Guid Id { get; set; }
        public string Email { get; set; }
        public uint QuotaLimit { get; set; }
        public uint OuotaUsed { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DeletedAt { get; set; }
        public ICollection<UserDevice> Devices { get; set; }
    }
}
