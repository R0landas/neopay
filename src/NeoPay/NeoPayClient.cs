using NeoPay.Features.PaymentInitiationService;

namespace NeoPay;

public sealed class NeoPayClient
{
    private readonly NeoPayConfig config;

    public NeoPayClient(NeoPayConfig config)
    {
        this.config = config;
    }

    public PispPayload StartNewRequest(string transactionId, decimal amount, string currency, string paymentPurpose)
    {
        return new PispPayload(config, transactionId, amount, currency, paymentPurpose);
    }

    public string GetPaymentUrl(PispPayload payload)
    {
        return $"{config.WidgetUrl}?{payload}";
    }
}