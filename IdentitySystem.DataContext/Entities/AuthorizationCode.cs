namespace IdentitySystem.DataContext.Entities;

public class AuthorizationCode
{
    public int Id { get; set; }
    public Guid AppId { get; set; }
    public string Code { get; set; }
    public string UserId { get; set; }
    public DateTime Expire { get; set; }
}
