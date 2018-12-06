using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organizer.Interfaces
{
    public interface IUserAuthData
    {
        int Id { get; }
        IEnumerable<string>Roles { get; set; }
    }
}
