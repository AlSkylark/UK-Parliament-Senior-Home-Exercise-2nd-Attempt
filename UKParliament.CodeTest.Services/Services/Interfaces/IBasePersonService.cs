using UKParliament.CodeTest.Data.HATEOAS.Interfaces;
using UKParliament.CodeTest.Data.Requests;

namespace UKParliament.CodeTest.Services.Services.Interfaces;

public interface IBasePersonService<TViewModel>
{
    Task<TViewModel?> Create(TViewModel model);
    Task<TViewModel?> View(int id);
    Task<IResourceCollection<TViewModel>> Search(SearchRequest? request);
    Task<TViewModel?> Update(TViewModel model);
    Task Delete(int id);
}
