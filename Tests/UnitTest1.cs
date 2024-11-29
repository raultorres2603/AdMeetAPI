using AdMeet.Contexts;
using AdMeet.Controllers;
using AdMeet.Services;
using Moq;

namespace Tests;

public class UnitTest1
{
    [Fact]
    public async void CheckIfItUserExist()
    {
        // Arrange
        var mockProductService = new Mock<UserServices>(Mock.Of<AppDbContext>());
        UserController userController = new UserController(mockProductService.Object);
        await userController.GetUserById("af71259b-4aa5-407a-9f29-e98cdf791bbf");
        mockProductService.Verify(uS => uS.GetUserById("af71259b-4aa5-407a-9f29-e98cdf791bbf"), Times.Once);

        // Error: El error es que no se puede pasar un objeto DbContextOptions<AppDbContext> como parametro en el constructor de AppDbContext, ya que no es el constructor correcto.
        // La solucion es crear un contexto de prueba con el metodo CreateDbContext del que se esta utilizando, y luego pasar ese contexto como parametro al constructor de UserServices.
    }
}