using Ansario.Web.API.Attributes;
using Ansario.Web.API.Models;
using Ansario.Web.API.Repositories;
using MongoDB.Driver;
using MongoDB.Driver.GeoJsonObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Ansario.Web.API.Controllers
{
    [RoutePrefix("v1/m3c")]
    public class M3CController : ApiController
    {
        IMongoDbRepository _repository;

        public M3CController(IMongoDbRepository repository)
        {
            _repository = repository;
        }

        [Route("all")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public async Task<HttpResponseMessage> Get(string longitude = null, string latitude = null, double? distance = null)
        {
            if (distance == null)
            {
                distance = 1000;
            }

            if (!string.IsNullOrWhiteSpace(longitude) && !string.IsNullOrWhiteSpace(latitude))
            {
                var index = Builders<BodyForm>.IndexKeys.Geo2DSphere("Location");
                await _repository.CreateIndex("ansario", "m3c-body", index);

                double lng = double.Parse(longitude);
                double lat = double.Parse(latitude);
                var point = new GeoJson2DGeographicCoordinates(lng, lat);
                var pnt = new GeoJsonPoint<GeoJson2DGeographicCoordinates>(point);

                var fil = Builders<BodyForm>.Filter.NearSphere(p => p.Location, pnt, distance);

                try
                {
                    var response = await _repository.Get("ansario", "m3c-body", fil);
                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }
                catch (Exception ex)
                {
                    throw new HttpResponseException(HttpStatusCode.InternalServerError);
                }
            }
            else
            {
                try
                {
                    var response = await _repository.GetAll<BodyForm>("ansario", "m3c-body");
                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }
                catch (Exception ex)
                {
                    throw new HttpResponseException(HttpStatusCode.InternalServerError);
                }
            }
           
        }

        [Route("record/{id}")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public async Task<HttpResponseMessage> GetRecord(string id)
        {
            try
            {
                var response = await _repository.Get("ansario", "m3c-body", Builders<BodyForm>.Filter.Eq("_id", id));
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        [JwtAuthentication]
        [Route("new")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public async Task Post(BodyForm value)
        {
            var auth = Request.Headers.Authorization;
            value.SubmittedBy = ((ClaimsIdentity)User.Identity).Claims.Where(x => x.Type == "id").ToList().First().Value.ToString();
            try
            {
                await _repository.Upsert("ansario", "m3c-body", x => x.Id == value.Id, value);
            }
            catch(Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }
    }
}