using AutoMapper;
using GhostyNetwork.Core.Application.Interfaces.Repositories;
using GhostyNetwork.Core.Application.Interfaces.Services;

namespace GhostyNetwork.Core.Application.Service
{
    public class GenericService<SaveViewModel, ViewModel, Model> : IGenericService<SaveViewModel, ViewModel, Model>
        where SaveViewModel : class
        where ViewModel : class
        where Model : class
    {
        private readonly IGenericRepository<Model> _repository;
        private readonly IMapper _mapper;

        public GenericService(IGenericRepository<Model> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<SaveViewModel> Add(SaveViewModel vm)
        {
            var entity = _mapper.Map<Model>(vm);
            var result = await _repository.AddAsync(entity);
            return _mapper.Map<SaveViewModel>(result);
        }

        public async Task Delete(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity != null)
            {
                await _repository.DeleteAsync(entity);
            }
        }

        public async Task<List<ViewModel>> GetAllViewModel()
        {
            var entities = await _repository.GetAllAsync();
            return _mapper.Map<List<ViewModel>>(entities);
        }

        public async Task<SaveViewModel> GetByIdSaveViewModel(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return _mapper.Map<SaveViewModel>(entity);
        }

        public async Task Update(SaveViewModel vm, int id)
        {
            var entity = _mapper.Map<Model>(vm);
            await _repository.UpdateAsync(entity, id);
        }
    }
}