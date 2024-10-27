using System;
using AutoMapper;

namespace BackendBlocks.Core.Mappings;

public static class MapperDecorator
    {
        private static readonly MapperConfiguration MapperConfiguration;

        static MapperDecorator()
        {
            MapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<HashProfile>();
            });

            MapperConfiguration.AssertConfigurationIsValid();
        }

        public static IMapper CreateMapper()
        {
            return MapperConfiguration.CreateMapper();
        }
    }