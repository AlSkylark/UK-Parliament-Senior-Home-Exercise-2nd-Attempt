using UKParliament.CodeTest.Data.Models;
using UKParliament.CodeTest.Data.ViewModels;

namespace UKParliament.CodeTest.Services.Mappers.Interfaces
{
    public interface IBasePersonMapper<Tview, Tbase>
        where Tview : EmployeeViewModel, new()
        where Tbase : Employee, new()
    {
        Tview Map(Tbase person);
        ShortManagerViewModel MapManager(Employee manager);
        Tbase MapForSave(EmployeeViewModel vm, Tbase existing);
    }
}
