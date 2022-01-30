using System;
using System.Collections.Generic;

namespace Dropbox.Domain.Entities
{
    public partial class Item
    {
        public Item()
        {
            Members = new HashSet<Item>();
        }
        public Guid Id { get; set; }
        public Guid UserDeviceId { get; set; }
        public Guid? ParentItemId { get; set; }
        public Guid? RootFolderId { get; set; }
        public bool IsFolder { get; set; }
        public string ItemName { get; set; }
        //public int Version { get; set; }
        //public string ItemExtension { get; set; }
        //public string ItemPath { get; set; }
        //public int ItemSize { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }

        public bool IsDeleted { get; set; } = false;
        public DateTime DeletedAt { get; set; }
        public virtual Item ParentItem { get; set; }
        public virtual UserDevice UserDevice { get; set; }
        public virtual UserDevice RootFolder { get; set; }
        public ICollection<Item> Members { get; set; }
    }
}
