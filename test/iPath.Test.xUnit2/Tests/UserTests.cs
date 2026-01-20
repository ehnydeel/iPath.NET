using Hl7.FhirPath.Sprache;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace iPath.Test.xUnit2.Tests;

[TestCaseOrderer(ordererTypeName: "iPath.Test.xUnit2.TestPriorityOrderer", ordererAssemblyName: "iPath.Test.xUnit2")]
[Collection("ipath")]
public class UserTests : IClassFixture<iPathFixture>
{
    private readonly iPathFixture _fixture;
    private readonly IMediator _mediator;
    private readonly UserManager<User> _userManager;
    private readonly IUserStore<User> _userStore;

    public UserTests(iPathFixture fixture)
    {
        _fixture = fixture;
        _mediator = _fixture.ServiceProvider.GetService(typeof(IMediator)) as IMediator;
        _userManager = _fixture.ServiceProvider.GetService(typeof(UserManager<User>)) as UserManager<User>;
        _userStore = _fixture.ServiceProvider.GetService(typeof(IUserStore<User>)) as IUserStore<User>;
    }

    #region "-- Init --"
    [Fact, TestPriority(1)]
    public async Task CreateDb()
    {
        /* 
         * not working yet with Sqlite
         * 
        var ctx = _fixture.ServiceProvider.GetService(typeof(iPathDbContext)) as iPathDbContext;

        await ctx.Database.EnsureCreatedAsync();
        await ctx.Database.MigrateAsync();

        var c = await ctx.Groups.CountAsync();
        Assert.Equal(0, c);
        */
    }


    [Fact, TestPriority(2)]
    public async Task InitializeData()
    {
    }
    #endregion


    static int _userId1;

    [Fact, TestPriority(10)]
    public async Task CreateUserCommand_Test()
    {
        /*
        _userManager.Should().NotBeNull();
        _userStore.Should().NotBeNull();

        var user = Activator.CreateInstance<User>();
        user.IsActive = true;
        await _userStore.SetUserNameAsync(user, "Admin", CancellationToken.None);

        var emailStore = (IUserEmailStore<User>)_userStore;
        await emailStore.SetEmailAsync(user, "admin@test.com", CancellationToken.None);

        var result = await _userManager.CreateAsync(user);
        result.Succeeded.Should().BeTrue();

        // uniques email test
        var user2 = new User { Id = Guid.CreateVersion7(), UserName = "Admin"};
        var res2 = await _userManager.CreateAsync(user);
        res2.Succeeded.Should().BeFalse();
        */
    }
}
