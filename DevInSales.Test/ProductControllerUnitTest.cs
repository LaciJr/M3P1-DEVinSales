﻿using DevInSales.Context;
using DevInSales.Controllers;
using DevInSales.DTOs;
using DevInSales.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace DevInSales.Test;

public class ProductControllerUnitTest
{
    private readonly DbContextOptions<SqlContext> _contextOptions;
    #region Constructor
    public ProductControllerUnitTest()
    {
        _contextOptions = new DbContextOptionsBuilder<SqlContext>()
        .UseInMemoryDatabase("UserControllerUnitTest")
        .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
        .Options;

        using var context = new SqlContext(_contextOptions);

        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

    }
    #endregion

    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task GetProductPrecoMinMaiorQuePrecoMax()
    {
        var context = new SqlContext(_contextOptions);

        var controller = new ProductController(context);

        var result = await controller.GetProduct("Curso de Java", 200, 150);

        var expected = (result.Result as ObjectResult);

        Assert.That(expected.StatusCode.ToString(), Is.EqualTo("400"));
        Assert.That(expected.Value.ToString().Contains("O Preço Máximo (150) não pode ser menor que o Preço Mínimo (200)."));
    }

    [Test]
    public async Task GetProductFiltradoPorNomeExistente()
    {
        var context = new SqlContext(_contextOptions);

        var controller = new ProductController(context);

        var result = await controller.GetProduct("Curso de C Sharp", null, null);

        var expected = (result.Result as ObjectResult);

        var content = expected.Value as List<ProductGetDTO>;

        Assert.That(expected.StatusCode.ToString(), Is.EqualTo("200"));
        Assert.That(content.Count(), Is.EqualTo(1));
        Assert.That(content[0].Name.Contains("Curso de C Sharp"));
    }

    [Test]
    public async Task GetProductFiltradoPorNomeInexistente()
    {
        var context = new SqlContext(_contextOptions);

        var controller = new ProductController(context);

        var result = await controller.GetProduct("Abacaxi", null, null);

        var expected = (result.Result as StatusCodeResult);

        Assert.That(expected.StatusCode.ToString(), Is.EqualTo("204"));
    }

    [Test]
    public async Task GetProductFiltradoPorPrecoMin()
    {
        var context = new SqlContext(_contextOptions);

        var controller = new ProductController(context);

        var result = await controller.GetProduct(null, 310, null);

        var expected = (result.Result as ObjectResult);

        var content = expected.Value as List<ProductGetDTO>;

        Assert.That(expected.StatusCode.ToString(), Is.EqualTo("200"));
        Assert.That(content.Count(), Is.EqualTo(1));
        Assert.That(content[0].Name.Contains("Curso de Ruby"));
    }

    [Test]
    public async Task GetProductFiltradoPorPrecoMax()
    {
        var context = new SqlContext(_contextOptions);

        var controller = new ProductController(context);

        var result = await controller.GetProduct(null, null, 140);

        var expected = (result.Result as ObjectResult);

        var content = expected.Value as List<ProductGetDTO>;

        Assert.That(expected.StatusCode.ToString(), Is.EqualTo("200"));
        Assert.That(content.Count(), Is.EqualTo(1));
        Assert.That(content[0].Name.Contains("Curso de HTML5 e CSS3"));
    }

    [Test]
    public async Task GetProductTest()
    {
        var context = new SqlContext(_contextOptions);
        var qtdProduct = context.Product.Count();

        var controller = new ProductController(context);

        var result = await controller.GetProduct(null, null, null);

        var expected = (result.Result as ObjectResult);

        var content = expected.Value as List<ProductGetDTO>;

        Assert.That(expected.StatusCode.ToString(), Is.EqualTo("200"));
        Assert.That(content.Count(), Is.EqualTo(qtdProduct));
    }

    [Test]
    public async Task PostProductUsandoProdutoExistente()
    {
        var product = new ProductPostAndPutDTO
        {
            Name = "Curso de C Sharp",
            CategoryId = 1,
            Suggested_Price = 13m
        };

        var context = new SqlContext(_contextOptions);

        var controller = new ProductController(context);

        var result = await controller.PostProduct(product);

        var expected = (result.Result as ObjectResult);

        Assert.That(expected.StatusCode.ToString(), Is.EqualTo("400"));
        Assert.That(expected.Value.ToString(), Is.EqualTo("Já existe um produto com este nome."));
    }

