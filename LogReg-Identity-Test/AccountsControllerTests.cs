using Moq;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using LogReg_Identity.Areas.Identity.Data;
using LogReg_Identity.Areas.Identity.Pages.Account;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity.UI.Services;

public class AccountControllerTests
{
    private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;
    private readonly Mock<SignInManager<ApplicationUser>> _mockSignInManager;


    private readonly Mock<ILogger<LoginModel>> _mockLogger;
    private readonly LoginModel _loginModel;


    private readonly Mock<ILogger<RegisterModel>> _mockRegisterLogger;
    private readonly RegisterModel _registerModel;
    
    private readonly Mock<IEmailSender> _mockEmailSender;
    private readonly Mock<IUserStore<ApplicationUser>> _mockUserStore;

    private readonly Mock<IUserEmailStore<ApplicationUser>> _mockEmailStore;

    public AccountControllerTests()
    {
        // Mock UserStore with email support
        _mockUserStore = new Mock<IUserStore<ApplicationUser>>();

        _mockEmailStore = new Mock<IUserEmailStore<ApplicationUser>>();
        _mockUserStore.As<IUserEmailStore<ApplicationUser>>().SetReturnsDefault(_mockEmailStore.Object);

        // Setup email store methods as needed
        _mockEmailStore.Setup(es => es.GetEmailAsync(It.IsAny<ApplicationUser>(), CancellationToken.None))
            .ReturnsAsync((ApplicationUser user, CancellationToken _) => user.Email);

        _mockEmailStore.Setup(es => es.SetEmailAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>(), CancellationToken.None))
            .Callback((ApplicationUser user, string email, CancellationToken _) => user.Email = email)
            .Returns(Task.CompletedTask);

        // Mock UserManager
        _mockUserManager = new Mock<UserManager<ApplicationUser>>(
            _mockUserStore.Object,
            //_mockEmailStore.Object,
            null, 
            null, 
            null, 
            null, 
            null, 
            null, 
            null, 
            null
        );

        // Mock SignInManager
        _mockSignInManager = new Mock<SignInManager<ApplicationUser>>(
            _mockUserManager.Object,
            Mock.Of<IHttpContextAccessor>(),
            Mock.Of<IUserClaimsPrincipalFactory<ApplicationUser>>(),
            null, 
            null, 
            null, 
            null
        );

        // Mock Logger for LoginModel
        _mockLogger = new Mock<ILogger<LoginModel>>();

        // Mock Logger for RegisterModel
        _mockRegisterLogger = new Mock<ILogger<RegisterModel>>();

        // Mock EmailSender
        _mockEmailSender = new Mock<IEmailSender>();




        // Initialize LoginModel with the mocks
        _loginModel = new LoginModel(_mockSignInManager.Object, _mockLogger.Object);

        //// Initialize RegisterModel with the mocks
        //_registerModel = new RegisterModel(
        //    _mockUserManager.Object,
        //    _mockUserStore.Object,
        //    _mockSignInManager.Object,
        //    _mockRegisterLogger.Object,
        //    _mockEmailSender.Object);
    }

    [Fact]
    public async Task Login_ValidUser_ShouldRedirectToHome()
    {
        // Arrange: Set up the mock SignInManager to return a successful result
        _mockSignInManager.Setup(s => s.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), false))
                          .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

        var loginInput = new LoginModel.InputModel
        {
            Email = "test@test.com",
            Password = "Test@123",
            RememberMe = false
        };

        _loginModel.Input = loginInput;

        // Act: Call the Login method
        var result = await _loginModel.OnPostAsync("/");

        // Assert: Verify the result
        var redirectResult = Assert.IsType<LocalRedirectResult>(result);
        Assert.Equal("/", redirectResult.Url); // Assuming a successful login redirects to home page
    }

    [Fact]
    public async Task Login_InvalidUser_ShouldReturnPageWithModelError()
    {
        // Arrange: Set up the mock SignInManager to return a failure result
        _mockSignInManager.Setup(s => s.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), false))
                          .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Failed);

        var loginInput = new LoginModel.InputModel
        {
            Email = "test@test.com",
            Password = "WrongPassword",
            RememberMe = false
        };

        _loginModel.Input = loginInput;

        // Act: Call the Login method
        var result = await _loginModel.OnPostAsync("/");

        // Assert: Verify the result
        var pageResult = Assert.IsType<PageResult>(result);
        Assert.False(_loginModel.ModelState.IsValid); // Check that the model state is invalid due to the login failure
    }

    //[Fact]
    //public async Task Register_ValidUser_ShouldRedirectToConfirmation()
    //{
    //    // Arrange: Set up the mock UserManager to return a success result
    //    _mockUserManager.Setup(um => um.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
    //                    .ReturnsAsync(IdentityResult.Success);

    //    // Mock successful email sending
    //    _mockEmailSender.Setup(e => e.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
    //                    .Returns(Task.CompletedTask);

    //    var registerInput = new RegisterModel.InputModel
    //    {
    //        Email = "newuser@test.com",
    //        Password = "NewPassword@123",
    //        ConfirmPassword = "NewPassword@123",
    //        FirstName = "John",
    //        LastName = "Doe",
    //        Phone = "123-456-7890"
    //    };

    //    _registerModel.Input = registerInput;

    //    // Act: Call the Register method
    //    var result = await _registerModel.OnPostAsync("/");

    //    // Assert: Verify the result
    //    var redirectResult = Assert.IsType<RedirectToPageResult>(result);
    //    Assert.Equal("RegisterConfirmation", redirectResult.PageName);
    //}

    //[Fact]
    //public async Task Register_InvalidUser_ShouldReturnPageWithModelError()
    //{
    //    // Arrange: Set up the mock UserManager to return a failure result
    //    _mockUserManager.Setup(um => um.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
    //                    .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Error occurred" }));

    //    // Mock email sending
    //    _mockEmailSender.Setup(e => e.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
    //                    .Returns(Task.CompletedTask);

    //    var registerInput = new RegisterModel.InputModel
    //    {
    //        Email = "newuser@test.com",
    //        Password = "NewPassword@123",
    //        ConfirmPassword = "DifferentPassword@123", // Mismatched password
    //        FirstName = "John",
    //        LastName = "Doe",
    //        Phone = "123-456-7890"
    //    };

    //    _registerModel.Input = registerInput;

    //    // Act: Call the Register method
    //    var result = await _registerModel.OnPostAsync("/");

    //    // Assert: Verify the result
    //    var pageResult = Assert.IsType<PageResult>(result);
    //    Assert.False(_registerModel.ModelState.IsValid); // Check that the model state is invalid due to registration failure
    //}
}



