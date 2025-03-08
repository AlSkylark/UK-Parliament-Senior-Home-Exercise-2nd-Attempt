using UKParliament.CodeTest.Data.Models;
using UKParliament.CodeTest.Data.ViewModels;

namespace UKParliament.CodeTest.Data.Repositories.Interfaces;

public interface ILookupRepository<T> : IBaseRepository<T>
    where T : BaseEntity, ILookupItem { }
