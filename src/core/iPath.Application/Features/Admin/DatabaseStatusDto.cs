namespace iPath.Application.Features.Admin;

public class DatabaseStatusDto
{
    public string? LastMigration { get; set; } = null;

    public string? InitialAdminPassword { get; set; } = null;
}
