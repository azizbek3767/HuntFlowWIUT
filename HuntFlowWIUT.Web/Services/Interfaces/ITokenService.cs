namespace HuntFlowWIUT.Web.Services.Interfaces
{
    public interface ITokenService
    {
        Task<string> GetAccessTokenAsync();

        Task RefreshTokenAsync();
    }
}
