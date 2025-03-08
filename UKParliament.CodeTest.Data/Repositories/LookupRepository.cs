using UKParliament.CodeTest.Data.Models;
using UKParliament.CodeTest.Data.Repositories.Interfaces;
using UKParliament.CodeTest.Data.ViewModels;

namespace UKParliament.CodeTest.Data.Repositories;

public abstract class LookupRepository<T>(PersonManagerContext db)
    : BaseRepository<T>(db),
        ILookupRepository<T>
    where T : BaseEntity, ILookupItem { }
