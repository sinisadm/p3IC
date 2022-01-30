using AutoMapper;
using System;
using System.Collections.Generic;
using Dropbox.Application.Common.Mappings;
using Dropbox.Domain.Entities;

namespace Dropbox.Application.UsersDevices.Queries
{
    public class UserDeviceDto : IMapFrom<UserDevice>
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public uint QuotaLimit { get; set; }
        public int QuotaUsed { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UserDevice, UserDeviceDto>();
                //.ForMember(d => d.Name, o => o.MapFrom(s => s.Label))
                //.ForMember(d => d.AssignTo, o => o.MapFrom(s => s.Value))
                //.ForMember(d => d.Description, o => o.MapFrom(s => s.Metadata))
                //.ForMember(d => d.ParentTopic, o => o.MapFrom(s => s.ParentCatalogItemId));
        }
    }
}
