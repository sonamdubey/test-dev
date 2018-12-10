using Carwale.Entity.ES;
using Carwale.Interfaces.ES;
using Carwale.Notifications;
using Carwale.Notifications.Logs;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;

namespace Carwale.DAL.ES
{
    public class SurveyRepository : RepositoryBase, ISurveyRepository
    {
        public ESSurveyEnity GetSurveyQuestionAnswers(int campaignId)
        {
            try
            {
                using (var con = EsMySqlReadConnection)
                {
                    var multi = con.QueryMultiple("CW_GetESSurveyQuestions_v17_5_1", new { v_CampaignId = campaignId }, commandType: CommandType.StoredProcedure);
                    LogLiveSps.LogSpInGrayLog("CW_GetESSurveyQuestions_v17_5_1");
                    List<ESSurveyQuestions> questions = multi.Read<ESSurveyQuestions>().AsList();
                    List<ESSurveyOptions> allOptions = multi.Read<ESSurveyOptions>().AsList();
                    List<ESSurveyCampaign> campaignData = multi.Read<ESSurveyCampaign>().AsList();

                    ESSurveyEnity esSurvey = new ESSurveyEnity();
                    
                    Dictionary<int, ESSurveyQuestions> dictQuestions = new Dictionary<int, ESSurveyQuestions>();
                    foreach (var qsn in questions)
                    {
                        dictQuestions[qsn.Id] = qsn;
                        if (qsn.Options == null)
                            qsn.Options = new List<ESSurveyOptions>();
                    }

                    foreach (ESSurveyOptions optn in allOptions)
                    {
                        dictQuestions[optn.QuestionId].Options.Add(optn);
                    }

                    esSurvey.Questions = new List<ESSurveyQuestions>();
                    esSurvey.Questions = questions;
                    esSurvey.Campaign = new ESSurveyCampaign();
                    if (campaignData.Count > 0)
                    {
                        esSurvey.Campaign.Id = campaignData[0].Id;
                        esSurvey.Campaign.EndDate = campaignData[0].EndDate;
                        esSurvey.Campaign.GATrackingCategory = campaignData[0].GATrackingCategory;
                        esSurvey.Campaign.ShowLeadForm = Convert.ToBoolean(campaignData[0].ShowLeadForm);
                        esSurvey.Campaign.ThankYouText = campaignData[0].ThankYouText;
                    }

                    return esSurvey;
                }
            }
            catch (Exception ex)
            {
                var obj = new ExceptionHandler(ex, "DAL.GetSurveyQuestionAnswers()");
                obj.LogException();
            }
            return null;
        }

        public int SubmitSurvey(ESSurveyCustomerResponse objCustomer)
        {
            int customerId = -1;
            try
            {
                if (!String.IsNullOrEmpty(objCustomer.SurveyResponse) && objCustomer.SurveyResponse.EndsWith(","))
                    objCustomer.SurveyResponse = objCustomer.SurveyResponse.TrimEnd(',');

                var param = new DynamicParameters();
                param.Add("v_Email", objCustomer.BasicInfo != null ? objCustomer.BasicInfo.Email : null);
                param.Add("v_Mobile", objCustomer.BasicInfo != null ? objCustomer.BasicInfo.Mobile : null);
                param.Add("v_Name", objCustomer.BasicInfo != null ? objCustomer.BasicInfo.Name : null);
                param.Add("v_SurveyResponse", objCustomer.SurveyResponse);
                param.Add("v_Platform", objCustomer.Platform);
                param.Add("v_CampaignId", objCustomer.CampaignId);
                param.Add("v_CustomerId", objCustomer.CustomerId);
                param.Add("v_CityId", objCustomer.CityId);

                using (var con = EsMySqlMasterConnection)
                {
                    var response = con.Query("SaveESSurveyCustomerData_v17_5_1", param, commandType: CommandType.StoredProcedure);
                    LogLiveSps.LogSpInGrayLog("SaveESSurveyCustomerData_v17_5_1");
                    customerId = response.AsList()[0].CustomerId;           
                }
            }
            catch (Exception ex)
            {
                var obj = new ExceptionHandler(ex, "DAL.SubmitSurvey()");
                obj.LogException();                
            }
            return customerId;
        }

        public int SubmitSurveyWithFreeText(ESSurveyCustomerResponse objCustomer)
        {
            int customerId = -1;
            try
            {
                if (!String.IsNullOrEmpty(objCustomer.Answer) && objCustomer.Answer.EndsWith(","))
                    objCustomer.Answer = objCustomer.Answer.TrimEnd(',');

                var param = new DynamicParameters();
                param.Add("v_Email", objCustomer.BasicInfo != null ? objCustomer.BasicInfo.Email : null);
                param.Add("v_Mobile", objCustomer.BasicInfo != null ? objCustomer.BasicInfo.Mobile : null);
                param.Add("v_Name", objCustomer.BasicInfo != null ? objCustomer.BasicInfo.Name : null);
                param.Add("v_Platform", objCustomer.Platform);
                param.Add("v_CampaignId", objCustomer.CampaignId);
                param.Add("v_CustomerId", objCustomer.CustomerId);  
                param.Add("v_Answers", objCustomer.Answer);
                param.Add("v_Comment", objCustomer.Comment);
                param.Add("v_CityId", objCustomer.CityId);

                using (var con = EsMySqlMasterConnection)
                {
                    var response = con.Query("SaveESSurveyCustomerDataFreeText_v17_5_1", param, commandType: CommandType.StoredProcedure);
                    LogLiveSps.LogSpInGrayLog("SaveESSurveyCustomerDataFreeText_v17_5_1");
                    customerId = response.AsList()[0].CustomerId;
                }
            }
            catch (Exception ex)
            {
                var obj = new ExceptionHandler(ex, "DAL.SubmitSurveyWithFreeText()");
                obj.LogException();
            }
            return customerId;
        }
    }
}