    [Test]
    public async Task PostProductUsandoPriceInvalido()
    {
        var product = new ProductPostAndPutDTO
        {
            Name = "Teste",
            CategoryId = 1,
            Suggested_Price = 0m
        };

        var context = new SqlContext(_contextOptions);

        var controller = new ProductController(context);

        var result = await controller.PostProduct(product);

        var expected = (result.Result as ObjectResult);

        Assert.That(expected.StatusCode.ToString(), Is.EqualTo("400"));
        Assert.That(expected.Value.ToString(), Is.EqualTo("O preço sugerido não pode ser menor ou igual a 0."));
    }

    [Test]
    public async Task PostProductTest()
    {
        var product = new ProductPostAndPutDTO
        {
            Name = "Abacaxi",
            CategoryId = 1,
            Suggested_Price = 13m
        };

        var context = new SqlContext(_contextOptions);
        var qtdProduct = context.Product.Count() + 1;

        var controller = new ProductController(context);

        var result = await controller.PostProduct(product);

        var expected = (result.Result as ObjectResult);

        Assert.That(expected.StatusCode.ToString(), Is.EqualTo("201"));
        Assert.That(context.Product.Count(), Is.EqualTo(qtdProduct));
    }

    [Test]
    public async Task PutProductUsandoIdInexistente()
    {
        var product = new ProductPostAndPutDTO
        {
            Name = "Teste",
            CategoryId = 1,
            Suggested_Price = 13m
        };

        var context = new SqlContext(_contextOptions);

        var controller = new ProductController(context);

        var result = await controller.PutProduct(0 ,product);

        var expected = (result.Result as ObjectResult);

        Assert.That(expected.StatusCode.ToString(), Is.EqualTo("404"));
        Assert.That(expected.Value.ToString(), Is.EqualTo("Não existe um produto com esta Id."));
    }

    [Test]
    public async Task PutProductUsandoNomeExistente()
    {
        var product = new ProductPostAndPutDTO
        {
            Name = "Curso de Java",
            CategoryId = 1,
            Suggested_Price = 13m
        };

        var context = new SqlContext(_contextOptions);

        var controller = new ProductController(context);

        var result = await controller.PutProduct(2, product);

        var expected = (result.Result as ObjectResult);

        Assert.That(expected.StatusCode.ToString(), Is.EqualTo("400"));
        Assert.That(expected.Value.ToString(), Is.EqualTo("Já existe um produto com este nome."));
    }

    [Test]
    public async Task PutProductUsandoPriceInvalido()
    {
        var product = new ProductPostAndPutDTO
        {
            Name = "Teste",
            CategoryId = 1,
            Suggested_Price = 0m
        };

        var context = new SqlContext(_contextOptions);

        var controller = new ProductController(context);

        var result = await controller.PutProduct(1, product);

        var expected = (result.Result as ObjectResult);

        Assert.That(expected.StatusCode.ToString(), Is.EqualTo("400"));
        Assert.That(expected.Value.ToString(), Is.EqualTo("O preço sugerido não pode ser menor ou igual a 0."));
    }

    [Test]
    public async Task PutProductUsandoNomeNulo()
    {
        var product = new ProductPostAndPutDTO
        {
            Name = null,
            CategoryId = 1,
            Suggested_Price = 10m
        };

        var context = new SqlContext(_contextOptions);

        var controller = new ProductController(context);

        var result = await controller.PutProduct(1, product);

        var expected = (result.Result as ObjectResult);

        Assert.That(expected.StatusCode.ToString(), Is.EqualTo("400"));
        Assert.That(expected.Value.ToString(), Is.EqualTo("Nome ou Preço Sugerido são Nulos."));
    }

    [Test]
    public async Task PutProductTest()
    {
        var product = new ProductPostAndPutDTO
        {
            Name = "Curso de C#",
            CategoryId = 1,
            Suggested_Price = 239.99M
        };

        var context = new SqlContext(_contextOptions);

        var controller = new ProductController(context);

        var result = await controller.PutProduct(1, product);

        var expected = (result.Result as StatusCodeResult);

        Assert.That(expected.StatusCode.ToString(), Is.EqualTo("204"));
    }

