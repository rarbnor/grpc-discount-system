namespace DiscountStorage.Models;

public class DiscountCode
{
    public int Id { get; set; }
    public string Code { get; set; } = null!;
    public bool Used { get; set; } = false;
}
