﻿using System.Configuration;
namespace Bikewale.BAL.ABServiceRef
{
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "1.0.3705.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name = "TCApi_InquirySoap", Namespace = "http://tradingcars.carwale.com/wsapis/")]
    public class TCApi_Inquiry : System.Web.Services.Protocols.SoapHttpClientProtocol
    {

        /// <remarks/>
        public TCApi_Inquiry()
        {
            string host = ConfigurationManager.AppSettings["ABApiHostUrl"];
            this.Url = host + "/wsapis/TCApi_Inquiry.asmx";
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tradingcars.carwale.com/wsapis/CheckService", RequestNamespace = "http://tradingcars.carwale.com/wsapis/", ResponseNamespace = "http://tradingcars.carwale.com/wsapis/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public bool CheckService()
        {
            object[] results = this.Invoke("CheckService", new object[0]);
            return ((bool)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginCheckService(System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("CheckService", new object[0], callback, asyncState);
        }

        /// <remarks/>
        public bool EndCheckService(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((bool)(results[0]));
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tradingcars.carwale.com/wsapis/AddBuyerInquiry", RequestNamespace = "http://tradingcars.carwale.com/wsapis/", ResponseNamespace = "http://tradingcars.carwale.com/wsapis/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public bool AddBuyerInquiry(string jsonInquiryDetails)
        {
            object[] results = this.Invoke("AddBuyerInquiry", new object[] {
                    jsonInquiryDetails});
            return ((bool)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginAddBuyerInquiry(string jsonInquiryDetails, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("AddBuyerInquiry", new object[] {
                    jsonInquiryDetails}, callback, asyncState);
        }

        /// <remarks/>
        public bool EndAddBuyerInquiry(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((bool)(results[0]));
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tradingcars.carwale.com/wsapis/AddSellerInquiry", RequestNamespace = "http://tradingcars.carwale.com/wsapis/", ResponseNamespace = "http://tradingcars.carwale.com/wsapis/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string AddSellerInquiry(string branchId, string jsonInquiryDetails)
        {
            object[] results = this.Invoke("AddSellerInquiry", new object[] {
                    branchId,
                    jsonInquiryDetails});
            return ((string)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginAddSellerInquiry(string branchId, string jsonInquiryDetails, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("AddSellerInquiry", new object[] {
                    branchId,
                    jsonInquiryDetails}, callback, asyncState);
        }

        /// <remarks/>
        public string EndAddSellerInquiry(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tradingcars.carwale.com/wsapis/AddNewCarInquiry", RequestNamespace = "http://tradingcars.carwale.com/wsapis/", ResponseNamespace = "http://tradingcars.carwale.com/wsapis/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string AddNewCarInquiry(string branchId, string jsonInquiryDetails)
        {
            object[] results = this.Invoke("AddNewCarInquiry", new object[] {
                    branchId,
                    jsonInquiryDetails});
            return ((string)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginAddNewCarInquiry(string branchId, string jsonInquiryDetails, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("AddNewCarInquiry", new object[] {
                    branchId,
                    jsonInquiryDetails}, callback, asyncState);
        }

        /// <remarks/>
        public string EndAddNewCarInquiry(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tradingcars.carwale.com/wsapis/AddOtherInquiries", RequestNamespace = "http://tradingcars.carwale.com/wsapis/", ResponseNamespace = "http://tradingcars.carwale.com/wsapis/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public bool AddOtherInquiries(string branchId, string jsonInquiryDetails)
        {
            object[] results = this.Invoke("AddOtherInquiries", new object[] {
                    branchId,
                    jsonInquiryDetails});
            return ((bool)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginAddOtherInquiries(string branchId, string jsonInquiryDetails, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("AddOtherInquiries", new object[] {
                    branchId,
                    jsonInquiryDetails}, callback, asyncState);
        }

        /// <remarks/>
        public bool EndAddOtherInquiries(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((bool)(results[0]));
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tradingcars.carwale.com/wsapis/FetchIpAddress", RequestNamespace = "http://tradingcars.carwale.com/wsapis/", ResponseNamespace = "http://tradingcars.carwale.com/wsapis/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string FetchIpAddress(string input)
        {
            object[] results = this.Invoke("FetchIpAddress", new object[] {
                    input});
            return ((string)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginFetchIpAddress(string input, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("FetchIpAddress", new object[] {
                    input}, callback, asyncState);
        }

        /// <remarks/>
        public string EndFetchIpAddress(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tradingcars.carwale.com/wsapis/InsertBuyerInquiry", RequestNamespace = "http://tradingcars.carwale.com/wsapis/", ResponseNamespace = "http://tradingcars.carwale.com/wsapis/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string InsertBuyerInquiry(string jsonInquiryDetails)
        {
            object[] results = this.Invoke("InsertBuyerInquiry", new object[] {
                    jsonInquiryDetails});
            return ((string)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginInsertBuyerInquiry(string jsonInquiryDetails, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("InsertBuyerInquiry", new object[] {
                    jsonInquiryDetails}, callback, asyncState);
        }

        /// <remarks/>
        public string EndInsertBuyerInquiry(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tradingcars.carwale.com/wsapis/InsertSellerInquiry", RequestNamespace = "http://tradingcars.carwale.com/wsapis/", ResponseNamespace = "http://tradingcars.carwale.com/wsapis/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string InsertSellerInquiry(string branchId, string jsonInquiryDetails)
        {
            object[] results = this.Invoke("InsertSellerInquiry", new object[] {
                    branchId,
                    jsonInquiryDetails});
            return ((string)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginInsertSellerInquiry(string branchId, string jsonInquiryDetails, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("InsertSellerInquiry", new object[] {
                    branchId,
                    jsonInquiryDetails}, callback, asyncState);
        }

        /// <remarks/>
        public string EndInsertSellerInquiry(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tradingcars.carwale.com/wsapis/InsertNewCarInquiry", RequestNamespace = "http://tradingcars.carwale.com/wsapis/", ResponseNamespace = "http://tradingcars.carwale.com/wsapis/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string InsertNewCarInquiry(string branchId, string jsonInquiryDetails)
        {
            object[] results = this.Invoke("InsertNewCarInquiry", new object[] {
                    branchId,
                    jsonInquiryDetails});
            return ((string)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginInsertNewCarInquiry(string branchId, string jsonInquiryDetails, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("InsertNewCarInquiry", new object[] {
                    branchId,
                    jsonInquiryDetails}, callback, asyncState);
        }

        /// <remarks/>
        public string EndInsertNewCarInquiry(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tradingcars.carwale.com/wsapis/InsertOtherInquiries", RequestNamespace = "http://tradingcars.carwale.com/wsapis/", ResponseNamespace = "http://tradingcars.carwale.com/wsapis/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string InsertOtherInquiries(string branchId, string jsonInquiryDetails)
        {
            object[] results = this.Invoke("InsertOtherInquiries", new object[] {
                    branchId,
                    jsonInquiryDetails});
            return ((string)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginInsertOtherInquiries(string branchId, string jsonInquiryDetails, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("InsertOtherInquiries", new object[] {
                    branchId,
                    jsonInquiryDetails}, callback, asyncState);
        }

        /// <remarks/>
        public string EndInsertOtherInquiries(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tradingcars.carwale.com/wsapis/GetNewCarMakes", RequestNamespace = "http://tradingcars.carwale.com/wsapis/", ResponseNamespace = "http://tradingcars.carwale.com/wsapis/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string GetNewCarMakes(string key)
        {
            object[] results = this.Invoke("GetNewCarMakes", new object[] {
                    key});
            return ((string)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginGetNewCarMakes(string key, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("GetNewCarMakes", new object[] {
                    key}, callback, asyncState);
        }

        /// <remarks/>
        public string EndGetNewCarMakes(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tradingcars.carwale.com/wsapis/GetNewCarModel", RequestNamespace = "http://tradingcars.carwale.com/wsapis/", ResponseNamespace = "http://tradingcars.carwale.com/wsapis/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string GetNewCarModel(string key, string makeId)
        {
            object[] results = this.Invoke("GetNewCarModel", new object[] {
                    key,
                    makeId});
            return ((string)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginGetNewCarModel(string key, string makeId, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("GetNewCarModel", new object[] {
                    key,
                    makeId}, callback, asyncState);
        }

        /// <remarks/>
        public string EndGetNewCarModel(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tradingcars.carwale.com/wsapis/GetCarVersion", RequestNamespace = "http://tradingcars.carwale.com/wsapis/", ResponseNamespace = "http://tradingcars.carwale.com/wsapis/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string GetCarVersion(string key, string modelId)
        {
            object[] results = this.Invoke("GetCarVersion", new object[] {
                    key,
                    modelId});
            return ((string)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginGetCarVersion(string key, string modelId, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("GetCarVersion", new object[] {
                    key,
                    modelId}, callback, asyncState);
        }

        /// <remarks/>
        public string EndGetCarVersion(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tradingcars.carwale.com/wsapis/GetInquirySource", RequestNamespace = "http://tradingcars.carwale.com/wsapis/", ResponseNamespace = "http://tradingcars.carwale.com/wsapis/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string GetInquirySource(string key)
        {
            object[] results = this.Invoke("GetInquirySource", new object[] {
                    key});
            return ((string)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginGetInquirySource(string key, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("GetInquirySource", new object[] {
                    key}, callback, asyncState);
        }

        /// <remarks/>
        public string EndGetInquirySource(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tradingcars.carwale.com/wsapis/GetDealerCities", RequestNamespace = "http://tradingcars.carwale.com/wsapis/", ResponseNamespace = "http://tradingcars.carwale.com/wsapis/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string GetDealerCities(string key)
        {
            object[] results = this.Invoke("GetDealerCities", new object[] {
                    key});
            return ((string)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginGetDealerCities(string key, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("GetDealerCities", new object[] {
                    key}, callback, asyncState);
        }

        /// <remarks/>
        public string EndGetDealerCities(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tradingcars.carwale.com/wsapis/GetTaskListDetails", RequestNamespace = "http://tradingcars.carwale.com/wsapis/", ResponseNamespace = "http://tradingcars.carwale.com/wsapis/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string GetTaskListDetails(string key, string inputJson)
        {
            object[] results = this.Invoke("GetTaskListDetails", new object[] {
                    key,
                    inputJson});
            return ((string)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginGetTaskListDetails(string key, string inputJson, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("GetTaskListDetails", new object[] {
                    key,
                    inputJson}, callback, asyncState);
        }

        /// <remarks/>
        public string EndGetTaskListDetails(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tradingcars.carwale.com/wsapis/ChangeInquiryDisposition", RequestNamespace = "http://tradingcars.carwale.com/wsapis/", ResponseNamespace = "http://tradingcars.carwale.com/wsapis/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string ChangeInquiryDisposition(string key, string inputJson)
        {
            object[] results = this.Invoke("ChangeInquiryDisposition", new object[] {
                    key,
                    inputJson});
            return ((string)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginChangeInquiryDisposition(string key, string inputJson, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("ChangeInquiryDisposition", new object[] {
                    key,
                    inputJson}, callback, asyncState);
        }

        /// <remarks/>
        public string EndChangeInquiryDisposition(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tradingcars.carwale.com/wsapis/InquiriesFollowUp", RequestNamespace = "http://tradingcars.carwale.com/wsapis/", ResponseNamespace = "http://tradingcars.carwale.com/wsapis/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string InquiriesFollowUp(string key, string inputJson)
        {
            object[] results = this.Invoke("InquiriesFollowUp", new object[] {
                    key,
                    inputJson});
            return ((string)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginInquiriesFollowUp(string key, string inputJson, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("InquiriesFollowUp", new object[] {
                    key,
                    inputJson}, callback, asyncState);
        }

        /// <remarks/>
        public string EndInquiriesFollowUp(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tradingcars.carwale.com/wsapis/UpdateCustomerDetails", RequestNamespace = "http://tradingcars.carwale.com/wsapis/", ResponseNamespace = "http://tradingcars.carwale.com/wsapis/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string UpdateCustomerDetails(string key, string inputJson)
        {
            object[] results = this.Invoke("UpdateCustomerDetails", new object[] {
                    key,
                    inputJson});
            return ((string)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginUpdateCustomerDetails(string key, string inputJson, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("UpdateCustomerDetails", new object[] {
                    key,
                    inputJson}, callback, asyncState);
        }

        /// <remarks/>
        public string EndUpdateCustomerDetails(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tradingcars.carwale.com/wsapis/LeadFollowupLoad", RequestNamespace = "http://tradingcars.carwale.com/wsapis/", ResponseNamespace = "http://tradingcars.carwale.com/wsapis/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string LeadFollowupLoad(string key, string leadId)
        {
            object[] results = this.Invoke("LeadFollowupLoad", new object[] {
                    key,
                    leadId});
            return ((string)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginLeadFollowupLoad(string key, string leadId, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("LeadFollowupLoad", new object[] {
                    key,
                    leadId}, callback, asyncState);
        }

        /// <remarks/>
        public string EndLeadFollowupLoad(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tradingcars.carwale.com/wsapis/FetchLeadDispositions", RequestNamespace = "http://tradingcars.carwale.com/wsapis/", ResponseNamespace = "http://tradingcars.carwale.com/wsapis/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string FetchLeadDispositions(string key)
        {
            object[] results = this.Invoke("FetchLeadDispositions", new object[] {
                    key});
            return ((string)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginFetchLeadDispositions(string key, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("FetchLeadDispositions", new object[] {
                    key}, callback, asyncState);
        }

        /// <remarks/>
        public string EndFetchLeadDispositions(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tradingcars.carwale.com/wsapis/ChangeEagerness", RequestNamespace = "http://tradingcars.carwale.com/wsapis/", ResponseNamespace = "http://tradingcars.carwale.com/wsapis/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string ChangeEagerness(string key, string inputJson)
        {
            object[] results = this.Invoke("ChangeEagerness", new object[] {
                    key,
                    inputJson});
            return ((string)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginChangeEagerness(string key, string inputJson, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("ChangeEagerness", new object[] {
                    key,
                    inputJson}, callback, asyncState);
        }

        /// <remarks/>
        public string EndChangeEagerness(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tradingcars.carwale.com/wsapis/SaveOtherRequests", RequestNamespace = "http://tradingcars.carwale.com/wsapis/", ResponseNamespace = "http://tradingcars.carwale.com/wsapis/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string SaveOtherRequests(string inputJson)
        {
            object[] results = this.Invoke("SaveOtherRequests", new object[] {
                    inputJson});
            return ((string)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginSaveOtherRequests(string inputJson, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("SaveOtherRequests", new object[] {
                    inputJson}, callback, asyncState);
        }

        /// <remarks/>
        public string EndSaveOtherRequests(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }
    }
}
