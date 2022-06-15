using DevInSales.Context;
using DevInSales.Controllers;
using DevInSales.DTOs;
using DevInSales.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Moq;
using System.Net;

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

        context.SaveChanges();
    }
    #endregion
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task GetUsersSemUsersCriados()
    {
        var context = new SqlContext(_contextOptions);

        var controller = new UserController(context);

        var result = await controller.Get("Test", "25/03/1997", "14/06/2022");

        var expected = (result.Result as ObjectResult).Value;

        Assert.That(expected.ToString(), Is.EqualTo("Nenhum usuário foi encontrado."));
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

        var expected = (result.Result as ObjectResult).Value;

        Assert.That(expected.ToString(), Is.EqualTo("{ id = 5 }"));

    }
}