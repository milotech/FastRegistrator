using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastRegistrator.ApplicationCore.Domain
{
    public abstract class BaseEntity
    {
        public int Id { get; protected set; }
    }
}
