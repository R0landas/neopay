using System.Text;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace NeoPay.Features.PaymentInitiationService;

public class PispPayload
{
    private readonly string projectKey;
    private readonly Dictionary<string, object> claims = new();

    public PispPayload(NeoPayConfig config, string transactionId, decimal amount, string currency, string paymentPurpose)
    {
        projectKey = config.ProjectKey;
        
        claims.Add("projectId", config.ProjectId);
        claims.Add("transactionId", transactionId);
        claims.Add("amount", amount);
        claims.Add("currency", currency);
        claims.Add("paymentPurpose", paymentPurpose);
    }

    public PispPayload AddClientRedirectUrl(string redirectUrl)
    {
        claims.Add("clientRedirectUrl", redirectUrl);
        return this;
    }

    public PispPayload AddUserIdentity(string userIdentity)
    {
        claims.Add("userIdentity", userIdentity);
        return this;
    }

    public PispPayload WithEndToEndId(string endToEndId)
    {
        claims.Add("endToEndId", endToEndId);
        return this;
    }

    public override string ToString()
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(projectKey));
        var tokenHandler = new JsonWebTokenHandler();
        var descriptor = new SecurityTokenDescriptor()
        {
            Claims = claims,
            SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256),
        };

        return tokenHandler.CreateToken(descriptor);
    }
}