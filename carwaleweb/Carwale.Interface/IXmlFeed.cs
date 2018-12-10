using Carwale.Entity.XmlFeed;
using System.Collections.Generic;

namespace Carwale.Interfaces
{
    public interface IXmlFeed
    {
        List<url> GenerateXmlFeed();
        List<SociomanticProduct> GenerateSociomanticXmlFeed();
    }
}
