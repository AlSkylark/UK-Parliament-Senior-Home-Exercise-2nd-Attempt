using FakeItEasy;
using UKParliament.CodeTest.Data.Models;
using UKParliament.CodeTest.Data.Repositories.Interfaces;
using UKParliament.CodeTest.Data.ViewModels;
using UKParliament.CodeTest.Services.HATEOAS.Interfaces;
using UKParliament.CodeTest.Services.Mappers.Interfaces;
using UKParliament.CodeTest.Services.Services;
using Xunit;

namespace UKParliament.CodeTest.Tests;

public class EmployeeServiceTests
{
    [Fact]
    public void Update_CallsTheCorrectServices_Successfully()
    {
        var fakeRepo = A.Fake<IEmployeeRepository>();
        var fakeMapper = A.Fake<IEmployeeMapper>();
        var fakePaginator = A.Fake<IPaginatorService>();

        var service = new EmployeeService(fakeRepo, fakeMapper, fakePaginator);
        var employee = new Employee
        {
            FirstName = "Alex",
            LastName = "Castro",
            EmployeeType = EmployeeTypeEnum.Employee,
        };

        A.CallTo(() => fakeRepo.GetById(A<int>.Ignored)).ReturnsLazily(() => employee);
        A.CallTo(() => fakeMapper.MapForSave(A<EmployeeViewModel>.Ignored, employee))
            .ReturnsLazily(() => employee);
        A.CallTo(() => fakeRepo.Update(employee)).ReturnsLazily(() => employee);
        A.CallTo(() => fakeMapper.Map(employee)).ReturnsLazily(() => new EmployeeViewModel());

        var result = service.Update(new EmployeeViewModel());

        A.CallTo(() => fakeRepo.GetById(A<int>.Ignored)).MustHaveHappenedOnceExactly();
        A.CallTo(() => fakeMapper.MapForSave(A<EmployeeViewModel>.Ignored, employee))
            .MustHaveHappenedOnceExactly();
        A.CallTo(() => fakeRepo.Update(employee)).MustHaveHappenedOnceExactly();
        A.CallTo(() => fakeMapper.Map(employee)).MustHaveHappenedOnceExactly();
    }
}
