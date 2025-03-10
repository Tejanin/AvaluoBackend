﻿using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.Repositories.Base;
using NuGet.Protocol.Core.Types;

namespace AvaluoAPI.Infrastructure.Persistence.Repositories.IDesempeñoRepositories
{
    public interface IDesempeñoRepository : IRepository<Desempeno>
    {
        Task InsertDesempeños(List<int> asignaturas, int año, string periodo, int estado);
    }
}
