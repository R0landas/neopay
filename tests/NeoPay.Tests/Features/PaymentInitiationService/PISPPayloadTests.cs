using System.Globalization;
using System.Text;
using FluentAssertions;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace NeoPay.Tests.Features.PaymentInitiationService;

public class PispPayloadTests
{
    private const int ProjectId = 0;
    private const string ProjectKey = "thisisaveryverylongsecretprojectkey";
    
    private readonly NeoPayClient client = new(new NeoPayConfig(ProjectId, ProjectKey, "https://testwidget.local"));
    
    [Fact]
    public void ToString_ReturnsExpectedJWT()
    {
        const string transactionId = "0";
        const decimal amount = 0.1M;
        const string currency = "EUR";
        const string paymentPurpose = "test";

        var token = client
            .StartNewRequest("0", 0.1M, "EUR", "test")
            .ToString();

        var tokenHandler = new JsonWebTokenHandler();

        var validationParams = new TokenValidationParameters()
        {
            ValidateIssuerSigningKey = true,
            ValidateAudience = false,
            ValidateIssuer = false,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ProjectKey)),
        };
        
        var result = tokenHandler.ValidateToken(token, validationParams);
        result.IsValid.Should().BeTrue("JWT should be signed with the provided key");
        
        var parsedToken = tokenHandler.ReadJsonWebToken(token);
        parsedToken.GetClaim("projectId").Value.Should().Be(ProjectId.ToString());
        parsedToken.GetClaim("transactionId").Value.Should().Be(transactionId);
        parsedToken.GetClaim("amount").Value.Should().Be(amount.ToString(CultureInfo.InvariantCulture));
        parsedToken.GetClaim("currency").Value.Should().Be(currency);
        parsedToken.GetClaim("paymentPurpose").Value.Should().Be(paymentPurpose);
    }
}