using MaratukAdmin.Dto.Response.Sansejour;
using MaratukAdmin.Models.Requests;

namespace MaratukAdmin.Managers.Abstract.Sansejour
{
    public interface IHttpRequestManager
    {
        Task<string> LoginAsync();
        //string Login();
        Task<bool> CheckTokenAsync(string token);
        Task<HttpResponseMessage> SendAsync(string baseAddress, string reqUrl, HttpRequestMessage request);
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage request);
        Task<List<HotelSansejourResponse>> GetAllHotelsSansejourAsync();
        Task<SyncSejourContractExportViewResponse> GetSejourContractExportViewAsync(GetSejourContractExportViewRequestModel reqModel);
        

    }
}
