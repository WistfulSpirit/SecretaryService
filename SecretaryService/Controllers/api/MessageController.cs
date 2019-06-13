using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SecretaryService.Models;

namespace SecretaryService.Controllers.api
{
    public class MessageController : ApiController
    {
        private MessageRepository messageRepository = new MessageRepository();

        // GET: api/Message
        public HttpResponseMessage Get()
        {
            List<Message> messages = messageRepository.GetAllMessages();
            return Request.CreateResponse(HttpStatusCode.OK, messages);
        }

        // GET: api/Message/5
        public HttpResponseMessage Get(int id)
        {
            Message message = messageRepository.GetMessage(id);
            return Request.CreateResponse(HttpStatusCode.OK, message);

        }

        // POST: api/Message
        public HttpResponseMessage Post([FromBody]Message value)
        {
            if (String.IsNullOrWhiteSpace(value.Title) || String.IsNullOrWhiteSpace(value.Content) || value.Adressee == null || value.Sender==null)
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Данные о тэге не введены!");
            if (messageRepository.InsertMessage(value) != -1)
                return Request.CreateResponse(HttpStatusCode.OK);
            else
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Сообщение не добавлено!");
        }

        // PUT: api/Message/5
        public HttpResponseMessage Put(int id, [FromBody]Message value)
        {
            if (String.IsNullOrWhiteSpace(value.Title) || String.IsNullOrWhiteSpace(value.Content) || value.Adressee == null || value.Sender == null)
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Данные о тэге не введены!");
            if (messageRepository.UpdateMessage(id, value) == 1)
                return Request.CreateResponse(HttpStatusCode.OK);
            else
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Сообщение не обновлено!");
        }
    }
}
