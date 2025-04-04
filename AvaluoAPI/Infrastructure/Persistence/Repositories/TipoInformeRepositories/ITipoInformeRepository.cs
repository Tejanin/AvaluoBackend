﻿using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.Repositories.Base;

namespace AvaluoAPI.Infrastructure.Persistence.Repositories.TipoInformeRepositories
{
    public interface ITipoInformeRepository:IRepository<TipoInforme>
    {
        Task<TipoInforme?> GetTipoInformeByDescripcionAsync(string descripcion);
    }
}
