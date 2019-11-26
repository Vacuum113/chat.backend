using System.Collections.Generic;

namespace chat.backend.Data.Domain
{
    public class User
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public ICollection<Dialogue> Dialogues { get; set; }
    }
}
