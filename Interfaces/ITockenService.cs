using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SampleAPI.Models;

namespace SampleAPI.Interfaces
{
    public interface ITockenService
    {
        string CreateTocken(AppUser user);
    }
}