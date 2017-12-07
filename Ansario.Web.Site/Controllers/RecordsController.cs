using Ansario.Web.Site.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Ansario.Web.Site.Controllers
{
    public class RecordsController : Controller
    {
        public ISiteRepository _repository;

        public RecordsController(ISiteRepository repository)
        {
            _repository = repository;
        }

        // GET: Records
        public async Task<ActionResult> ViewRecords()
        {
            var records = await _repository.Records();
            return View(records);
        }

        // GET: Records
        public async Task<ActionResult> ViewMap()
        {
            var records = await _repository.Records();
            return View(records);
        }

        public async Task<ActionResult> ViewRecord(string id)
        {
            var record = await _repository.Record(id);
            return View(record);
        }
    }
}