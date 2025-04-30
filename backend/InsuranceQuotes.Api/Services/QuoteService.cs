using InsuranceQuotes.Api.Models;

namespace InsuranceQuotes.Api.Services;

public class QuoteService
{
    private readonly List<Quote> _quotes;

    public QuoteService()
    {
        _quotes = new List<Quote>
        {
            new Quote
            {
                Id = 100,
                CustomerName = "John Smith",
                VehicleMake = "Toyota",
                VehicleModel = "Camry",
                VehicleYear = 2023,
                CoverageType = "Full Coverage",
                MonthlyPremium = 125.50M,
                Deductible = 500.00M,
                CreatedAt = DateTime.Now,
                PolicyTerm = "12 months",
                CoverageLimits = new CoverageLimits
                {
                    BodilyInjury = "100000/300000",
                    PropertyDamage = "50000"
                }
            },
            new Quote
            {
                Id = 101,
                CustomerName = "Emily Johnson",
                VehicleMake = "Honda",
                VehicleModel = "CR-V",
                VehicleYear = 2024,
                CoverageType = "Full Coverage",
                MonthlyPremium = 145.75M,
                Deductible = 1000.00M,
                CreatedAt = DateTime.Now,
                PolicyTerm = "6 months",
                CoverageLimits = new CoverageLimits
                {
                    BodilyInjury = "250000/500000",
                    PropertyDamage = "100000"
                }
            },
            new Quote
            {
                Id = 102,
                CustomerName = "Michael Brown",
                VehicleMake = "Ford",
                VehicleModel = "F-150",
                VehicleYear = 2022,
                CoverageType = "Liability Only",
                MonthlyPremium = 95.25M,
                Deductible = 1500.00M,
                CreatedAt = DateTime.Now,
                PolicyTerm = "12 months",
                CoverageLimits = new CoverageLimits
                {
                    BodilyInjury = "50000/100000",
                    PropertyDamage = "25000"
                }
            },
            new Quote
            {
                Id = 103,
                CustomerName = "Sarah Wilson",
                VehicleMake = "Tesla",
                VehicleModel = "Model 3",
                VehicleYear = 2024,
                CoverageType = "Full Coverage",
                MonthlyPremium = 185.00M,
                Deductible = 1000.00M,
                CreatedAt = DateTime.Now,
                PolicyTerm = "12 months",
                CoverageLimits = new CoverageLimits
                {
                    BodilyInjury = "300000/500000",
                    PropertyDamage = "100000"
                }
            },
            new Quote
            {
                Id = 104,
                CustomerName = "David Martinez",
                VehicleMake = "Chevrolet",
                VehicleModel = "Silverado",
                VehicleYear = 2023,
                CoverageType = "Full Coverage",
                MonthlyPremium = 155.50M,
                Deductible = 500.00M,
                CreatedAt = DateTime.Now,
                PolicyTerm = "6 months",
                CoverageLimits = new CoverageLimits
                {
                    BodilyInjury = "100000/300000",
                    PropertyDamage = "50000"
                }
            },
            new Quote
            {
                Id = 105,
                CustomerName = "Lisa Anderson",
                VehicleMake = "Subaru",
                VehicleModel = "Outback",
                VehicleYear = 2024,
                CoverageType = "Full Coverage",
                MonthlyPremium = 135.25M,
                Deductible = 1000.00M,
                CreatedAt = DateTime.Now,
                PolicyTerm = "12 months",
                CoverageLimits = new CoverageLimits
                {
                    BodilyInjury = "250000/500000",
                    PropertyDamage = "100000"
                }
            },
            new Quote
            {
                Id = 106,
                CustomerName = "Robert Taylor",
                VehicleMake = "BMW",
                VehicleModel = "330i",
                VehicleYear = 2023,
                CoverageType = "Full Coverage",
                MonthlyPremium = 215.75M,
                Deductible = 1000.00M,
                CreatedAt = DateTime.Now,
                PolicyTerm = "12 months",
                CoverageLimits = new CoverageLimits
                {
                    BodilyInjury = "300000/500000",
                    PropertyDamage = "100000"
                }
            },
            new Quote
            {
                Id = 107,
                CustomerName = "Jennifer Lee",
                VehicleMake = "Hyundai",
                VehicleModel = "Tucson",
                VehicleYear = 2024,
                CoverageType = "Full Coverage",
                MonthlyPremium = 128.50M,
                Deductible = 500.00M,
                CreatedAt = DateTime.Now,
                PolicyTerm = "6 months",
                CoverageLimits = new CoverageLimits
                {
                    BodilyInjury = "100000/300000",
                    PropertyDamage = "50000"
                }
            },
            new Quote
            {
                Id = 108,
                CustomerName = "William Garcia",
                VehicleMake = "Nissan",
                VehicleModel = "Altima",
                VehicleYear = 2023,
                CoverageType = "Liability Only",
                MonthlyPremium = 85.25M,
                Deductible = 1500.00M,
                CreatedAt = DateTime.Now,
                PolicyTerm = "12 months",
                CoverageLimits = new CoverageLimits
                {
                    BodilyInjury = "50000/100000",
                    PropertyDamage = "25000"
                }
            },
            new Quote
            {
                Id = 109,
                CustomerName = "Rachel Thompson",
                VehicleMake = "Mazda",
                VehicleModel = "CX-5",
                VehicleYear = 2024,
                CoverageType = "Full Coverage",
                MonthlyPremium = 142.00M,
                Deductible = 1000.00M,
                CreatedAt = DateTime.Now,
                PolicyTerm = "12 months",
                CoverageLimits = new CoverageLimits
                {
                    BodilyInjury = "250000/500000",
                    PropertyDamage = "100000"
                }
            }
        };
    }

    public IEnumerable<Quote> GetAllQuotes() => _quotes;

    public Quote? GetQuoteById(int id) => _quotes.FirstOrDefault(q => q.Id == id);
}