﻿using Bikewale.Interfaces.BikeBooking;
using Bikewale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Configuration;
using System.Web;


namespace Bikewale.BikeBooking
{
    public class RedirectToBillDesk : System.Web.UI.Page
    {
        protected string msg = string.Empty;
        public int submit = 0;

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["msg"] != null && Request.QueryString["msg"] != "")
            {
                msg = Carwale.Utility.CarwaleSecurity.Decrypt(Request.QueryString["msg"].ToString());
                submit = 1;
            }
            Trace.Warn("msg : " + msg);
            UpdatePGTranscationId(msg);
        }

        private static void UpdatePGTranscationId(string msg)
        {
            string[] bwTranParameters = null;
            string pqId = string.Empty, pgRecordId = string.Empty;
            bool isUpdated = false;

            try
            {
                bwTranParameters = msg.Split('|');
                string MPQ = HttpUtility.ParseQueryString(bwTranParameters[21].Split('?')[1]).Get("MPQ");
                pqId = HttpUtility.ParseQueryString(EncodingDecodingHelper.DecodeFrom64(MPQ)).Get("PQId");
                pgRecordId = bwTranParameters[1].Replace(BWConfiguration.Instance.OfferUniqueTransaction, "");

                using (IUnityContainer containerTran = new UnityContainer())
                {
                    containerTran.RegisterType<IDealerPriceQuote, Bikewale.BAL.BikeBooking.DealerPriceQuote>();
                    IDealerPriceQuote objDealer = containerTran.Resolve<IDealerPriceQuote>();
                    isUpdated = objDealer.UpdatePQTransactionalDetail(Convert.ToUInt32(pqId), Convert.ToUInt32(pgRecordId),
                                    false, ConfigurationManager.AppSettings["OfferUniqueTransaction"]);
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "Bikewale.BikeBooking.RedirectToBillDesk.UpdatePGTranscationId");
                objErr.SendMail();
            }
        }
    }
}
