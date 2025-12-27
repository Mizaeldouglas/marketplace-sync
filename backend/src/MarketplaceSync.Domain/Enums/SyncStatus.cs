using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketplaceSync.Domain.Enums
{
    public enum SyncStatus
    {
        Pendente = 1,
        Sincronizando = 2,
        Sucesso = 3,
        Erro = 4
    }
}
