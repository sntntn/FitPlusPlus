namespace ManagerService.Common.Entities;

public class Finance
{
    public string Id { get; set; }
    public DateTime Date { get; set; }
    public string Description { get; set; }
    public int Amount { get; set; }
    public FinanceType Type { get; set; }
    
    /*Payment da se prosiri za opominjanje klijenata koji nisu platili*/
}