IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_SendMailAutoPausePackageData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_SendMailAutoPausePackageData]
GO

	

--==============================================
-- Author: Vinay Kumar Prajapati
-- purpose : Get all data for  Mailling purpose, When Package AutoPauses For (UCD OR UCD_NCD dealer) 
-- EXEC DCRM_SendMailAutoPausePackageData  
-- =============================================

CREATE  PROCEDURE [dbo].[DCRM_SendMailAutoPausePackageData]
@IsMailSent bit = 0
AS
BEGIN
	   --Avoid extra Messsge 
	   SET NOCOUNT ON

	   IF(@IsMailSent =1)
	  BEGIN
			UPDATE DCRM_AutoPausedDataToSendmail SET  IsMailSend=1  WHERE ISNULL(IsMailSend,0)=0
	  END
	  ELSE
		BEGIN

			SELECT   APSM.TransactionId, APSM.DealerId, APSM.DealerName, APSM.CityName AS  City, 
			    APSM.L3Name, APSM.PackageName AS Product, APSM.Amount,APSM.StartDate,APSM.PausedDate,APSM.L3UserId, APSM.L3LoginId
			FROM DCRM_AutoPausedDataToSendmail AS APSM WITH(NOLOCK)
	   
			WHERE ISNULL(APSM.IsMailSend,0)=0
	   END

	   -- Update After get data for sending mail
	  --- UPDATE DCRM_AutoPausedDataToSendmail SET  IsMailSend=1  WHERE ISNULL(IsMailSend,0)=0

	  

END 
