using NerdStore.Core.DomainObjects;

namespace NerdStore.Pagamentos.Business;

public class Pagamento : Entity, IAggregateRoot
{
    public Guid PedidoId { get; set; }
    public string Status { get; set; } = string.Empty; 
    public decimal Valor { get; set; }

    public string NomeCartao { get; set; } = string.Empty;
    public string NumeroCartao { get; set; } = string.Empty;
    public string ExpiracaoCartao { get; set; } = string.Empty;
    public string CvvCartao { get; set; } = string.Empty;

    public Transacao Transacao { get; set; } = null!;
}
