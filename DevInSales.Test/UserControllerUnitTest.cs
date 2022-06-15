using DevInSales.Context;
using DevInSales.Controllers;
using DevInSales.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace DevInSales.Test;

public class UserControllerUnitTest
{
    private readonly DbContextOptions<SqlContext> _contextOptions;
    #region Constructor
    public UserControllerUnitTest()
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
    public async Task GetUsersTestSemFiltro()
    {
        var context = new SqlContext(_contextOptions);

        var controller = new UserController(context);

        var result = await controller.Get(null, null, null);

        var expected = (result.Result as ObjectResult).Value as List<UserResponseDTO>;

        Assert.That(expected.Count, Is.EqualTo(4));
    }

    [Test]
    public async Task GetUsersFiltrandoPorNameInexistente()
    {
        var context = new SqlContext(_contextOptions);

        var controller = new UserController(context);

        var result = await controller.Get("TESTE", "25/03/1997", "14/06/2022");

        var expected = (result.Result as ObjectResult).Value;

        Assert.That(expected.ToString(), Is.EqualTo("Nenhum usuário foi encontrado."));
    }

    [Test]
    public async Task GetUsersTestFiltrandoPorNome()
    {
        var context = new SqlContext(_contextOptions);

        var controller = new UserController(context);

        var result = await controller.Get("Romeu", null, null);

        var expected = (result.Result as ObjectResult).Value as List<UserResponseDTO>;

        Assert.That(expected.Count, Is.EqualTo(1));
        Assert.That(expected[0].Name.Contains("Romeu"));
    }

    [Test]
    public async Task GetUsersTestFiltrandoPorDataMinima()
    {
        var context = new SqlContext(_contextOptions);

        var controller = new UserController(context);

        var result = await controller.Get(null, "01/02/2000", null);

        var expected = (result.Result as ObjectResult).Value as List<UserResponseDTO>;

        Assert.That(expected.Count, Is.EqualTo(1));
        Assert.That(expected[0].Name.Contains("Romeu"));
    }

    [Test]
    public async Task GetUsersTestFiltrandoPorDataMaxima()
    {
        var context = new SqlContext(_contextOptions);

        var controller = new UserController(context);

        var result = await controller.Get(null, null, "11/04/1974");

        var expected = (result.Result as ObjectResult).Value as List<UserResponseDTO>;

        Assert.That(expected.Count, Is.EqualTo(1));
        Assert.That(expected[0].Name.Contains("Gustavo"));
    }


    [Test]
    public async Task CreateUserTest()
    {
        UserCreateDTO userDTO = new UserCreateDTO();
        userDTO.BirthDate = "25/03/1998";
        userDTO.Email = "test123@test123.com";
        userDTO.Name = "Teste";
        userDTO.Password = "1234";
        userDTO.ProfileId = 3;

        var context = new SqlContext(_contextOptions);

        var controller = new UserController(context);

        var result = await controller.Create(userDTO);

        var expected = (result.Result as ObjectResult);

        Assert.That(expected.StatusCode.ToString(), Is.EqualTo("201"));
        Assert.That(context.User.Count() == 5);

    }

    [Test]
    public async Task CreateUserMenorDeIdade()
    {
        UserCreateDTO userDTO = new UserCreateDTO();
        userDTO.BirthDate = "25/03/2018";
        userDTO.Email = "test123@test123.com";
        userDTO.Name = "Teste";
        userDTO.Password = "1234";
        userDTO.ProfileId = 1;

        var context = new SqlContext(_contextOptions);

        var controller = new UserController(context);

        var result = await controller.Create(userDTO);

        var expected = (result.Result as ObjectResult).Value;

        Assert.That(expected.ToString(), Is.EqualTo("O usuário deve ser maior de 18 anos."));
    }

    [Test]
    public async Task CreateUserDataAoContrario()
    {
        UserCreateDTO userDTO = new UserCreateDTO();
        userDTO.BirthDate = "03/25/2018";
        userDTO.Email = "test123@test123.com";
        userDTO.Name = "Teste";
        userDTO.Password = "1234";
        userDTO.ProfileId = 1;

        var context = new SqlContext(_contextOptions);

        var controller = new UserController(context);

        var result = await controller.Create(userDTO);

        var expected = (result.Result as ObjectResult).Value;

        Assert.That(expected.ToString(), Is.EqualTo("Data inválida."));
    }

    [Test]
    public async Task CreateUserSenhaInvalida()
    {
        UserCreateDTO userDTO = new UserCreateDTO();
        userDTO.BirthDate = "25/03/1998";
        userDTO.Email = "test123@test123.com";
        userDTO.Name = "Teste";
        userDTO.Password = "111";
        userDTO.ProfileId = 1;

        var context = new SqlContext(_contextOptions);

        var controller = new UserController(context);

        var result = await controller.Create(userDTO);

        var expected = (result.Result as ObjectResult).Value;

        Assert.That(expected.ToString(), Is.EqualTo("Senha inválida. Deve-se ter pelo menos um caractere diferente dos demais."));
    }

    [Test]
    public async Task CreateUserEmailJaExiste()
    {
        UserCreateDTO userDTO = new UserCreateDTO();
        userDTO.BirthDate = "25/03/1998";
        userDTO.Email = "romeu@lenda.com";
        userDTO.Name = "Teste";
        userDTO.Password = "1234";
        userDTO.ProfileId = 1;

        var context = new SqlContext(_contextOptions);

        var controller = new UserController(context);

        var result = await controller.Create(userDTO);

        var expected = (result.Result as ObjectResult).Value;

        Assert.That(expected.ToString(), Is.EqualTo("O email informado já existe."));
    }

    [Test]
    public async Task CreateUserProfileIdInvalido()
    {
        UserCreateDTO userDTO = new UserCreateDTO();
        userDTO.BirthDate = "25/03/1998";
        userDTO.Email = "test123@test123.com";
        userDTO.Name = "Teste";
        userDTO.Password = "1234";
        userDTO.ProfileId = 4;

        var context = new SqlContext(_contextOptions);

        var controller = new UserController(context);

        var result = await controller.Create(userDTO);

        var expected = (result.Result as ObjectResult).Value;

        Assert.That(expected.ToString(), Is.EqualTo("O perfil informado não foi encontrado."));
    }

    [Test]
    public void DeleteUserTest()
    {
        var context = new SqlContext(_contextOptions);

        var controller = new UserController(context);

        var result = controller.DeleteUser(5);

        var expected = (result.Result as ObjectResult);

        Assert.That(expected.Value.ToString(), Is.EqualTo("5"));
        Assert.That(expected.StatusCode.ToString(), Is.EqualTo("200"));
    }

    [Test]
    public void DeleteUserComIdInvalido()
    {
        var context = new SqlContext(_contextOptions);

        var controller = new UserController(context);

        var result = controller.DeleteUser(0);

        var expected = (result.Result as ObjectResult);

        Assert.That(expected.Value.ToString(), Is.EqualTo("O Id de Usuário de número 0 não foi encontrado."));
        Assert.That(expected.StatusCode.ToString(), Is.EqualTo("404"));
    }
}