using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFinance.Domain.Entities
{
    // abstract: Não posso dar "new BaseEntity()", ela só serve de pai para outras.
    public abstract class BaseEntity
    {
        public Guid Id { get; private set; }
        public DateTime DataCriacao { get; private set; }

        protected BaseEntity()
        {
            Id = Guid.NewGuid();
            DataCriacao = DateTime.UtcNow; // Sempre use UTC no banco para não dar pau de fuso horário
        }
    }
}
