using System.Collections.Generic;

namespace chat.backend.Data.Domain
{
    class Dialogue
    {
        public ICollection<User> Users { get; set; }
    }
}
