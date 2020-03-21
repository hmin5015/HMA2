using AutoMapper;
using HMA2.Dtos;
using HMA2.Models;

namespace HMA2.Core
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<Version, VersionDto>();
            CreateMap<VersionDto, Version>();

            CreateMap<VersionType, VersionTypeDto>();
            CreateMap<VersionTypeDto, VersionType>();

            CreateMap<File, FileDto>();
            CreateMap<FileDto, File>();

            CreateMap<VersionFileMapping, VersionFileMappingDto>();
            CreateMap<VersionFileMappingDto, VersionFileMapping>();
        }
    }
}