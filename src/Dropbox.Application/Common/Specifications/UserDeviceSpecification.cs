using LinqKit;
using System;
using System.Linq.Expressions;
using Dropbox.Application.Common.Interfaces;
using Dropbox.Domain.Entities;

namespace Dropbox.Application.Common.Specifications
{
    public class UserDeviceSpecification : ISpecification<UserDevice>
    {
        public UserDeviceSpecification(
            Guid? userId,
            Guid? deviceId)
        {
            UserId = userId;
            DeviceId = deviceId;
        }

        public Guid? UserId { get; set; }
        public Guid? DeviceId { get; set; }

        public Expression<Func<UserDevice, bool>> Predicate
        {
            get
            {
                Expression<Func<UserDevice, bool>> predicate = t => true;
                if (UserId.HasValue)
                {
                    predicate = predicate.And(t => t.UserId == UserId.Value);
                }

                if (DeviceId.HasValue)
                {
                    predicate = predicate.And(t => t.DeviceId == DeviceId);
                }

                return predicate.Expand();
            }
        }
    }
}
