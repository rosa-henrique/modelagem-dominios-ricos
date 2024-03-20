using NerdStore.Core.DomainObjects;

namespace NerdStore.Catalogo.Domain.Events;

public class ProdutoAbaixoEstoqueEvent(Guid aggregateId, int quantidadeRestante) : DomainEvent(aggregateId)
{
    public int QuantidadeRestante { get; private set; } = quantidadeRestante;
}
