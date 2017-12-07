using Ansario.Web.Site.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ansario.Web.Site.Repositories
{
    public interface ISiteRepository
    {
        Task<string> Login(string username, string password);
        Task<List<BodyRecordModel>> Records(string latitude = null, string longitude = null, double? distance = null);
        Task<List<BodyRecordModel>> Record(string id);
    }
}