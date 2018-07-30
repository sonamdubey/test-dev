using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Bikewale.Entities.UsersTestimonial;
using Bikewale.Interfaces.UsersTestimonial;
using Bikewale.DAL.UsersTestimonial;
using Microsoft.Practices.Unity;
using Bikewale.Notifications;

namespace Bikewale.BindViewModels.Controls
{
    /// <summary>
    /// Created by  :   Sumit Kate on 19 Jan 2016
    /// Description :   Binds UsersTestimonial User Control with Data
    /// </summary>
    public class BindUsersTestimonialControl
    {
        public uint TopCount { get; set; }
        public int FetchedCount { get; private set; }
        /// <summary>
        /// Binds the Repeater with data
        /// </summary>
        /// <param name="rpt"></param>
        public void BindRepeater(Repeater rpt)
        {
            IEnumerable<UsersTestimonial> lstUsersTestimonial = null;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IUsersTestimonial, UsersTestimonialRepository>();
                    IUsersTestimonial objRepository = container.Resolve<IUsersTestimonial>();

                    lstUsersTestimonial = objRepository.FetchUsersTestimonial(TopCount);

                    if (lstUsersTestimonial != null && lstUsersTestimonial.Any())
                    {
                        FetchedCount = lstUsersTestimonial.Count();
                        rpt.DataSource = lstUsersTestimonial;
                        rpt.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }
        }
    }
}