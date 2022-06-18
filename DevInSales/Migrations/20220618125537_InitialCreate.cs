using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable
namespace DevInSales.Migrations
{
    [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "varchar(100)", nullable: false),
                    slug = table.Column<string>(type: "varchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Profile",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Permissão = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profile", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ShippingCompany",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShippingCompany", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "State",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Initials = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_State", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "varchar(100)", nullable: false),
                    suggested_price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.id);
                    table.ForeignKey(
                        name: "FK_Product_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    birth_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProfileId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.id);
                    table.ForeignKey(
                        name: "FK_User_Profile_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profile",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "City",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    State_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_City", x => x.Id);
                    table.ForeignKey(
                        name: "FK_City_State_State_Id",
                        column: x => x.State_Id,
                        principalTable: "State",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StatePrice",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StateId = table.Column<int>(type: "int", nullable: false),
                    ShippingCompanyId = table.Column<int>(type: "int", nullable: false),
                    base_price = table.Column<decimal>(type: "decimal", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatePrice", x => x.id);
                    table.ForeignKey(
                        name: "FK_StatePrice_ShippingCompany_ShippingCompanyId",
                        column: x => x.ShippingCompanyId,
                        principalTable: "ShippingCompany",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StatePrice_State_StateId",
                        column: x => x.StateId,
                        principalTable: "State",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    SellerId = table.Column<int>(type: "int", nullable: false),
                    date_order = table.Column<DateTime>(type: "date", nullable: false),
                    shipping_Company = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    shipping_company_price = table.Column<decimal>(type: "decimal", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.id);
                    table.ForeignKey(
                        name: "FK_Order_User_SellerId",
                        column: x => x.SellerId,
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Order_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Address",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    City_Id = table.Column<int>(type: "int", nullable: false),
                    Street = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CEP = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Number = table.Column<int>(type: "int", nullable: false),
                    Complement = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Address_City_City_Id",
                        column: x => x.City_Id,
                        principalTable: "City",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CityPrice",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CityId = table.Column<int>(type: "int", nullable: false),
                    base_price = table.Column<decimal>(type: "decimal", nullable: false),
                    ShippingCompanyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CityPrice", x => x.id);
                    table.ForeignKey(
                        name: "FK_CityPrice_City_CityId",
                        column: x => x.CityId,
                        principalTable: "City",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CityPrice_ShippingCompany_ShippingCompanyId",
                        column: x => x.ShippingCompanyId,
                        principalTable: "ShippingCompany",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Order_Product",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    unit_price = table.Column<decimal>(type: "decimal", nullable: false),
                    amount = table.Column<int>(type: "int", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order_Product", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Order_Product_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Order_Product_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Delivery",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    AddressId = table.Column<int>(type: "int", nullable: false),
                    delivery_Forecast = table.Column<DateTime>(type: "date", nullable: false),
                    delivery_Date = table.Column<DateTime>(type: "date", nullable: true),
                    status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Delivery", x => x.id);
                    table.ForeignKey(
                        name: "FK_Delivery_Address_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Address",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Delivery_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Address",
                columns: new[] { "Id", "CEP", "City_Id", "Complement", "Number", "Street" },
                values: new object[,]
                {
                    { 1, "999999-99", 11, "casa", 22, "Rua Lateral" },
                    { 2, "999999-99", 12, "apto", 45, "Rua Frente" },
                    { 3, "999999-99", 13, "casa", 123, "Rua Lateral" }
                });

            migrationBuilder.InsertData(
                table: "Category",
                columns: new[] { "id", "name", "slug" },
                values: new object[] { 1, "Categoria Padrão", "categoria-padrao" });

            migrationBuilder.InsertData(
                table: "Profile",
                columns: new[] { "id", "Permissão", "Role" },
                values: new object[,]
                {
                    { 1, 1, "Usuário" },
                    { 2, 2, "Gerente" },
                    { 3, 3, "Administrador" }
                });

            migrationBuilder.InsertData(
                table: "ShippingCompany",
                columns: new[] { "id", "Name" },
                values: new object[,]
                {
                    { 1, "Rapidex" },
                    { 2, "Veloz e Feroz" },
                    { 3, "Além Paraíba" },
                    { 4, "Empresa Padrão" }
                });

            migrationBuilder.InsertData(
                table: "State",
                columns: new[] { "Id", "Initials", "Name" },
                values: new object[,]
                {
                    { 11, "RO", "Rond�nia" },
                    { 12, "AC", "Acre" },
                    { 13, "AM", "Amazonas" },
                    { 14, "RR", "Roraima" },
                    { 15, "PA", "Par�" },
                    { 16, "AP", "Amap�" },
                    { 17, "TO", "Tocantins" },
                    { 21, "MA", "Maranh�o" },
                    { 22, "PI", "Piau�" },
                    { 23, "CE", "Cear�" },
                    { 24, "RN", "Rio Grande do Norte" },
                    { 25, "PB", "Para�ba" },
                    { 26, "PE", "Pernanmbuco" },
                    { 27, "AL", "Alagoas" },
                    { 28, "SE", "Sergipe" },
                    { 29, "BA", "Bahia" },
                    { 31, "MG", "Minas Gerais" },
                    { 32, "ES", "Espirito Santo" },
                    { 33, "RJ", "Rio de Janeiro" },
                    { 35, "SP", "S�o Paulo" },
                    { 41, "PR", "Paran�" },
                    { 42, "SC", "Santa Catarina" },
                    { 43, "RS", "Rio Grande do Sul" },
                    { 50, "MS", "Mato Grosso do Sul" },
                    { 51, "MT", "Mato Grosso" },
                    { 52, "GO", "Goias" },
                    { 53, "DF", "Distrito Federal" }
                });

            migrationBuilder.InsertData(
                table: "CityPrice",
                columns: new[] { "id", "base_price", "CityId", "ShippingCompanyId" },
                values: new object[,]
                {
                    { 1, 10m, 1, 1 },
                    { 2, 20m, 1, 2 },
                    { 3, 30m, 1, 3 },
                    { 4, 21m, 2, 1 },
                    { 5, 22m, 2, 2 },
                    { 6, 23m, 2, 3 },
                    { 7, 31m, 3, 1 },
                    { 8, 32m, 3, 2 },
                    { 9, 33m, 3, 3 },
                    { 10, 5m, 1, 4 },
                    { 11, 6m, 2, 4 },
                    { 12, 7m, 3, 4 }
                });

            migrationBuilder.InsertData(
                table: "Product",
                columns: new[] { "id", "CategoryId", "name", "suggested_price" },
                values: new object[,]
                {
                    { 1, 1, "Curso de C Sharp", 259.99m },
                    { 2, 1, "Curso de Java", 249.99m },
                    { 3, 1, "Curso de Delphi", 189.99m },
                    { 4, 1, "Curso de React", 289.99m },
                    { 5, 1, "Curso de HTML5 e CSS3", 139.99m },
                    { 6, 1, "Curso de JavaScript", 219.99m },
                    { 7, 1, "Curso de Angular", 199.99m },
                    { 8, 1, "Curso de Ruby", 319.99m },
                    { 9, 1, "Curso de Kotlin", 289.99m },
                    { 10, 1, "Curso de Python", 229.99m }
                });

            migrationBuilder.InsertData(
                table: "StatePrice",
                columns: new[] { "id", "base_price", "ShippingCompanyId", "StateId" },
                values: new object[,]
                {
                    { 1, 17m, 1, 11 },
                    { 2, 20m, 1, 22 },
                    { 3, 30m, 1, 33 },
                    { 4, 19m, 2, 11 },
                    { 5, 29m, 2, 22 },
                    { 6, 37m, 2, 33 },
                    { 7, 10m, 3, 11 },
                    { 8, 35m, 3, 22 },
                    { 9, 33m, 3, 33 },
                    { 10, 5m, 4, 11 },
                    { 11, 6m, 4, 22 },
                    { 12, 7m, 4, 33 }
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "id", "birth_date", "email", "name", "password", "ProfileId" },
                values: new object[,]
                {
                    { 1, new DateTime(2000, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "romeu@lenda.com", "Romeu A Lenda", "romeu123@", 1 },
                    { 2, new DateTime(1974, 4, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "gustavo_levi_ferreira@gmail.com", "Gustavo Levi Ferreira", "!romeu321", 2 },
                    { 3, new DateTime(1986, 3, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "lemosluiz@gmail.com", "Henrique Luiz Lemos", "lemos$2022", 3 },
                    { 4, new DateTime(1996, 8, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "tomas.paulo.aragao@hotmail.com", "Tomás Paulo Aragão", "$tpa1996", 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Address_City_Id",
                table: "Address",
                column: "City_Id");

            migrationBuilder.CreateIndex(
                name: "IX_City_State_Id",
                table: "City",
                column: "State_Id");

            migrationBuilder.CreateIndex(
                name: "IX_CityPrice_CityId",
                table: "CityPrice",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_CityPrice_ShippingCompanyId",
                table: "CityPrice",
                column: "ShippingCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Delivery_AddressId",
                table: "Delivery",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Delivery_OrderId",
                table: "Delivery",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_SellerId",
                table: "Order",
                column: "SellerId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_UserId",
                table: "Order",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_Product_OrderId",
                table: "Order_Product",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_Product_ProductId",
                table: "Order_Product",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_CategoryId",
                table: "Product",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_StatePrice_ShippingCompanyId",
                table: "StatePrice",
                column: "ShippingCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_StatePrice_StateId",
                table: "StatePrice",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_User_ProfileId",
                table: "User",
                column: "ProfileId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CityPrice");

            migrationBuilder.DropTable(
                name: "Delivery");

            migrationBuilder.DropTable(
                name: "Order_Product");

            migrationBuilder.DropTable(
                name: "StatePrice");

            migrationBuilder.DropTable(
                name: "Address");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "ShippingCompany");

            migrationBuilder.DropTable(
                name: "City");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "State");

            migrationBuilder.DropTable(
                name: "Profile");
        }
    }
}
