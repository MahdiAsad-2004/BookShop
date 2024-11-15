﻿using BookShop.Domain.Common.Repository;
using BookShop.Domain.Entities;

namespace BookShop.Domain.IRepositories
{
    public interface ITranslatorRepository :
        IRepository,
        IReadRepository<Translator, Guid>,
        IWriteRepository<Translator, Guid>,
        IDeleteRepository<Translator, Guid>
    {
    }
}
