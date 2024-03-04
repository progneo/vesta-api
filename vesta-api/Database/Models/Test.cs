﻿namespace vesta_api.Database.Models;

public partial class Test
{
    public int Id { get; set; }

    public DateTime TestingDate { get; set; }

    public int ClientId { get; set; }

    public virtual Client Client { get; set; } = null!;
}