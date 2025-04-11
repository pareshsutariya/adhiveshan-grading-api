using System;
using AutoMapper;

namespace AdhiveshanGrading.Settings;

public static class AutoMapperExtensions
{
    public static T Map<T>(this Object source, IMapper mapper) where T : class
    {
        return mapper.Map<T>(source);
    }
}
