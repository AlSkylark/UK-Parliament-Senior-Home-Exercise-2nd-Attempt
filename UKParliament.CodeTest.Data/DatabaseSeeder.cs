using Bogus;
using UKParliament.CodeTest.Data.Models;

namespace UKParliament.CodeTest.Data;

public static class DatabaseSeeder
{
    public static Department[] GetDepartments =>
        [
            new() { Name = "Sales" },
            new() { Name = "Marketing" },
            new() { Name = "Finance" },
            new() { Name = "HR" },
        ];

    public static PayBand[] GetPayBands =>
        [
            new()
            {
                Name = "C2",
                MinPay = 20_000,
                MaxPay = 29_000,
            },
            new()
            {
                Name = "C1",
                MinPay = 29_999,
                MaxPay = 39_000.90M,
            },
            new()
            {
                Name = "B2",
                MinPay = 39_999.99M,
                MaxPay = 49_000,
            },
            new()
            {
                Name = "B1",
                MinPay = 49_900,
                MaxPay = 59_000,
            },
            new()
            {
                Name = "A2",
                MinPay = 59_000,
                MaxPay = 69_000,
            },
            new()
            {
                Name = "A1",
                MinPay = 69_900,
                MaxPay = 200_000,
            },
        ];

    public static void SeedDatabase(this PersonManagerContext context)
    {
        context.Departments.AddRange(GetDepartments);
        context.PayBands.AddRange(GetPayBands);
        context.SaveChanges();

        context.Managers.AddRange(CreateFakeManager(5, context));

        context.SaveChanges();
    }

    public static IEnumerable<Manager> CreateFakeManager(
        int amount,
        PersonManagerContext context,
        int employees = 10
    )
    {
        return new Faker<Manager>()
            .ApplyEmployeeRules(context)
            .RuleFor(p => p.Employees, (f, r) => CreateFakeEmployee(employees, r, context))
            .Generate(amount);
    }

    public static IEnumerable<Employee> CreateFakeEmployee(
        int amount,
        Manager manager,
        PersonManagerContext context
    )
    {
        return new Faker<Employee>()
            .ApplyEmployeeRules(context)
            .RuleFor(p => p.Manager, manager)
            .Generate(amount);
    }

    public static Faker<T> ApplyEmployeeRules<T>(this Faker<T> faker, PersonManagerContext context)
        where T : Employee
    {
        return faker
            .RuleFor(p => p.PayBand, f => context.PayBands.ToList().ElementAt(f.Random.Number(5)))
            .RuleFor(
                p => p.Department,
                f => context.Departments.ToList().ElementAt(f.Random.Number(3))
            )
            .RuleFor(p => p.BankAccount, f => f.Finance.Iban())
            .RuleFor(p => p.DateJoined, f => f.Date.PastDateOnly(f.Random.Number(50)))
            .RuleFor(
                p => p.Salary,
                (f, r) => f.Finance.Amount(r.PayBand!.MinPay, r.PayBand!.MaxPay)
            )
            .RuleFor(p => p.Address, f => CreateFakeAddress())
            .RuleFor(p => p.DoB, f => f.Date.PastDateOnly(f.Random.Number(30, 70)))
            .RuleFor(p => p.FirstName, f => f.Name.FirstName())
            .RuleFor(p => p.LastName, f => f.Name.LastName());
    }

    public static Address CreateFakeAddress()
    {
        return new Faker<Address>(locale: "en_GB")
            .RuleFor(v => v.Address1, f => f.Address.StreetAddress())
            .RuleFor(v => v.Address2, f => f.Address.SecondaryAddress())
            .RuleFor(v => v.County, f => f.Address.County())
            .RuleFor(v => v.Postcode, f => f.Address.ZipCode());
    }
}
