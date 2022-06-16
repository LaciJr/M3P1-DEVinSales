using DevInSales.Context;
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

        var controller = new ProductController(context);

        var result = await controller.GetProduct(null, null, null);

        var expected = (result.Result as ObjectResult);

        var content = expected.Value as List<ProductGetDTO>;

        Assert.That(expected.StatusCode.ToString(), Is.EqualTo("200"));
        Assert.That(content.Count(), Is.EqualTo(10));
    }
}
