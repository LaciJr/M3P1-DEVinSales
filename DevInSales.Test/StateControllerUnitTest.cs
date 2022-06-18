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
        var qtdState = context.State.Count();

        var controller = new StateController(context);

        var result = await controller.GetState(null);

        var expected = (result.Result as ObjectResult);

        var content = expected.Value as List<State>;

        Assert.That(content.Count, Is.EqualTo(qtdState));
        Assert.That(expected.StatusCode.ToString(), Is.EqualTo("200"));
    }

    [Test]
    public async Task GetStateFiltrandoPeloNome()
    {
        var context = new SqlContext(_contextOptions);
        var state = await context.State.FirstAsync(state => state.Name.Contains("Santa Catarina"));

        var controller = new StateController(context);

        var result = await controller.GetState("Santa Catarina");

        var expected = (result.Result as ObjectResult);

        var content = expected.Value as List<State>;

        Assert.That(content[0].Name.Contains(state.Name));
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
    public async Task GetCitybyStateIdCityComCityInexistente()
    {
        var context = new SqlContext(_contextOptions);

        var controller = new StateController(context);

        var result = await controller.GetByStateIdCity(42, "Abacaxi");

        var expected = result.Result as StatusCodeResult;

        Assert.That(expected.StatusCode.ToString(), Is.EqualTo("204"));
    }

    [Test]
    public async Task GetCitybyStateIdCityComNameNull()
    {
        var context = new SqlContext(_contextOptions);
        var state = await context.State.FindAsync(42);
        var city = new City
        {
            Id = 2,
            Name = "Florianopolis",
            State_Id = 42,
            State = state
        };
        await context.City.AddAsync(city);
        await context.SaveChangesAsync();

        var controller = new StateController(context);

        var result = await controller.GetByStateIdCity(42, "Florianopolis");

        var expected = result.Result as ObjectResult;
        var content = expected.Value as List<CityStateDTO>;

        Assert.That(expected.StatusCode.ToString(), Is.EqualTo("200"));
        Assert.That(content[0].Name_City, Is.EqualTo(city.Name));
    }

    [Test]
    public async Task GetCitybyStateIdUsandoCityDeOutroState()
    {
        var context = new SqlContext(_contextOptions);

        var city = new City
        {
            Id = 1,
            Name = "Abacaxi",
            State_Id = 11
        };
        context.City.Add(city);

        var controller = new StateController(context);

        var result = await controller.GetByStateIdCityId(42, 1);

        var expected = result.Result as StatusCodeResult;

        Assert.That(expected.StatusCode.ToString(), Is.EqualTo("400"));
    }

    [Test]
    public async Task GetCitybyStateIdCityIdUsandoIdInexistente()
    {
        var context = new SqlContext(_contextOptions);

        var controller = new StateController(context);

        var result = await controller.GetByStateIdCityId(0, 0);

        var expected = result.Result as StatusCodeResult;

        Assert.That(expected.StatusCode.ToString(), Is.EqualTo("404"));
    }

    [Test]
    public async Task GetCitybyStateIdCityIdTest()
    {
        var city = new City
        {
            Id = 1,
            Name = "Florianopolis",
            State_Id = 42,
        };

        var context = new SqlContext(_contextOptions);
        context.Add(city);

        var controller = new StateController(context);

        var result = await controller.GetByStateIdCityId(42, 1);

        var expected = result.Result as ObjectResult;

        var content = expected.Value as CityStateDTO;

        Assert.That(expected.StatusCode.ToString(), Is.EqualTo("200"));
        Assert.That(content.State_Id, Is.EqualTo(42));
        Assert.That(content.City_Id, Is.EqualTo(1));
        Assert.That(content.Name_City, Is.EqualTo("Florianopolis"));
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
        var qtdCity = context.City.Count();

        var controller = new StateController(context);

        var result = await controller.PostState(city, 42);

        var expected = result.Result as ObjectResult;

        Assert.That(expected.StatusCode.ToString(), Is.EqualTo("201"));
        Assert.That(context.City.Count() == qtdCity+1);
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
    public async Task PostStateAddresUsandoCityDeOutroState()
    {
        var context = new SqlContext(_contextOptions);
        var state = await context.State.FindAsync(42);
        var city = new City
        {
            Id = 2,
            Name = "Florianopolis",
            State_Id = 42,
            State = state
        };
        await context.City.AddAsync(city);

        var address = new Address
        {
        };

        var controller = new StateController(context);

        var result = await controller.PostState(address, 41, 2);

        var expected = result.Result as StatusCodeResult;

        Assert.That(expected.StatusCode.ToString(), Is.EqualTo("400"));
    }

    [Test]
    public async Task PostStateAddresUsandoAddresInvalido()
    {
        var context = new SqlContext(_contextOptions);
        var state = await context.State.FindAsync(42);
        var city = new City
        {
            Id = 2,
            Name = "Florianopolis",
            State_Id = 42,
            State = state
        };
        await context.City.AddAsync(city);

        var address = new Address
        {
            Id = 4,
            City = city,
            City_Id = 2,
            Street = "",
            Number = 0,
            CEP = null
        };

        var controller = new StateController(context);

        var result = await controller.PostState(address, 42, 2);

        var expected = result.Result as StatusCodeResult;

        Assert.That(expected.StatusCode.ToString(), Is.EqualTo("400"));
    }

    [Test]
    public async Task PostStateAddresUsandoAddresTest()
    {
        var context = new SqlContext(_contextOptions);
        var state = await context.State.FindAsync(42);
        var city = new City
        {
            Id = 3,
            Name = "Florianopolis",
            State_Id = 42,
            State = state
        };
        await context.City.AddAsync(city);

        var address = new Address
        {
            Id = 4,
            City = city,
            City_Id = 2,
            Street = "Rua fulano",
            Number = 49,
            CEP = "999999-99"
        };
        var qtdAddress = context.Address.Count();

        var controller = new StateController(context);

        var result = await controller.PostState(address, 42, 2);

        var expected = result.Result as ObjectResult;

        Assert.That(expected.StatusCode.ToString(), Is.EqualTo("201"));
        Assert.That(context.Address.Count() == qtdAddress + 1);
    }

    [Test]
    public async Task DeleteStateUsandoStateIdInexistente()
    {
        var context = new SqlContext(_contextOptions);
        var qtdState = context.State.Count();

        var controller = new StateController(context);

        var result = await controller.DeleteState(0);

        var expected = result as StatusCodeResult;

        Assert.That(expected.StatusCode.ToString(), Is.EqualTo("404"));
        Assert.That(context.State.Count(), Is.EqualTo(qtdState));
    }

    [Test]
    public async Task DeleteStateTest()
    {
        var context = new SqlContext(_contextOptions);
        
        var qtdState = context.State.Count();

        var controller = new StateController(context);

        var result = await controller.DeleteState(11);

        var expected = result as StatusCodeResult;

        Assert.That(expected.StatusCode.ToString(), Is.EqualTo("204"));
        Assert.That(context.State.Count() == qtdState-1);
    }
}