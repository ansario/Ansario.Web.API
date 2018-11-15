using Ansario.Web.API.Models;
using Ansario.Web.API.Repositories;
using Ansario.Web.API.Services;
using MongoDB.Driver;
using System;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Ansario.Web.API.Controllers
{
    [RoutePrefix("v1/twilio")]
    public class TwilioController : ApiController
    {
        private readonly IToneAnalyzerService _toneAnalyzerService;
        private readonly IMongoDbRepository _repository;

        public TwilioController(IToneAnalyzerService toneAnalyzerService, IMongoDbRepository repository)
        {
            _toneAnalyzerService = toneAnalyzerService;
            _repository = repository;
        }

        [Route("")]
        [HttpGet]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public async Task<IHttpActionResult> Get()
        {
            try
            {
                var documents = await _repository.GetAll<AnalyzedDocument>("ansario", "christmas");
                var document = documents.OrderByDescending(x => x.Time).First();
                return Json(document);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("")]
        [HttpPost]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public async Task<IHttpActionResult> Post(FormDataCollection formData)
        {
            try
            {
                var fromNumber = formData["From"];
                var body = formData["Body"];

                var tone = _toneAnalyzerService.AnalyzeText(body);

                await _repository.Post("ansario", "christmas", new AnalyzedDocument()
                {
                    From = fromNumber,
                    Body = body,
                    Tones = tone,
                    Time = DateTime.UtcNow
                });


                return Ok();
            }
            catch(Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
