using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Persistence.Interface.Repository;

namespace Persistence.Interface
{
    public interface IDataManager : IDisposable
    {
        ITaggingRepository GetTEMPLATERepository();
    }
}
