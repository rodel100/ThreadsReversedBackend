using ThreadService.Models.DTO;

namespace ThreadService.Models
{
    public class Reply
    {
        
        public long Id {get; set;}
        public String? Message {get; set;}
        List<Like>? likes {get; set;}
        public uint? UserId { get; set; }
    }
}