using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using BLL.Models;
using DAL.Entities;
using Gamestore.Models;
using Gamestore.Models.RequestViewModels;

namespace Gamestore.Mappings;

[ExcludeFromCodeCoverage]
public class AutoMapper : Profile
{
    public AutoMapper()
    {
        // Game <-> GameDTO
        CreateMap<Game, GameDto>()
            .ForMember(dest => dest.GenresDto, opt => opt.MapFrom(src => src.Genres))
            .ForMember(dest => dest.PlatformsDto, opt => opt.MapFrom(src => src.Platforms));

        CreateMap<GameDto, Game>()
            .ForMember(dest => dest.Genres, opt => opt.MapFrom(src => src.GenresDto))
            .ForMember(dest => dest.Platforms, opt => opt.MapFrom(src => src.PlatformsDto));

        // GameDTO <-> GameViewModel
        CreateMap<GameDto, GameViewModel>()
            .ReverseMap();

        // AddUpdateGameViewModel <-> GameDTO
        CreateMap<AddUpdateGameViewModel, GameDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Game.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Game.Name))
            .ForMember(dest => dest.Key, opt => opt.MapFrom(src => src.Game.Key))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Game.Description))
            .ForMember(dest => dest.GenresDto, opt => opt.MapFrom(src => src.GenreIds.Select(id => new GenreDto { Id = id })))
            .ForMember(dest => dest.PlatformsDto, opt => opt.MapFrom(src => src.PlatformIds.Select(id => new PlatformDto { Id = id })));

        CreateMap<GameDto, AddUpdateGameViewModel>()
            .ForMember(dest => dest.Game, opt => opt.MapFrom(src => new GameViewModel
            {
                Id = src.Id,
                Name = src.Name,
                Key = src.Key,
                Description = src.Description,
            }))
            .ForMember(dest => dest.GenreIds, opt => opt.MapFrom(src => src.GenresDto.Select(g => g.Id).ToList()))
            .ForMember(dest => dest.PlatformIds, opt => opt.MapFrom(src => src.PlatformsDto.Select(p => p.Id).ToList()));

        // Genre <-> GenreDTO
        CreateMap<Genre, GenreDto>()
            .ForMember(dest => dest.ChildGenresDto, opt => opt.MapFrom(src => src.ChildGenres))
            .ReverseMap();

        // GenreDTO <-> GenreViewModel
        CreateMap<GenreDto, GenreViewModel>()
            .ReverseMap();

        // AddUpdateGenreViewModel <-> GenreDTO
        CreateMap<AddUpdateGenreViewModel, GenreDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Genre.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Genre.Name))
            .ForMember(dest => dest.ParentGenreId, opt => opt.MapFrom(src => src.Genre.ParentGenreId))
            .ReverseMap();

        // Platform <-> PlatformDTO
        CreateMap<Platform, PlatformDto>()
            .ReverseMap();

        // PlatformDTO <-> PlatformViewModel
        CreateMap<PlatformDto, PlatformViewModel>()
            .ReverseMap();

        // AddUpdatePlatformViewModel <-> PlatformDTO
        CreateMap<AddUpdatePlatformViewModel, PlatformDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Platform.Id))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Platform.Type))
            .ReverseMap();
    }
}
