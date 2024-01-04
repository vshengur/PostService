using MapsterMapper;
using Microsoft.AspNetCore.Mvc;

using PostService.API.Dto;
using PostService.API.Services;
using PostService.Domain.Models;

namespace PostService.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PostController(IMapper mapper, IPostService postServices) : ControllerBase
{
    private readonly IMapper _mapper = mapper;
    private readonly IPostService _postServices = postServices;

    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var posts = await _postServices.GetAllAsync();

        if (posts != null)
        {
            var mapRes = _mapper.Map<IList<ResponsePostDto>>(posts);
            return Ok(mapRes);
        }

        return NoContent();
    }

    [HttpGet("details")]
    public async Task<IActionResult> GetById(string id)
    {
        var post = await _postServices.GetPostByIdAsync(id);

        if (post is not null)
        {
            var mapRes = _mapper.Map<ResponsePostDto>(post);
            return Ok(mapRes);
        }

        return NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreatePostDto createPostDto)
    {
        var mapInput = _mapper.Map<Post>(createPostDto);
        var res = await _postServices.AddPostAsync(mapInput);

        if (res is not null)
        {
            var mapRes = _mapper.Map<ResponsePostDto>(res);
            return CreatedAtAction(nameof(GetById), new { id = mapRes.Id }, mapRes);
        }

        return BadRequest();
    }

    [HttpPut]
    public async Task<IActionResult> Put([FromBody] UpdatePostDto updatePostDto)
    {
        var mapInput = _mapper.Map<Post>(updatePostDto);
        var res = await _postServices.EditPostAsync(mapInput);

        if (res is not null)
        {
            var mapRes = _mapper.Map<ResponsePostDto>(res);
            return Ok(mapRes);
        }

        return BadRequest();
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(string id)
    {
        var res = await _postServices.DeletePostAsync(id);
        if (res is true)
        {
            return Ok($"post with id {id} has been deleted successfully");
        }

        return BadRequest("There is an error.");
    }
}