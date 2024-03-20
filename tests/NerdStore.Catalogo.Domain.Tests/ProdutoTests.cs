﻿using NerdStore.Core.DomainObjects;

namespace NerdStore.Catalogo.Domain.Tests;

public class ProdutoTests
{
    [Test]
    public void Produto_Validar_ValidacoesDevemRetornarExceptions()
    {
        // Arrange & Act & Assert

        var ex = Assert.Throws<DomainException>(() =>
            new Produto(string.Empty, "Descricao", false, 100, Guid.NewGuid(), DateTime.Now, "Imagem", new Dimensoes(1, 1, 1))
        );

        Assert.That(ex.Message, Is.EqualTo("O campo Nome do produto não pode estar vazio"));

        ex = Assert.Throws<DomainException>(() =>
            new Produto("Nome", string.Empty, false, 100, Guid.NewGuid(), DateTime.Now, "Imagem", new Dimensoes(1, 1, 1))
        );

        Assert.That(ex.Message, Is.EqualTo("O campo Descricao do produto não pode estar vazio"));

        ex = Assert.Throws<DomainException>(() =>
            new Produto("Nome", "Descricao", false, 0, Guid.NewGuid(), DateTime.Now, "Imagem", new Dimensoes(1, 1, 1))
        );

        Assert.That(ex.Message, Is.EqualTo("O campo Valor do produto não pode se menor igual a 0"));

        ex = Assert.Throws<DomainException>(() =>
            new Produto("Nome", "Descricao", false, 100, Guid.Empty, DateTime.Now, "Imagem", new Dimensoes(1, 1, 1))
        );

        Assert.That(ex.Message, Is.EqualTo("O campo CategoriaId do produto não pode estar vazio"));

        ex = Assert.Throws<DomainException>(() =>
            new Produto("Nome", "Descricao", false, 100, Guid.NewGuid(), DateTime.Now, string.Empty, new Dimensoes(1, 1, 1))
        );

        Assert.That(ex.Message, Is.EqualTo("O campo Imagem do produto não pode estar vazio"));

        ex = Assert.Throws<DomainException>(() =>
            new Produto("Nome", "Descricao", false, 100, Guid.NewGuid(), DateTime.Now, "Imagem", new Dimensoes(0, 1, 1))
        );

        Assert.That(ex.Message, Is.EqualTo("O campo Altura não pode ser menor ou igual a 0"));
    }
}