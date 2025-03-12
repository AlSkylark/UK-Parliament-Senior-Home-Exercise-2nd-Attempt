using UKParliament.CodeTest.Data.ViewModels;

namespace UKParliament.CodeTest.Services.Services.Interfaces
{
    public interface IManagerService
    {
        IEnumerable<ShortManagerViewModel> GetAll();
        Task<ManagerViewModel?> Get(int id);
        Task<ManagerViewModel?> Update(ManagerViewModel model);
    }
}
