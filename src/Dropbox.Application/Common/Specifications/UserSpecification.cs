using LinqKit;
using System;
using System.Linq.Expressions;
using Dropbox.Application.Common.Interfaces;
using Dropbox.Domain.Entities;

namespace Dropbox.Application.Common.Specifications
{
    public class UserSpecification : ISpecification<User>
    {
        public UserSpecification(
            Guid? id,
            string email)
        {
            Id = id;
            Email = email;
        }

        public Guid? Id { get; set; }
        public string Email { get; set; }

        public Expression<Func<User, bool>> Predicate
        {
            get
            {
                Expression<Func<User, bool>> predicate = t => true;
                if (Id.HasValue)
                {
                    predicate = predicate.And(t => t.Id == Id.Value);
                }

                if (!string.IsNullOrWhiteSpace(Email))
                {
                    predicate = predicate.And(t => t.Email.ToLower() == Email.ToLower());
                }

                return predicate.Expand();
            }
        }
    }
}
