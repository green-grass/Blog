#if NET451
using AutoMapper;

namespace PhantomNet.Mvc
{
    public interface IMapperContainer<out T>
    {
        IMapper Mapper { get; }
    }

    public class MapperContainer<T> : IMapperContainer<T>
    {
        public MapperContainer(IMapper mapper)
        {
            Mapper = mapper;
        }

        public IMapper Mapper { get; }
    }
}
#endif
