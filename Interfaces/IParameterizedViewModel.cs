using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiContactManager.Interfaces
{
    public interface IParameterizedViewModel
    {
        void ApplyParameters(Dictionary<string, object> parameters);
    }
}
