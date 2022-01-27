using System.Threading.Tasks;
using ViesService;
using WireMock.Server;
using WireMock.Settings;
using Xunit;

namespace WireMockWithVies
{
    public class WireMockWithViesTest
    {
        [Fact]
        public async Task WireMockWithVies_ShouldReturnFalse()
        {
            var wireMockServerSettings = new WireMockServerSettings()
            {
                StartAdminInterface = true,
                ProxyAndRecordSettings = new ProxyAndRecordSettings
                {
                    Url = "http://ec.europa.eu",
                    SaveMapping = true,
                    SaveMappingToFile = true,
                    AllowAutoRedirect = true
                }
            };

            var wireMockServer = WireMockServer.Start(wireMockServerSettings);

            checkVatPortTypeClient client = new checkVatPortTypeClient(checkVatPortTypeClient.EndpointConfiguration.checkVatPort, $"{wireMockServer.Urls[0]}/taxation_customs/vies/services/checkVatService");
            checkVatRequest request = new("GR", "123456789");
            var exception = await Record.ExceptionAsync(() => client.checkVatAsync(request));

            wireMockServer.Stop();

            Assert.Equal("INVALID_INPUT", exception.Message);
        }
    }
}