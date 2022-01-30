using System;
using System.Collections.Generic;

namespace Dropbox.Domain.Entities
{
    public partial class UserDevice
    {
        public UserDevice()
        {
            Items = new HashSet<Item>();
        }
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid DeviceId { get; set; }
        public string SyncFolder { get; set; }
        public Guid? RootFolderId { get; set; }

        public virtual User User { get; set; }
        public virtual Device Device { get; set; }
        public virtual Item RootFolder { get; set; }
        public ICollection<Item> Items { get; set; }

    }
}
