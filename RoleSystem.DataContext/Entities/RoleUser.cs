namespace RoleSystem.DataContext.Entities;

public class RoleUser
{
    public int Id { get; set; }
    public int RoleId { get; set; }
    public string UserId { get; set; }
    public DateTime Created { get; set; }
    public string CreatedUser { get; set; }
}
