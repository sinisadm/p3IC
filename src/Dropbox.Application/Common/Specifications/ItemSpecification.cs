using LinqKit;
using System;
using System.Linq.Expressions;
using Dropbox.Application.Common.Interfaces;
using Dropbox.Domain.Entities;

namespace Dropbox.Application.Common.Specifications
{
    public class ItemSpecification : ISpecification<Item>
    {
        public ItemSpecification(
            Guid userDeviceId,
            Guid? parentItemId,
            string name)
        {
            Name = name?.Trim().ToLower();
            UserDeviceId = userDeviceId;
            ParentItemId = parentItemId;
        }

        public Guid UserDeviceId { get; set; }
        public Guid? ParentItemId { get; set; }
        public string Name { get; set; }

        public Expression<Func<Item, bool>> Predicate
        {
            get
            {
                Expression<Func<Item, bool>> predicate = t => true;
 
                predicate = predicate.And(t => t.UserDeviceId == UserDeviceId);
              

                if (ParentItemId.HasValue)
                {
                    predicate = predicate.And(x => x.ParentItemId == ParentItemId.Value);
                }
                 
                if (!string.IsNullOrEmpty(Name))
                {
                    predicate = predicate.And(t => t.ItemName.ToLower().StartsWith(Name));
                }

                return predicate.Expand();
            }
        }
    }
}
