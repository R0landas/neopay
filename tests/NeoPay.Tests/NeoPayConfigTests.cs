using FluentAssertions;

namespace NeoPay.Tests;

public class NeoPayConfigTests
{
    [Fact]
    public void NeoPayConfig_WhenKeyIsShorterThanMin_AddsRightPadding()
    {
        const int minKeyLen = 32;
        const string key = "averysecretkey";
        var expectedKey = key.PadRight(32, '\0');

        var config = new NeoPayConfig(0, key, "https://testwidget.local");

        config.ProjectKey.Should().Be(expectedKey);
        config.ProjectKey.Length.Should().Be(minKeyLen);
    }
    
    [Fact]
    public void NeoPayConfig_WhenKeyIsLonger_DoesNotAddRightPadding()
    {
        const string key = "averyverylongsecretaveryverylongsecret";

        var config = new NeoPayConfig(0, key, "https://testwidget.local");

        config.ProjectKey.Should().Be(key);
        config.ProjectKey.Length.Should().Be(key.Length);
    }
}