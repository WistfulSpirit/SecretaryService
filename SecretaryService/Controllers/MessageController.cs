using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SecretaryService.Models;

namespace SecretaryService.Controllers
{
    public class MessageController : Controller
    {
        private MessageRepository messageRepository = new MessageRepository();
        private TagRepository tagsRepository = new TagRepository();
        private PersonRepository personRepository = new PersonRepository();

        // GET: Message
        public ActionResult Index()
        {
            ViewData["VBPerson"] = new SelectList(personRepository.GetAllPersons(), "Id", "DataTextFieldLabel");
            ViewData["VBTag"] = new SelectList(tagsRepository.GetAllTags(), "Id", "Value");
            List<Message> messages = new List<Message>();
            messages = messageRepository.GetAllMessages();
            return View(messages);
        }

        // GET: Message/Details/5
        public ActionResult Details(int id)
        {
            Message message = messageRepository.GetMessage(id);
            return View(message);
        }

        // GET: Message/Create
        public ActionResult Create()
        {
            ViewBag.VBTags = new MultiSelectList(tagsRepository.GetAllTags(), "Id", "Value");
            ViewData["VBPerson"] = new SelectList(personRepository.GetAllPersons(), "Id", "DataTextFieldLabel");
            return View();
        }

        // POST: Message/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                Message message = new Message();
                string[] tagsId = collection.GetValues("Tags");
                List<Tag> tags = new List<Tag>();
                if (tagsId != null && tagsId.Count() != 0)
                {
                    foreach (string tagId in tagsId)
                    {
                        tags.Add(new Tag { Id = Int32.Parse(tagId) });
                    }
                }
                UpdateModel(message, new string[] {"Title", "Content", "Adressee", "Sender" },collection);
                message.Registry_Date = DateTime.Now;
                message.Tags = tags;
                messageRepository.InsertMessage(message);
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.VBTags = new MultiSelectList(tagsRepository.GetAllTags(), "Id", "Value");
                ViewData["VBPerson"] = new SelectList(personRepository.GetAllPersons(), "Id", "DataTextFieldLabel");
                return View();
            }
        }

        // GET: Message/Edit/5
        public ActionResult Edit(int id)
        {
            Message message = messageRepository.GetMessage(id);
            int[] selectedTags = message.Tags?.Select(t => t.Id).ToArray();
            ViewData["AdresseeId"] = message.Adressee.Id;
            ViewData["SenderId"] = message.Sender.Id;
            ViewBag.VBTags = new MultiSelectList(tagsRepository.GetAllTags(), "Id", "Value", selectedTags);
            ViewData["selected"] = selectedTags;
            ViewData["VBPerson"] = personRepository.GetAllPersons();// new SelectList(personRepository.GetAllPersons(), "Id", "DataTextFieldLabel");
            return View(message);
        }

        // POST: Message/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                Message message = new Message();
                string[] tagsId = collection.GetValues("Tags");
                List<Tag> tags = new List<Tag>();
                if (tagsId != null && tagsId.Count() != 0)
                {
                    foreach (string tagId in tagsId)
                    {
                        tags.Add(new Tag { Id = Int32.Parse(tagId) });
                    }
                }
                UpdateModel(message, new string[] { "Title", "Content", "Adressee", "Sender" }, collection);
                message.Registry_Date = DateTime.Now;
                message.Tags = tags;
                messageRepository.UpdateMessage(id, message);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Message message = messageRepository.GetMessage(id);
                int[] selectedTags = message.Tags?.Select(t => t.Id).ToArray();
                ViewData["AdresseeId"] = message.Adressee.Id;
                ViewData["SenderId"] = message.Sender.Id;
                ViewBag.VBTags = new MultiSelectList(tagsRepository.GetAllTags(), "Id", "Value", selectedTags.AsEnumerable());
                ViewData["selected"] = selectedTags;
                ViewData["VBPerson"] = personRepository.GetAllPersons();
                return View();
            }
        }

        // GET: Message/Delete/5
        public ActionResult Delete(int id)
        {
            messageRepository.DeleteMessage(id);
            return RedirectToAction("Index");
        }

        public ActionResult ShowMessagesByDate(DateTime? dateStart, DateTime? dateEnd)
        {
            List<Message> messages = new List<Message>();
            messages = messageRepository.GetMessagesInDateInterval(dateStart.Value, dateEnd.Value);
            return PartialView("TableView", messages);
        }

        public ActionResult ShowMessagesByPerson(string pRole, int PersonId)
        {
            List<Message> messages = new List<Message>();
            messages = messageRepository.GetMessagesOfPerson(pRole, PersonId);
            return PartialView("TableView", messages);
        }

        public ActionResult ShowMessagesByTag(int TagId)
        {
            List<Message> messages = new List<Message>();
            messages = messageRepository.GetMessagesByTag(TagId);
            return PartialView("TableView", messages);
        }



    }
}
