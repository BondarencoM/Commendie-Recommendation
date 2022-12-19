using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecommendationService.Services.Interfaces;

public interface IRepository<TViewModel, TInputModel, TUpdateModel>
{
    public Task<List<TViewModel>> All();

    public Task<TViewModel> Find(long id);

    public Task Update(long id, TUpdateModel persona);

    public Task<TViewModel> Add(TInputModel persona);
}
