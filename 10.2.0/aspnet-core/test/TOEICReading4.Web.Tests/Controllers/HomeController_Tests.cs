using TOEICReading4.Models.TokenAuth;
using TOEICReading4.Web.Controllers;
using Shouldly;
using System.Threading.Tasks;
using Xunit;

namespace TOEICReading4.Web.Tests.Controllers;

public class HomeController_Tests : TOEICReading4WebTestBase
{
    [Fact]
    public async Task Index_Test()
    {
        await AuthenticateAsync(null, new AuthenticateModel
        {
            UserNameOrEmailAddress = "admin",
            Password = "123qwe"
        });

        //Act
        var response = await GetResponseAsStringAsync(
            GetUrl<HomeController>(nameof(HomeController.Index))
        );

        //Assert
        response.ShouldNotBeNullOrEmpty();
    }
}