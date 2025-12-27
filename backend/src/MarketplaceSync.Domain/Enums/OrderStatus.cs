using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketplaceSync.Domain.Enums
{
    public enum OrderStatus
    {
        Pendente = 1,
        Processado = 2,
        Enviado = 3,
        Entregue = 4,
        Cancelado = 5
    }
}
