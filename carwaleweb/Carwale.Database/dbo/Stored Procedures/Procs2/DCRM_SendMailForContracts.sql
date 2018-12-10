IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_SendMailForContracts]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_SendMailForContracts]
GO

	

-- =============================================
-- Author	:	Vinay Kumar Prajapati  7th Desc 2015
-- Description: Used to get Details for sending mail to executive   regarding no. of  Contract reached 85 or 95 or 100 
-- EXEC DCRM_SendMailForContracts
-- Modifier : Vaibhav K 15 July 2016 cast ActualPercentage value to decimal upto 2 plcaes
-- =============================================

CREATE PROCEDURE [dbo].[DCRM_SendMailForContracts]
AS
BEGIN	 
	 
	     WITH CTE AS(
			SELECT DISTINCT  TCM.DealerId, D.Organization AS DealerName,TCM.ContractId,ISNULL(TCM.StartDate,'') AS ContractStartDate,
			TCM.TotalGoal AS LeadSigned,ISNULL(TCM.TotalDelivered,0) AS LeadDeliver,ISNULL(TCM.ReplacementContractId,0) AS ReplacementContractId,
			CAST((CASE TCM.ContractBehaviour
			  WHEN 1 THEN  --- Lead Based 
				( CASE ISNULL(TCM.TotalGoal,0) WHEN 0 THEN 0.00 ELSE ROUND(((ISNULL(TCM.TotalDelivered,0)*100.0)/ TCM.TotalGoal),2) END )
			  WHEN 2 THEN	 -- Duration Based 
				  (CASE WHEN ISNULL(DATEDIFF(DAY, TCM.StartDate, TCM.EndDate),0) <=  0 THEN 0.00 
					 WHEN  ISNULL(DATEDIFF(DAY, TCM.StartDate, TCM.EndDate),0)<=ISNULL(DATEDIFF(DAY, TCM.StartDate, GETDATE()),0)  THEN 100.00 --  if end date of contract passed 
				   ELSE ROUND(((ISNULL(DATEDIFF(DAY, TCM.StartDate, GETDATE()),0)*100.0)/ ISNULL(DATEDIFF(DAY, TCM.StartDate, TCM.EndDate),0)),2) END ) -- get actual percent of duration based 
			  ELSE 0 END) AS decimal(10,2)) AS ActualPercentage -- Vaibhav K 15 July 2016 cast value decimal upto 2 plcaes
			--CASE ISNULL(TCM.TotalGoal,0) WHEN 0 THEN 0 ELSE ((ISNULL(TCM.TotalDelivered,0)*100)/TCM.TotalGoal) END  ActualPercentage
			FROM TC_ContractCampaignMapping AS TCM WITH(NOLOCK)  
			INNER JOIN Dealers AS D WITH(NOLOCK) ON D.ID=TCM.DealerId
			WHERE ContractStatus=1 
		)

 
		SELECT  CT.DealerId,CT.DealerName,OU.Id AS UserId,OU.UserName,ISNULL(OU.LoginId,'') AS LoginId,CT.ContractId,CT.ActualPercentage,CONVERT(VARCHAR(11),ISNULL(CT.ContractStartDate,''),106)  AS ContractStartDate
	    ,CT.LeadSigned,CT.LeadDeliver,CT.ReplacementContractId
	    FROM  CTE AS CT WITH(NOLOCK)	 
			INNER JOIN DCRM_ADM_UserDealers AS DAU WITH(NOLOCK) ON DAU.DealerId =CT.DealerId
			INNER JOIN OprUsers AS OU WITH(NOLOCK) ON OU.Id=DAU.UserId
		 WHERE CT.ActualPercentage>74


		SELECT DISTINCT OU.Id AS UserId,OU.UserName ,REPLACE(ISNULL(OU.LoginId,''), '@carwale.com', '') AS MailTo	
		FROM TC_ContractCampaignMapping AS TCM WITH(NOLOCK) 
		INNER JOIN DCRM_ADM_UserDealers AS DAU WITH(NOLOCK) ON DAU.DealerId =TCM.DealerId
		AND 
		   (
		   CASE TCM.ContractBehaviour
			  WHEN 1 THEN  --- Lead Based 
			   ( CASE ISNULL(TCM.TotalGoal,0) WHEN 0 THEN 0.00 ELSE ROUND(((ISNULL(TCM.TotalDelivered,0)*100.0)/ TCM.TotalGoal),2) END )
			  WHEN 2 THEN	 -- Duration Based 
			  (CASE WHEN ISNULL(DATEDIFF(DAY, TCM.StartDate, TCM.EndDate),0) <=  0 THEN 0.00 
			     WHEN  ISNULL(DATEDIFF(DAY, TCM.StartDate, TCM.EndDate),0)<=ISNULL(DATEDIFF(DAY, TCM.StartDate, GETDATE()),0)  THEN 100.00 --  if end date of contract passed 
			   ELSE ROUND(((ISNULL(DATEDIFF(DAY, TCM.StartDate, GETDATE()),0)*100.0)/ ISNULL(DATEDIFF(DAY, TCM.StartDate, TCM.EndDate),0)),2) END ) -- get actual percent of duration based 
			  ELSE 0 END ) > 74
		
	
		INNER JOIN OprUsers AS OU WITH(NOLOCK) ON  OU.Id=DAU.UserId
		WHERE ContractStatus=1
		-- AND 	ISNULL(TCM.TotalGoal,0) <> 0 AND ((ISNULL(TCM.TotalDelivered,0)*100)/ISNULL(TCM.TotalGoal,-1))>74	

END
