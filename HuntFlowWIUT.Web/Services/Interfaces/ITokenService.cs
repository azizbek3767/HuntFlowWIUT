namespace HuntFlowWIUT.Web.Services.Interfaces
{
    public interface ITokenService
    {
        // refresh
        Task<string> GetAccessTokenAsync();
        Task RefreshTokenAsync();
    }
}
