using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dropbox.Domain.Entities
{
    public partial class Device
    {
        public Device()
        {
            Users = new HashSet<UserDevice>();
        }

        public Guid Id { get; set; }
        public string MacAddress { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DeletedAt { get; set; }


        public ICollection<UserDevice> Users { get; set; }
    }
}
