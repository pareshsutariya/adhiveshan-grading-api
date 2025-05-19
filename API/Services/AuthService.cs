using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace AdhiveshanGrading.Services;

public interface IAuthService
{
    Task<AuthResponseModel> Authenticate(AuthRequestModel request);
}

public class AuthService : BaseService, IAuthService
{
    private readonly IMongoCollection<User> _UsersCollection;
    private readonly IConfiguration configuration;

    public AuthService(IAdvGradingSettings settings,
                        IMapper mapper,
                        IWebHostEnvironment hostingEnvironment,
                        IConfiguration configuration) : base(settings, mapper, hostingEnvironment)
    {
        _UsersCollection = Database.GetCollection<User>(settings.UsersCollectionName);
        this.configuration = configuration;
    }

    public async Task<AuthResponseModel> Authenticate(AuthRequestModel request)
    {
        var userModel = (await _UsersCollection.Find(item => item.BAPSId.ToLower() == request.UserName.ToLower() && item.Password == request.Password)
                                               .FirstOrDefaultAsync()
                        )?.Map<UserModel>(mapper);

        if (userModel == null)
            throw new ApplicationException($"User not found for the given credentials");

        if (userModel.Status != "Active")
            throw new ApplicationException($"User {userModel.FullName} is not Active");

        if (userModel != null && userModel.AssignedRoles.Any())
        {
            userModel.AssignedPermissions = RolePermissionsService.GetRolePermissions()
                            .Where(r => userModel.AssignedRoles.Contains(r.RoleName))
                            .SelectMany(c => c.Permissions).ToList();
        }

        var issuer = configuration.GetValue<string>("JwtConfig:issuer");
        var audience = configuration.GetValue<string>("JwtConfig:audiance");
        var key = configuration.GetValue<string>("JwtConfig:key");
        var tokenValidityMins = configuration.GetValue<int>("JwtConfig:tokenValidityMins");
        var tokenExpiryTimeStamps = DateTime.UtcNow.AddMinutes(tokenValidityMins);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(JwtRegisteredClaimNames.Name, request.UserName)
            }),

            Issuer = issuer,
            Audience = audience,
            Expires = tokenExpiryTimeStamps,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)), SecurityAlgorithms.HmacSha512Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.CreateToken(tokenDescriptor);
        var accessToken = tokenHandler.WriteToken(securityToken);

        var loginResponse = new AuthResponseModel
        {
            UserName = request.UserName,
            AccessToken = accessToken,
            ExpiresInMinutes = (int)tokenExpiryTimeStamps.Subtract(DateTime.UtcNow).TotalMinutes,
            ExpiresInSeconds = (int)tokenExpiryTimeStamps.Subtract(DateTime.UtcNow).TotalSeconds,
            ExpiresAtUtc = tokenExpiryTimeStamps,
            //ExpiresAtEst = ConvertToEasternTime(tokenExpiryTimeStamps),

            User = userModel!,
        };

        return loginResponse;
    }

    public DateTime ConvertToEasternTime(DateTime dateUtc)
    {
        return TimeZoneInfo.ConvertTimeFromUtc(dateUtc, TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"));
    }
}
