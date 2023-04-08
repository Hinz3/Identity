namespace IdentitySystem.DataContext.Entities;

public class ScopeAudience
{
    public int Id { get; set; }
    public int ScopeId { get; set; }
    public int AudienceId { get; set; }
    public DateTime Created { get; set; }
    public string CreatedUser { get; set; }
}