    [Test]
    public async Task PatchProductUsandoIdInexistente()
    {
        var product = new ProductPatchDTO
        {
            Name = "Curso de C#",
            Suggested_Price = 239.99M
        };

        var context = new SqlContext(_contextOptions);

        var controller = new ProductController(context);

        var result = await controller.PatchProduct(0, product);

        var expected = (result.Result as StatusCodeResult);

        Assert.That(expected.StatusCode.ToString(), Is.EqualTo("400"));
    }

    [Test]
    public async Task PatchProductUsandoNameNulo()
    {
        var product = new ProductPatchDTO
        {
            Name = null,
            Suggested_Price = 100m,
        };

        var context = new SqlContext(_contextOptions);

        var controller = new ProductController(context);

        var result = await controller.PatchProduct(5, product);

        var expected = (result.Result as StatusCodeResult);

        Assert.That(expected.StatusCode.ToString(), Is.EqualTo("204"));
    }

    [Test]
    public async Task PatchProductUsandoNameExistente()
    {
        var product = new ProductPatchDTO
        {
            Name = "Curso de Java",
            Suggested_Price = 100m,
        };

        var context = new SqlContext(_contextOptions);

        var controller = new ProductController(context);

        var result = await controller.PatchProduct(2, product);

        var expected = (result.Result as StatusCodeResult);

        Assert.That(expected.StatusCode.ToString(), Is.EqualTo("400"));
    }

    [Test]
    public async Task PatchProductUsandoNameEPriceNulo()
    {
        var product = new ProductPatchDTO
        {
            Name = null,
        };

        var context = new SqlContext(_contextOptions);

        var controller = new ProductController(context);

        var result = await controller.PatchProduct(2, product);

        var expected = (result.Result as StatusCodeResult);

        Assert.That(expected.StatusCode.ToString(), Is.EqualTo("400"));
    }

    [Test]
    public async Task DeleteProductUsandoIdInexistente()
    {
        var context = new SqlContext(_contextOptions);

        var controller = new ProductController(context);

        var result = await controller.DeleteProduct(0);

        var expected = (result as ObjectResult);

        Assert.That(expected.StatusCode.ToString(), Is.EqualTo("404"));
        Assert.That(expected.Value.ToString(), Is.EqualTo("O Id de Produto de número 0 não foi encontrado."));
    }

    [Test]
    public async Task DeleteProductComProductEmOrdemDeCompra()
    {
        var context = new SqlContext(_contextOptions);
       
        var user = await context.User.FindAsync(1);
        var seller = await context.User.FindAsync(2);
        var order = new Order
        {
            Id = 1,
            User = user,
            UserId = 1,
            Seller = seller,
            SellerId = 2,
            Date_Order = DateTime.Now,
            Shipping_Company = "Sedex",
            Shipping_Company_Price = 22m,
        };
        var product = await context.Product.FindAsync(3);
        var orderProduct = new OrderProduct
        {
            Id = 1,
            Unit_Price = 189.99M,
            Amount = 1,
            Order = order,
            Product = product
        };
        await context.Order_Product.AddAsync(orderProduct);

        await context.SaveChangesAsync();
        
        var controller = new ProductController(context);

        var result = await controller.DeleteProduct(3);

        var expected = (result as ObjectResult);

        Assert.That(expected.StatusCode.ToString(), Is.EqualTo("400"));
        Assert.That(expected.Value.ToString(), Is.EqualTo("O Id de Produto de número 3 possui uma Ordem de Produto vinculada, por este motivo não pode ser deletado."));
    }

    [Test]
    public async Task DeleteProductTest()
    {
        var context = new SqlContext(_contextOptions);
        var qtdProduct = context.Product.Count() - 1;

        var controller = new ProductController(context);

        var result = await controller.DeleteProduct(10);

        var expected = (result as StatusCodeResult);

        Assert.That(expected.StatusCode.ToString(), Is.EqualTo("204"));
        Assert.That(context.Product.Count() == qtdProduct);
    }
}
