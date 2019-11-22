using System;
using System.Collections.Generic;
using System.Text;

namespace chat.backend.Data.Interfaces
{
    interface IRepository<T> : IDisposable 
        where T : class
    {
    }
}
