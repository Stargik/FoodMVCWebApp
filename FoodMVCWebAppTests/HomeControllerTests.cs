using FoodMVCWebApp.Controllers;
using FoodMVCWebApp.Data;
using Microsoft.AspNetCore.Mvc;

namespace FoodMVCWebAppTests;

[TestClass]
public class HomeControllerTests
{
    [TestMethod]
    public void TestPrivacyView()
    {
        var controller = new HomeController();
        var result = controller.Privacy() as ViewResult;
        Assert.AreEqual("Privacy", result.ViewName);

    }
}

