﻿namespace PostService.API.Dto;

public class ResponsePostDto
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Title { get; set; }
    public string UserName { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}