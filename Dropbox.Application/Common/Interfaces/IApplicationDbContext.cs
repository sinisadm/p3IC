using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using Dropbox.Domain.Entities;

namespace Dropbox.Application.Common.Interfaces
{
    public interface IApplicationDbContext : IDisposable
    {

        DbSet<User> Users { get; set; }
        DbSet<Device> Devices { get; set; }
        DbSet<Item> Items { get; set; }
        DbSet<UserDevice> UsersDevices { get; set; }



        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
