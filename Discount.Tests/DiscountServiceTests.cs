using DiscountServer.Services;
using DiscountStorage;
using DiscountStorage.Models;
using DiscountShared;
using Microsoft.EntityFrameworkCore;

public class DiscountServiceTests
{
    private DiscountDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<DiscountDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new DiscountDbContext(options);
    }

    [Fact]
    public async Task GenerateCodes_Should_Create_Unique_Codes()
    {
        // Arrange
        var db = GetInMemoryDbContext();
        var service = new DiscountServiceImpl(db);

        var request = new GenerateRequest { Count = 10, Length = 8 };

        // Act
        var response = await service.GenerateCodes(request, FakeServerCallContext.Create());

        // Assert
        Assert.True(response.Result);
        Assert.Equal(10, db.DiscountCodes.Count());
        Assert.Equal(10, db.DiscountCodes.Select(c => c.Code).Distinct().Count());
    }

    [Fact]
    public async Task UseCode_Should_Return_Success_If_Code_Valid()
    {
        // Arrange
        var db = GetInMemoryDbContext();
        var code = new DiscountCode { Code = "TEST1234", Used = false };
        db.DiscountCodes.Add(code);
        db.SaveChanges();

        var service = new DiscountServiceImpl(db);
        var request = new UseCodeRequest { Code = "TEST1234" };

        // Act
        var response = await service.UseCode(request, FakeServerCallContext.Create());

        // Assert
        Assert.Equal(0u, response.Result);
        Assert.True(code.Used);
    }

    [Fact]
    public async Task UseCode_Should_Return_AlreadyUsed()
    {
        var db = GetInMemoryDbContext();
        var code = new DiscountCode { Code = "USED1234", Used = true };
        db.DiscountCodes.Add(code);
        db.SaveChanges();

        var service = new DiscountServiceImpl(db);
        var request = new UseCodeRequest { Code = "USED1234" };

        var response = await service.UseCode(request, FakeServerCallContext.Create());

        Assert.Equal(2u, response.Result);
    }

    [Fact]
    public async Task UseCode_Should_Return_NotFound()
    {
        var db = GetInMemoryDbContext();
        var service = new DiscountServiceImpl(db);
        var request = new UseCodeRequest { Code = "NOTEXIST" };

        var response = await service.UseCode(request, FakeServerCallContext.Create());

        Assert.Equal(1u, response.Result);
    }
}
