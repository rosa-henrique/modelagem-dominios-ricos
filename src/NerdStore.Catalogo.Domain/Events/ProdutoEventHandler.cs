using MediatR;

namespace NerdStore.Catalogo.Domain.Events;

public class ProdutoEventHandler(IProdutoRepository produtoRepository) : INotificationHandler<ProdutoAbaixoEstoqueEvent>
{
    private readonly IProdutoRepository _produtoRepository = produtoRepository;

    public async Task Handle(ProdutoAbaixoEstoqueEvent mensagem, CancellationToken cancellationToken)
    {
        var produto = await _produtoRepository.ObterPorId(mensagem.AggregateId);

        // Enviar um email para aquisicao de mais produtos.
    }
}
