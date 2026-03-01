using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFinance.Domain.Interfaces
{
    public interface IEntityComDono
    {
        string UserId { get; }
        void AssociarUsuario(string userId);
    }
}
