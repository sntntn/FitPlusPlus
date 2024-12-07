namespace ManagerService.Common.Entities;

public class Trainer
{
    public string Id { get; set; }
    public string FullName { get; set; }
    public string ContactEmail { get; set; }
    public string ContactPhone { get; set; }
    public int Salary  { get; set; }
    public DateTime HiringDate { get; set; }
}