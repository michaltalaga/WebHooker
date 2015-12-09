using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebHookerClient
{
    public interface IRequestForwarder
    {
        void Forward(Request data);
    }
}
