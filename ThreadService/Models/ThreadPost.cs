using System.ComponentModel.DataAnnotations;
using ThreadService.Models.DTO;

namespace ThreadService.Models
{
    public class ThreadPost 
    {
        public long Id {get; set;}
        public String? Message {get; set;}
        public String? ImagePath {get; set;}
        public List<Like> likes {get; set;}
        public uint UserId { get; set;}
    }
}