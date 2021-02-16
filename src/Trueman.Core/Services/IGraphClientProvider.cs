using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trueman.Core.Services
{
    public interface IGraphClientProvider
    {
        GraphServiceClient CreateGraphServiceClient();
    }
}
