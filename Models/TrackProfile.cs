using AutoMapper;
namespace lab7.Models
{
    public class TrackProfile : Profile
    {
        public TrackProfile()
        {
            CreateMap<Track, TrackDTOModel>();
            CreateMap<TrackDTOModel, Track>();
        }
    }
}
