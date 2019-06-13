using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SecretaryService.Models
{
    public class Message
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Название")]
        public string Title { get; set; }
        [Display(Name = "Дата")]
        public System.DateTime Registry_Date { get; set; }
        [Display(Name = "Адресат")]
        public Person Adressee { get; set; }
        [Display(Name = "Отправитель")]
        public Person Sender { get; set; }
        [Required]
        [Display(Name = "Содержание")]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }
        public IList<Tag> Tags { get; set; } 
    }
}