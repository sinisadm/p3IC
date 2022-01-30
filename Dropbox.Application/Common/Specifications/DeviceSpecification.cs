using LinqKit;
using System;
using System.Linq.Expressions;
using Dropbox.Application.Common.Interfaces;
using Dropbox.Domain.Entities;

namespace Dropbox.Application.Common.Specifications
{
    public class DeviceSpecification : ISpecification<Device>
    {
        public DeviceSpecification(
            Guid? id,
            string macAddress)
        {
            Id = id;
            MacAddress = macAddress;
        }

        public Guid? Id { get; set; }
        public string MacAddress { get; set; }

        public Expression<Func<Device, bool>> Predicate
        {
            get
            {
                Expression<Func<Device, bool>> predicate = t => true;
                if (Id.HasValue)
                {
                    predicate = predicate.And(t => t.Id == Id.Value);
                }

                if (!string.IsNullOrWhiteSpace(MacAddress))
                {
                    predicate = predicate.And(t => t.MacAddress == MacAddress);
                }

                return predicate.Expand();
            }
        }
    }
}
