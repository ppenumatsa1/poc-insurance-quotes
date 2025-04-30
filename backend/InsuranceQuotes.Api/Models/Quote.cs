namespace InsuranceQuotes.Api.Models;

public class Quote
{
    public int Id { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string VehicleMake { get; set; } = string.Empty;
    public string VehicleModel { get; set; } = string.Empty;
    public int VehicleYear { get; set; }
    public string CoverageType { get; set; } = string.Empty;
    public decimal MonthlyPremium { get; set; }
    public decimal Deductible { get; set; }
    public DateTime CreatedAt { get; set; }
    public string PolicyTerm { get; set; } = string.Empty;
    public CoverageLimits CoverageLimits { get; set; } = new();
}

public class CoverageLimits
{
    public string BodilyInjury { get; set; } = string.Empty;
    public string PropertyDamage { get; set; } = string.Empty;
}