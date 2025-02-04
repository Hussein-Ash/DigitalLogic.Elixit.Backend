using System;
using AutoMapper;
using Elixir.DATA;
using Elixir.Entities;
using Elixir.Interface;
using Elixir.Repository;

namespace Elixir.Respository;

public class CategoryRepository : GenericRepository<Category, Guid>, ICategoryRepository
{
    public CategoryRepository(DataContext context, IMapper mapper) : base(context, mapper)
    {
    }
}
