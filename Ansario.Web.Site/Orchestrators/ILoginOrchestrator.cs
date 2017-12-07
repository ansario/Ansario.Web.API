using Ansario.Web.Site.Models;
using System.Threading.Tasks;

namespace Ansario.Web.Site.Orchestrators
{
    public interface ILoginOrchestrator
    {
        Task<bool> Login(LoginModel model);
    }
}