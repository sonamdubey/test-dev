IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_SendMailAndSMSForContracts]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_SendMailAndSMSForContracts]
GO

	-- =============================================
-- Author	:	Vinay Kumar Prajapati  7th Desc 2015
-- Description: Used to get details of user and send mail and  sms (If Cotract reached 75 85,95 or above 100) 
-- EXEC DCRM_SendMailAndSMSForContracts
-- Modified By : Komal Manjare on 25 July 2016 get only active users and roleid(3-Sales Field) condition for sms
--				 get userid and mobileNo as per the dealer assigned to it. 
--Modified By : Komal Manjare on 29 July 2016 cast and round function added for groupcategory to include between values
-- =============================================

CREATE PROCEDURE [dbo].[DCRM_SendMailAndSMSForContracts]
	
AS

BEGIN
	--Modified By : Komal Manjare on 25 July 2016  insert data into temptable for calculating the contract where achievement>74
	DECLARE  @TempSmsContract TABLE (DealerId INT,DealerName VARCHAR(100),ContractId INT,UserId NUMERIC(18,0),PhoneNo VARCHAR(25),MailTo VARCHAR(50),GroupCategory DECIMAL(10,2)
	,UserName VARCHAR(50),ContractStartDate DATETIME,LeadSigned INT,LeadDeliver  INT,ReplacementContractId INT) 

	INSERT INTO @TempSmsContract(DealerId,DealerName,ContractId,UserId,PhoneNo,MailTo,UserName,ContractStartDate,
								LeadSigned,LeadDeliver,ReplacementContractId,GroupCategory) 

     SELECT  TCM.DealerId, D.Organization AS DealerName,TCM.ContractId,DAU.UserId,OU.PhoneNo,REPLACE(ISNULL(OU.LoginId,''), '@carwale.com', '') AS MailTo,OU.UserName,  -- Modified By : Komal Manjare on 25 July 2016 PhoneNo and mobile number fetched
	 ISNULL(TCM.StartDate,'') AS ContractStartDate,TCM.TotalGoal AS LeadSigned,ISNULL(TCM.TotalDelivered,0) AS LeadDeliver,ISNULL(TCM.ReplacementContractId,0) AS ReplacementContractId,
						
			-- Modified By : Komal Manjare on 29 July 2016 cast and round function added for groupcategory
			CAST(( CASE TCM.ContractBehaviour 
				  WHEN 1 THEN  --- Lead Based 
				       ( CASE ISNULL(TCM.TotalGoal,0) WHEN 0 THEN 0.00 ELSE ROUND(((ISNULL(TCM.TotalDelivered,0)*100.0)/ TCM.TotalGoal),2) END )
				  WHEN 2 THEN	 -- Duration Based 
					   (CASE WHEN ISNULL(DATEDIFF(DAY, TCM.StartDate, TCM.EndDate),0) <=  0 THEN 0.00 
						WHEN  ISNULL(DATEDIFF(DAY, TCM.StartDate, TCM.EndDate),0)<=ISNULL(DATEDIFF(DAY, TCM.StartDate, GETDATE()),0)  THEN 100.00 --  if end date of contract passed 
					   ELSE ROUND(((ISNULL(DATEDIFF(DAY, TCM.StartDate, GETDATE()),0)*100.0)/ ISNULL(DATEDIFF(DAY, TCM.StartDate, TCM.EndDate),0)),2) END ) -- get actual percent of duration based 
					   ELSE 0 END) AS DECIMAL(10,2) )AS GroupCategory

			FROM TC_ContractCampaignMapping AS TCM WITH(NOLOCK)  
			INNER JOIN DCRM_ADM_UserDealers AS DAU WITH(NOLOCK) ON DAU.DealerId =TCM.DealerId AND DAU.RoleId=3 -- Modified By : Komal Manjare on 25 July 2016 Send sms only to sales field
			INNER JOIN Dealers AS D WITH(NOLOCK) ON D.ID=TCM.DealerId AND D.TC_DealerTypeId IN(2,3)
			INNER JOIN OprUsers AS OU WITH(NOLOCK) ON OU.Id=DAU.UserId AND OU.IsActive=1 
			WHERE TCM.ContractStatus = 1 AND ISNULL(TCM.TotalGoal,0) <> 0 AND   ((ISNULL(TCM.TotalDelivered,0)*100)/ISNULL(TCM.TotalGoal,-1))>74 		
        
		 -- Sms details to send the specific Executive (GroupCategory 85 defined as contract are in between 85 to 94 and so on ) 
		-- Modified By : Komal Manjare on 25 July 2016 fetch userdetails for the dealer where actualpercentage is greater than 74

		SELECT  COUNT(TSM.DealerId) AS TotalDealer,
		 CASE 
			 WHEN  CAST(ROUND(TSM.GroupCategory,0) AS INT) BETWEEN 75 AND 84 THEN 75
			 WHEN CAST(ROUND(TSM.GroupCategory,0) AS INT) BETWEEN  85 AND 94 THEN 85
			 WHEN CAST(ROUND(TSM.GroupCategory,0) AS INT)  BETWEEN  95 AND 100 THEN 95
			 WHEN CAST(ROUND(TSM.GroupCategory,0) AS INT)  >100  THEN 100
			ELSE 0 END  AS GroupCategory,
		TSM.UserId,TSM.PhoneNo AS MobileNo,TSM.MailTo
		FROM @TempSmsContract TSM 
		WHERE CAST(TSM.GroupCategory AS DECIMAL(10,2))>74
		 GROUP BY 
		  CASE 
			 WHEN  CAST(ROUND(TSM.GroupCategory,0) AS INT) BETWEEN 75 AND 84 THEN 75
			 WHEN CAST(ROUND(TSM.GroupCategory,0) AS INT) BETWEEN  85 AND 94 THEN 85
			 WHEN CAST(ROUND(TSM.GroupCategory,0) AS INT)  BETWEEN  95 AND 100 THEN 95
			 WHEN CAST(ROUND(TSM.GroupCategory,0) AS INT)  >100  THEN 100
			ELSE 0 END ,
			TSM.UserId,TSM.PhoneNo,TSM.MailTo
		
	
	--	  SELECT COUNT(CTE.DealerId) AS TotalDealer,CTE.GroupCategory  FROM CTE WITH(NOLOCK)  GROUP BY CTE.GroupCategory ORDER BY CTE.GroupCategory 			       
			
		-- For sending  Mail Execute Sp 
		 --Modified By : Komal Manjare on 25 July 2016  Get mail details to be sent to sales field executive
		SELECT DISTINCT TSM.DealerId,TSM.DealerName,TSM.UserId,TSM.UserName,TSM.MailTo,TSM.ContractId,TSM.GroupCategory  AS ActualPercentage,CONVERT(VARCHAR(11),TSM.ContractStartDate,106) AS ContractStartDate
	    ,TSM.LeadSigned,TSM.LeadDeliver,TSM.ReplacementContractId		
		FROM @TempSmsContract TSM
		WHERE TSM.GroupCategory>74
		ORDER BY GroupCategory

		--	EXECUTE DCRM_SendMailForContracts 

END
