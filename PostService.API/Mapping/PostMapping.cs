using Mapster;
using PostService.API.Dto;
using PostService.Domain.Models;

namespace PostService.API.Mapping;

public class PostMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreatePostDto, Post>();
        config.NewConfig<UpdatePostDto, Post>();
        config.NewConfig<Post, ResponsePostDto>();
    }
}
