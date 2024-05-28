using System.Security.Authentication;
using Infrastructure.Model;
using JWT;
using JWT.Algorithms;
using JWT.Builder;
using JWT.Serializers;
using Newtonsoft.Json;
using Serilog;

namespace API.Security;
public static class UnixEpoch
{
    public static long ToUnixEpochDate(DateTime date)
    {
        var unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        return (long)Math.Round((date.ToUniversalTime() - unixEpoch).TotalSeconds);
    }
}
public class TokenService
{
    public string IssueJwt(EndUser user)
    {
        try
        {
            // Define claims including the 'email' claim
            var claims = new Dictionary<string, object>
            {
                { "email", user.Email },
                // Add other claims as needed
            };

            // Set JWT token expiration time
            var issuedAt = DateTime.UtcNow;
            var expires = issuedAt.AddHours(1); // Example: Token expires in 1 hour

            // Convert DateTime values to Unix epoch time format
            var iat = UnixEpoch.ToUnixEpochDate(issuedAt);
            var exp = UnixEpoch.ToUnixEpochDate(expires);

            // Configure JWT token parameters
            var token = new JwtBuilder()
                .WithAlgorithm(new HMACSHA512Algorithm())
                .WithSecret(Environment.GetEnvironmentVariable(ENV_VAR_KEYS.JWT_KEY.ToString()))
                .AddClaim("iss", "your_issuer") // Example: Issuer claim
                .AddClaim("aud", "your_audience") // Example: Audience claim
                .AddClaim("iat", iat)
                .AddClaim("exp", exp)
                .AddClaims(claims) // Add custom claims including 'email'
                .Encode();

            return token;
        }
        catch (Exception e)
        {
            Log.Error(e, "IssueJWT");
            throw new InvalidOperationException("User authentication succeeded, but could not create token");
        }
    }

    public Dictionary<string, string> ValidateJwtAndReturnClaims(string jwt)
    {
        try
        {
            IJsonSerializer serializer = new JsonNetSerializer();
            var provider = new UtcDateTimeProvider();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtValidator validator = new JwtValidator(serializer, provider);
            IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder, new HMACSHA512Algorithm());
            var json = decoder.Decode(jwt, Environment.GetEnvironmentVariable(ENV_VAR_KEYS.JWT_KEY.ToString()));
            return JsonConvert.DeserializeObject<Dictionary<string, string>>(json)!;
        }
        catch (Exception e)
        {
            Log.Error(e, "ValidateJwtAndReturnClaims");
            throw new AuthenticationException("Authentication failed.");
        }
    }
}