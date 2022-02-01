using AutoMapper;
using System;
using System.Collections.Generic;
using Dropbox.Application.Common.Mappings;
using Dropbox.Domain.Entities;

namespace Dropbox.Application.Items.Queries
{
    public class FileDto : IMapFrom<Item>
    {
        public Guid Id { get; set; }
        public Guid? ParentItemId { get; set; }
        public Guid UserDeviceId { get; set; }
        public string FileName { get; set; }
        public string InFolder { get; set; }
        public string OnDevice { get; set; }


        public void Mapping(Profile profile)
        {
            profile.CreateMap<Item, FileDto>()
                .ForMember(d => d.InFolder, o => o.MapFrom(s => s.ParentItem.ItemName))
                .ForMember(d => d.OnDevice, o => o.MapFrom(s => s.UserDevice.Device.Name))
                .ForMember(d => d.FileName, o => o.MapFrom(s => s.ItemName));
        }
    }
}
