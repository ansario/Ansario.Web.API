using Ansario.Web.API.Models;
using System.Net;
using System.Threading.Tasks;

namespace Ansario.Web.API.Orchestrators
{
    public interface IAuthenticationOrchestrator
    {
        Task<string> RegisterUser(RegisterUser user);
        Task<string> LoginUser(LoginUser user);
    }
}