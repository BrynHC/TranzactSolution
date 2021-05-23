using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tranzact.Entities;

namespace Tranzact.Service
{
    public interface ISearch
    {
        Result GetSearchList();
    }
}
