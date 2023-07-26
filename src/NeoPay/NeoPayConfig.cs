namespace NeoPay;

public sealed class NeoPayConfig
{
    private const int MinKeyLen = 32;
    
    public int ProjectId { get; }
    public string ProjectKey { get; }
    public string WidgetUrl { get; }

    public NeoPayConfig(int projectId, string projectKey, string widgetUrl)
    {
        ProjectId = projectId;
        ProjectKey = projectKey;
        WidgetUrl = widgetUrl;

        if (ProjectKey.Length < MinKeyLen)
        {
            ProjectKey = ProjectKey.PadRight(MinKeyLen, '\0');
        }
    }
}