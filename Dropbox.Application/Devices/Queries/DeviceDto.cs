using AutoMapper;
using System;
using System.Collections.Generic;
using Dropbox.Application.Common.Mappings;
using Dropbox.Domain.Entities;

namespace Dropbox.Application.Devices.Queries
{
    public class DeviceDto : IMapFrom<Device>
    {
        public Guid Id { get; set; }
        public string MacAddress { get; set; }
        public string Name { get; set; }

        public List<DeviceDto> Content { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Device, DeviceDto>();
                //.ForMember(d => d.Name, o => o.MapFrom(s => s.Label))
                //.ForMember(d => d.AssignTo, o => o.MapFrom(s => s.Value))
                //.ForMember(d => d.Description, o => o.MapFrom(s => s.Metadata))
                //.ForMember(d => d.ParentTopic, o => o.MapFrom(s => s.ParentCatalogItemId));
        }
    }
}
