namespace Application.DTO.AppVersion
{
    public record GetLatestVersionRequest(string Branch = "stable");
}