using AutoMapper;

namespace Dropbox.Application.Common.Mappings
{
    public interface IMapFrom
    {

    }

    public interface IMapFrom<T> : IMapFrom
    {
        void Mapping(Profile profile)
        { 
            profile.CreateMap(typeof(T), GetType()); 
        }
    }
}
