using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AdhiveshanGrading.Models;

public class UserModel : ModelBase
{
    public int UserId { get; set; }
    public string FullName { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Status { get; set; }

    public List<string> Permissions { get; set; } = new();
}

public class UserCreateModel
{
    public string FullName { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Status { get; set; }

    public List<string> Permissions { get; set; } = new();
}

public class UserUpdateModel : ModelBase
{
    public int UserId { get; set; }
    public string FullName { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Status { get; set; }

    public List<string> Permissions { get; set; } = new();
}