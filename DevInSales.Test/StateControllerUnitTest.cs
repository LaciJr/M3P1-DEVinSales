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

    [Test]
    public async Task GetStateFiltrandoPeloNome()
    {
        var context = new SqlContext(_contextOptions);

        var controller = new StateController(context);

        var result = await controller.GetState("Santa Catarina");

        var expected = (result.Result as ObjectResult);

        var content = expected.Value as List<State>;

        Assert.That(content.Count, Is.EqualTo(1));
        Assert.That(content[0].Name, Is.EqualTo("Santa Catarina"));
        Assert.That(expected.StatusCode.ToString(), Is.EqualTo("200"));
    }

    [Test]
    public async Task GetStateFiltrandoPorNomeInexistente()
    {
        var context = new SqlContext(_contextOptions);

        var controller = new StateController(context);

        var result = await controller.GetState("Abacaxi");

        var expected = result.Result as StatusCodeResult;

        Assert.That(expected.StatusCode.ToString(), Is.EqualTo("204"));
    }

    [Test]
    public async Task GetStateIdUsandoId()
    {
        var context = new SqlContext(_contextOptions);

        var controller = new StateController(context);

        var result = await controller.GetStateId(42);

        var expected = (result.Result as ObjectResult);

        Assert.That(expected.Value.ToString().Contains("State_Id encontrado com sucesso"));
        Assert.That(expected.StatusCode.ToString(), Is.EqualTo("200"));
    }

    [Test]
    public async Task GetStateIdUsandoIdInexistente()
    {
        var context = new SqlContext(_contextOptions);

        var controller = new StateController(context);

        var result = await controller.GetStateId(0);

        var expected = (result.Result as ObjectResult);

        Assert.That(expected.Value.ToString(), Is.EqualTo("{ message = State_Id não encontrado }"));
        Assert.That(expected.StatusCode.ToString(), Is.EqualTo("404"));
    }

    [Test]
    public async Task GetCitybyStateIdUsandoStateIdInexistente()
    {
        var context = new SqlContext(_contextOptions);

        var controller = new StateController(context);

        var result = await controller.GetByStateIdCity(0, null);

        var expected = result.Result as StatusCodeResult;

        Assert.That(expected.StatusCode.ToString(), Is.EqualTo("404"));
    }

    [Test]
    public async Task PutStateUsandoIdDiferenteDoStateId()
    {
        var state = new State
        {
            Id = 42,
            Name = "Santa Catarina",
            Initials = "SC"
        };
        var context = new SqlContext(_contextOptions);

        var controller = new StateController(context);

        var result = await controller.PutState(1, state);

        var expected = result as StatusCodeResult;

        Assert.That(expected.StatusCode.ToString(), Is.EqualTo("400"));
    }

    [Test]
    public async Task PutStateTest()
    {
        var state = new State
        {
            Id = 42,
            Name = "Santa Catarina",
            Initials = "SC"
        };
        var context = new SqlContext(_contextOptions);

        var controller = new StateController(context);

        var result = await controller.PutState(42, state);

        var expected = result as StatusCodeResult;

        Assert.That(expected.StatusCode.ToString(), Is.EqualTo("204"));
    }

    [Test]
    public async Task PutStateDeStateInexistente()
    {
        var state = new State
        {
            Id = 100,
            Name = "Abacaxi",
            Initials = "AB"
        };
        var context = new SqlContext(_contextOptions);

        var controller = new StateController(context);

        var result = await controller.PutState(100, state);

        var expected = result as StatusCodeResult;

        Assert.That(expected.StatusCode.ToString(), Is.EqualTo("404"));
    }

    [Test]
    public async Task PostStateUsandoStateIdInexistente()
    {
        var city = new City
        {
        };
        var context = new SqlContext(_contextOptions);

        var controller = new StateController(context);

        var result = await controller.PostState(city, 0);

        var expected = result.Result as StatusCodeResult;

        Assert.That(expected.StatusCode.ToString(), Is.EqualTo("404"));
    }

    [Test]
    public async Task PostStateTest()
    {
        var city = new City
        {
            Id = 1,
            Name = "Florianópolis",
            State_Id = 42,
        };
        var context = new SqlContext(_contextOptions);

        var controller = new StateController(context);

        var result = await controller.PostState(city, 42);

        var expected = result.Result as ObjectResult;

        Assert.That(expected.StatusCode.ToString(), Is.EqualTo("201"));
        Assert.That(context.City.Count() == 1);
    }

    [Test]
    public async Task PostStateAddresUsandoStateIdInexistente()
    {
        var address = new Address
        {
        };
        var context = new SqlContext(_contextOptions);

        var controller = new StateController(context);

        var result = await controller.PostState(address, 0, 1);

        var expected = result.Result as StatusCodeResult;

        Assert.That(expected.StatusCode.ToString(), Is.EqualTo("404"));
    }

    [Test]
    public async Task PostStateAddresUsandoCityIdInexistente()
    {
        var address = new Address
        {
        };
        var context = new SqlContext(_contextOptions);

        var controller = new StateController(context);

        var result = await controller.PostState(address, 42, 0);

        var expected = result.Result as StatusCodeResult;

        Assert.That(expected.StatusCode.ToString(), Is.EqualTo("404"));
    }

    [Test]
    public async Task DeleteStateUsandoStateIdInexistente()
    {
        var context = new SqlContext(_contextOptions);

        var controller = new StateController(context);

        var result = await controller.DeleteState(0);

        var expected = result as StatusCodeResult;

        Assert.That(expected.StatusCode.ToString(), Is.EqualTo("404"));
    }

    [Test]
    public async Task DeleteStateTest()
    {
        var context = new SqlContext(_contextOptions);
        
        var qtdState = context.State.Count();
        
        var state = new State
        {
            Id = 101,
            Name = "Abacaxi",
            Initials = "AB"
        };
        context.State.Add(state);

        var controller = new StateController(context);

        var result = await controller.DeleteState(101);

        var expected = result as StatusCodeResult;

        Assert.That(expected.StatusCode.ToString(), Is.EqualTo("204"));
        Assert.That(context.State.Count() == qtdState);
    }
}