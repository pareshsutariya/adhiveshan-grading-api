namespace AdhiveshanGrading.Models;

public class AuthRequestModel
{
    public string UserName { get; set; }
    public string Password { get; set; }
}

public class AuthResponseModel
{
    public string UserName { get; set; }
    public string AccessToken { get; set; }
    public int ExpiresInSeconds { get; set; }
    public int ExpiresInMinutes { get; set; }
    public DateTime ExpiresAtUtc { get; set; }
    public DateTime ExpiresAtEst { get; set; }

    public UserModel User { get; set; }
}