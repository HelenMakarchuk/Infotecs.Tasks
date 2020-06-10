using System.Collections.Generic;
using System.Linq;

namespace Infotecs.Identity
{
    public class LoginViewModel : LoginInputModel
    {
        public IEnumerable<ExternalProvider> ExternalProviders { get; set; } = Enumerable.Empty<ExternalProvider>();
    }
}
