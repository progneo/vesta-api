﻿namespace vesta_api.Database.Models.View.Requests;

public class NoteViewModel
{
    public string Text { get; set; } = null!;
    
    public int ClientId { get; set; }
}