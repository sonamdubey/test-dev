using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Interfaces.Template
{
    public interface ITemplateRender
    {
        string Render<T>(string templateName, T model, string template);
        string Render<T>(int templateId, T model);
    }
}
