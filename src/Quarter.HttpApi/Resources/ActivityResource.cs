using Quarter.Core.Models;

namespace Quarter.HttpApi.Resources;


// ReSharper disable InconsistentNaming

/// <summary>
/// Output resource for project aggregates
/// </summary>
/// <param name="id">The ID of the activity</param>
/// <param name="projectId">The ID of the parent project</param>
/// <param name="name">The name of the activity</param>
/// <param name="description">The activity description</param>
/// <param name="color">The activity color in CSS HEX</param>
/// <param name="isArchived">Whether or not the activity is archived</param>
/// <param name="created">Timestamp for when the activity was created (ISO-8601)</param>
/// <param name="updated">Timestamp for when the activity was last updated, or null if it has never been updated (ISO-8601)</param>
public record ActivityResourceOutput(string id, string projectId, string name, string description, string color, bool isArchived, string created, string? updated)
{
    public Uri Location()
        => new ($"/api/project/{projectId}/activity/{id}", UriKind.Relative);

    public static ActivityResourceOutput From(Activity activity)
    {
        return new ActivityResourceOutput(
            activity.Id.AsString(),
            activity.ProjectId.AsString(),
            activity.Name,
            activity.Description,
            activity.Color.ToHex(),
            activity.IsArchived,
            activity.Created.IsoString(),
            activity.Updated?.IsoString());
    }
}
