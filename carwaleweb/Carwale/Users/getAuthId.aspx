<%@ Page Language="C#" Trace="false" %>
<%@ Import Namespace="Carwale.UI.Common"%>
<%@ Import Namespace="Carwale.Entity"%>
<%@ Import Namespace="Carwale.Interfaces"%>
<%@ Import Namespace="Carwale.Service"%>

<% 
    if (!string.IsNullOrEmpty(Request.QueryString["authid"]))
    {
        ICustomerRepository<Customer, CustomerOnRegister> customerRepo = UnityBootstrapper.Resolve<ICustomerRepository<Customer, CustomerOnRegister>>();
        Customer customer = customerRepo.GetCustomerFromSecurityKey(Request.QueryString["authid"]);
        if (customer != null)
        {
            CookiesCustomers.CustomerName = customer.Name;
            CookiesCustomers.Mobile = customer.Mobile;
            CookiesCustomers.Email = (!String.IsNullOrEmpty(customer.Email) && !customer.Email.Contains("@unknown.com")) ? customer.Email : null;

            if (!string.IsNullOrEmpty(Request.QueryString["login"]) && Request.QueryString["login"].ToLower() == "true")
            {
                CurrentUser.EndSession();
                CurrentUser.StartSession(customer.Name, customer.CustomerId, customer.Email, customer.IsEmailVerified);
            }
        }
        NameValueCollection nvc = Request.QueryString;

        string endPart = string.Empty;

        foreach (string key in nvc.Keys)
        {
            if (!(key == "authid" || key == "returl"))
            {
                endPart += key + "=" + nvc[key] + "&";
            }
        }

        if (!string.IsNullOrEmpty(Request.QueryString["returl"]))
        {
            string retUrl = Server.UrlDecode(Request.QueryString["returl"]);
            string qStr = (retUrl.Contains("?") ? "&" : "?") + endPart.TrimEnd('&');
            Response.Redirect(retUrl + qStr);
        }
    }


%>

