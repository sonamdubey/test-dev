using Carwale.Interfaces.Template;
using Carwale.Notifications;
using RazorEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.BL.Template
{
    public class TemplateRender : ITemplateRender
    {
        private ITemplatesCacheRepository _templatesCacheRepo;

        public TemplateRender(ITemplatesCacheRepository templatesCacheRepo)
        {
            _templatesCacheRepo = templatesCacheRepo;
        }

        public string Render<T>(string templateName, T model, string template)
        {
            // loading a template might be expensive, so be careful to cache content
            try
            {
                if (Razor.Resolve(templateName) == null)
                {
                    // we've never seen this template before, so compile it and stick it in cache.                
                    Razor.Compile(template, typeof(T), templateName);
                }

                // by now, we know we've got a the template cached and ready to run; this is fast
               return Razor.Run(templateName, model);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "TemplateRender.Render<T>()" + "Template Name:" + templateName);
                objErr.SendMail();
            }
            return string.Empty;
        }

        public string Render<T>(int templateId, T model)
        {
            string templateData = null;
            var template = _templatesCacheRepo.GetById(templateId);

            if (template != null && model != null)
            {
                templateData = Render(template.UniqueName, model, template.Html);
            }
            return templateData;
        }
    }
}
