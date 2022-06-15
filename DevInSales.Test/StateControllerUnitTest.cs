using DevInSales.Context;
using DevInSales.Controllers;
using DevInSales.DTOs;
using DevInSales.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace DevInSales.Test;

public class StateControllerUnitTest
{
    private readonly DbContextOptions<SqlContext> _contextOptions;
    #region Constructor
    public StateControllerUnitTest()
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
    public async Task GetStateSemFiltro()
    {
        var context = new SqlContext(_contextOptions);

        var controller = new StateController(context);

        var result = await controller.GetState(null);

        var expected = (result.Result as ObjectResult);

        var content = expected.Value as List<State>;

        Assert.That(content.Count, Is.EqualTo(27));
        Assert.That(expected.StatusCode.ToString(), Is.EqualTo("200"));
    }
}