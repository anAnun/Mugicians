using Organizer.Models;
using Organizer.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace Organizer.Controllers
{
    public class OrganizerController : ApiController
    {
        readonly IOrganizerService organizerService; // this was pre-unity: = new OrganizerService();
        public OrganizerController(IOrganizerService organizerService)
        {
            //store the parameter "carsService' INTO THE FIELD "CARSsERVICE"
            this.organizerService = organizerService;
        }
        [Route("api/songs"), HttpGet]
        public HttpResponseMessage GetAll()
        {
            var results = organizerService.GetAll();

            return Request.CreateResponse(HttpStatusCode.OK, results);
        }

        [Route("api/songs/{id:int}"), HttpGet()]
        public HttpResponseMessage GetById(int Id)
        {
            var result = organizerService.GetById(Id);
            //OrganizerModel organizerModel = organizerService.GetById(id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("api/songs"), HttpPost]
        public HttpResponseMessage Create(OrganizerCreateModel model)
        {
            if (model == null)
            {
                ModelState.AddModelError("", "You did not send any data!");
            }
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(
                    HttpStatusCode.BadRequest,
                    ModelState
                    );
            }
            IOrganizerService organizerService = new OrganizerService();
            int id = organizerService.Create(model);
            return Request.CreateResponse(HttpStatusCode.Created, id);
        }

        [Route("api/song/{id:int}"), HttpPut]
        public HttpResponseMessage Put(int Id, OrganizerUpdateModel req)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            organizerService.Update(req);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [Route("api/song/{id:int}"), HttpDelete]
        public HttpResponseMessage Delete(int Id)
        {
            organizerService.Delete(Id);
            return Request.CreateResponse(HttpStatusCode.OK);

        }

        [Route("api/songfile/{id:int}"), HttpDelete]
        public HttpResponseMessage DeleteFile(int Id)
        {
            organizerService.DeleteFile(Id);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [Route("api/songfile"), HttpGet]
        public IHttpActionResult GetFileForPlayback(string filename)
        {
            var result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StreamContent(File.OpenRead(filename))
            };

            result.Content.Headers.ContentType = new MediaTypeHeaderValue("audio/mpeg");
            result.Headers.AcceptRanges.Add("bytes");
            return ResponseMessage(result);
        }

        [Route("api/song/{songId}/files"), HttpGet]
        public HttpResponseMessage GetAllFiles(int songId)
        {
            var results = organizerService.GetAllFiles(songId);

            return Request.CreateResponse(HttpStatusCode.OK, results);
        }

        [Route("api/songfile/{id:int}"), HttpPut]
        public HttpResponseMessage PutSongFile(int Id, OrganizerUpdateFileModel req)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            organizerService.UpdateFile(req);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [Route("api/songfile/{songId}"), HttpPost]
        public async Task<HttpResponseMessage> CreateAudioFile(int songId)
        {
            var filesReadToProvider = await Request.Content.ReadAsMultipartAsync();

            //var audioFilename = @"C:\Users\allan\Documents\musicLibrary\" + Guid.NewGuid().ToString("N") +".wav";

            var stream = filesReadToProvider.Contents.First();
            var fileBytes = await stream.ReadAsByteArrayAsync();
            var ext = "";                
            var contentType = stream.Headers.ContentType.MediaType;
            if (contentType == "audio/mp3")
            {
                ext = ".mp3";
            }
            else if (contentType == "audio/wav")
            {
                ext = ".wav";
            }
            else if (contentType == "audio/x-m4a")
            {
                ext = ".m4a";
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "unsupported file type");
            }               
            var audioFileName = @"C:\Users\allan\Documents\musicLibrary\" + Guid.NewGuid().ToString("N") + ext;
            File.WriteAllBytes(audioFileName, fileBytes);
            IOrganizerService organizerService = new OrganizerService();
            int id = organizerService.CreateAudioFile(songId, audioFileName);
            return Request.CreateResponse(HttpStatusCode.Created, id);
        }


        [Route("api/songfile/drive/{fileString}"), HttpDelete]
        public HttpResponseMessage DeleteFromDrive(string fileString)
        {
            if (File.Exists(@fileString))
            {
                File.Delete(@fileString);
            }
            
            else if (!File.Exists(@fileString))
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "NOT OK");
            }
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        //[Route("api/songFile"), HttpPut]
        //public async Task<HttpResponseMessage> Create()
        //{
        //    if (!Request.Content.IsMimeMultipartContent())
        //    {
        //        return new HttpResponseMessage(HttpStatusCode.BadRequest);
        //    }

        //    var filesReadToProvider = await Request.Content.ReadAsMultipartAsync();

        //    foreach (var stream in filesReadToProvider.Contents)
        //    {
        //        var fileBytes = await stream.ReadAsByteArrayAsync();
        //        File.WriteAllBytes(@"c:\testFolderForS3\audioe", fileBytes);
        //    }
        //    return new HttpResponseMessage(HttpStatusCode.Created);
        //}
    }
}
