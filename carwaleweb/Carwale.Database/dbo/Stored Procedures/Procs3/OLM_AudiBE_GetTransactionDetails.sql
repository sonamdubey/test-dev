IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[OLM_AudiBE_GetTransactionDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[OLM_AudiBE_GetTransactionDetails]
GO

	-- =============================================  
-- Author:  Vaibhav Kale  
-- Create date: 26-July-2013  
-- Description: To get the complete details of the Audi Booking Transaction over the TransactionId Passed  
-- Modified By : Ashish G. Kamble on 26 July 2013
-- Description : Added column ChequeStateId in select query 
-- Modified By : Ashish Ambokar on 30 July 2013
-- Description : Added column MainModelColorId,ModelColorId 
-- Modified By : Vaibhav K 5-8-2013
-- Description : Joint with the version prices table for VersionPriceId
--			   : Also get the model id for the version as selected by user
--             : if model changes afterwards then check modelId with versionModelId
--			   : Vaibhav K 10-8-2013 UpholestryColorId also added
--			   : Supriya K 10-8-2013 MainUpholestryColorId,ZipCode,DealerMobileNumber,DealerEmailID also added
--			   : Vaibhav K 11-8-2013 ResponseCode, ResponseMsg, EPGTransactionId, AuthId, ProcessCompleted, TransactionCompleted
--			   : Vaibhav K 14-8-2013 Transmission
-- =============================================  
CREATE PROCEDURE [dbo].[OLM_AudiBE_GetTransactionDetails]  
 -- Add the parameters for the stored procedure here  
 @TransactionId   NUMERIC  
   
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
  
    -- Insert statements for procedure here  
 IF @TransactionId <> -1  
  BEGIN  
   SELECT ABT.Id AS TransactionId, ABT.ModelId, ABMO.Name AS ModelName, ABVM.Id AS VersionModelId, ABMO.MainModelColorId,ABMO.MainUpholestryColorId, ABT.VersionId, ABV.Name AS VersionName, ABT.FuelTypeId,  
   CASE ABT.FuelTypeId WHEN 1 THEN 'Diesel' WHEN 2 THEN 'Petrol' ELSE '' END AS FuelType, ABVS.Value AS Transmission,  
   ABT.GradeId, ABG.Name AS Grade, ABT.ModelColorId, ABCLR.Name AS ColorName, ABCT.Name AS ColorType, ABCFR.Name AS ColorFor,
   ABT.UpholestryColorId, ABCLRU.Name AS UpholestryColorName,
   ABT.ExShowRoomPrice, ABT.VersionPriceId,  
   ST.ID AS StateId, ST.Name AS StateName, ABT.CityId, ABC.Name AS CityName,   
   ABT.DealerId, ABD.Name AS DealerName, ABD.Address AS DealerAddress,ABD.ZipCode,ABD.MobileNumber AS DealerMobileNumber,ABD.EmailID AS DealerEmailID, 
   ABT.PaymentMode, ABT.PaymentType, ABT.Amount,   
   ABT.ChequeAddress, ABT.ChequeStateId, ABT.ChequeCity, ABT.ChequePinCode, ABT.PGTransactionId, ABT.SourceId,
   APG.ResponseCode, APG.ResponseMsg, APG.EPGTransactionId, APG.AuthId, APG.ProcessCompleted, APG.TransactionCompleted
   FROM OLM_AudiBE_Transactions ABT  
   LEFT JOIN OLM_AudiBE_Models ABMO ON ABT.ModelId = ABMO.Id AND ABMO.IsActive = 1  
   LEFT JOIN OLM_AudiBE_Versions ABV ON ABT.VersionId = ABV.Id AND ABV.IsActive = 1
   LEFT JOIN OLM_AudiBE_Version_Specs ABVS ON ABT.VersionId = ABVS.VersionId AND ABVS.IsActive = 1 AND ABVS.SpecId = 5   
   LEFT JOIN OLM_AudiBE_Models ABVM ON ABV.ModelId = ABVM.Id AND ABVM.IsActive = 1
   LEFT JOIN OLM_AudiBE_Grades ABG ON ABT.GradeId = ABG.Id AND ABG.IsActive = 1  
   LEFT JOIN OLM_AudiBE_ModelColors ABMC ON ABT.ModelColorId = ABMC.Id AND ABMC.IsActive = 1  
   LEFT JOIN OLM_AudiBE_Colors ABCLR ON ABMC.ColorId = ABCLR.Id AND ABCLR.IsActive = 1  
   LEFT JOIN OLM_AudiBE_ColorFor ABCFR ON ABMC.ColorForId = ABCFR.Id AND ABCFR.IsActive = 1  
   LEFT JOIN OLM_AudiBE_ColorTypes ABCT ON ABMC.ColorTypeId = ABCT.Id AND ABCT.IsActive = 1 
   LEFT JOIN OLM_AudiBE_ModelColors ABMCU ON ABT.UpholestryColorId = ABMCU.Id AND ABMCU.IsActive = 1 
   LEFT JOIN OLM_AudiBE_Colors ABCLRU ON ABMCU.ColorId = ABCLRU.Id AND ABCLRU.IsActive = 1
   LEFT JOIN OLM_AudiBE_Cities ABC ON ABT.CityId = ABC.Id AND ABC.IsActive = 1  
   LEFT JOIN States ST ON ABC.StateId = ST.ID  
   LEFT JOIN OLM_AudiBE_VersionPrices ABVP ON ABT.VersionPriceId = ABVP.Id AND ABVP.IsActive = 1
   LEFT JOIN OLM_AudiBE_Dealers ABD ON ABT.DealerId = ABD.Id AND ABD.IsActive = 1 
   LEFT JOIN OLM_AudiBE_PGTransactions APG ON ABT.PGTransactionId = APG.ID
   WHERE ABT.Id = @TransactionId  
  END  
END  