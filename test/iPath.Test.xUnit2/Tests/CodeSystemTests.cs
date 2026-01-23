using iPath.Application.Coding;
using iPath.Domain.Config;
using Microsoft.Extensions.Options;

namespace iPath.Test.xUnit2.Tests;

[TestCaseOrderer(ordererTypeName: "iPath.Test.xUnit2.TestPriorityOrderer", ordererAssemblyName: "iPath.Test.xUnit2")]
[Collection("icdo")]
public class CodeSystemTests : IClassFixture<iPathFixture>
{
    private readonly iPathFixture _fixture;
    private readonly IOptions<iPathConfig> _opts;
    private static CodingService _coding;

    public CodeSystemTests(iPathFixture fixture)
    {
        _fixture = fixture;
        _opts = _fixture.ServiceProvider.GetService(typeof(IOptions<iPathConfig>)) as IOptions<iPathConfig>;
    }



    #region "-- Init --"
    [Fact, TestPriority(1)]
    public async Task LoadICDO()
    {
        var cd = System.IO.Directory.GetCurrentDirectory();
        var di = new System.IO.DirectoryInfo(_opts.Value.FhirResourceFilePath);

        _coding = new CodingService(_fixture.ServiceProvider, "icdo");
        await _coding.LoadCodeSystem();
        _coding.CodeSystem.Should().NotBeNull("CodeSystem icdo not loaded");
        _coding.CodeSystemUrl.Should().NotBeNullOrEmpty();
    }
    #endregion



    #region "-- Topo --"


    [Fact, TestPriority(11)]
    public async Task TopoTree()
    {
        // Test T codes
        var resT = _coding.GetChildCodes("T");
        resT.IsSuccess.Should().BeTrue();
        resT.ChildCodes.Should().NotBeNull();
        resT.ChildCodes.Capacity.Should().BeGreaterThan(100);
    }


    [Fact, TestPriority(12)]
    public async Task TopoValueSet()
    {
        // Load Topo ValueSet
        await _coding.LoadValueSet("icdo-topo");
        var vs = _coding.GetValueSetDisplay("icdo-topo");
        vs.Should().NotBeNull("ValueSet icdo-topo not loaded");
        vs.ValueSet.Should().NotBeNull();

        // Test T codes
        var resT = _coding.GetChildCodes("T", "icdo-topo", false);
        resT.IsSuccess.Should().BeTrue();
        resT.ChildCodes.Should().NotBeNull();
        resT.ChildCodes.Capacity.Should().BeGreaterThan(100);

        var c100 = resT.ChildCodes.ToList()[99];
        var c1 = _coding.IsChildCode(c100, "T");
        c1.Should().BeTrue();
    }


    [Fact, TestPriority(13)]
    public async Task TopoValueSetKnochen()
    {
        // Load Topo ValueSet
        await _coding.LoadValueSet("icdo-topo-knochen");
        var vs = _coding.GetValueSetDisplay("icdo-topo-knochen");
        vs.Should().NotBeNull("ValueSet icdo-topo-knochen not loaded");
        vs.ValueSet.Should().NotBeNull();

        // Test T codes
        var resT = _coding.GetChildCodes("T", "icdo-topo-knochen", false);
        resT.IsSuccess.Should().BeTrue();
        resT.ChildCodes.Should().NotBeNull();
        resT.ChildCodes.Capacity.Should().BeLessThan(100);
    }
    #endregion
}
