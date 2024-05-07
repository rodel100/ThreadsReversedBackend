using ThreadService.Models.DTO;

namespace ThreadService.Models
{
    public class Like
    {
        public long Id {get; set;}
        public ThreadPost threadpost { get; set;}
        public long userId { get; set;}
    }

}