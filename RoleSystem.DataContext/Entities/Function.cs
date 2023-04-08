namespace RoleSystem.DataContext.Entities;

public class Function
{
    public int Id { get; set; }
    public int? ParentFunctionId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime Created { get; set; }
    public string CreatedUser { get; set; }
    public DateTime LastEdit { get; set; }
    public string LastEditUser { get; set; }
}
