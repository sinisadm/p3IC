using AutoMapper;
using System;
using System.Collections.Generic;
using Dropbox.Application.Common.Mappings;
using Dropbox.Domain.Entities;

namespace Dropbox.Application.Items.Queries
{
    public class ItemDto : IMapFrom<Item>
    {
        public Guid Id { get; set; }
        public Guid? ParentItemId { get; set; }
        public Guid UserDeviceId { get; set; }
        public bool IsFolder { get; set; }
        public string InFolder { get; set; }
        public string OnDevice { get; set; }
        public string ItemName { get; set; }
        //public int Version { get; set; }
        //public string Path { get; set; }
        //public uint Size { get; set; }
        public List<ItemDto> Members { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Item, ItemDto>()
                .ForMember(d => d.InFolder, o => o.MapFrom(s => s.ParentItem.ItemName))
                .ForMember(d => d.OnDevice, o => o.MapFrom(s => s.UserDevice.Device.Name));
        }
    }
}
