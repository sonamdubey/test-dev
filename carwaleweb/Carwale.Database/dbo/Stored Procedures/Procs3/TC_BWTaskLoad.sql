IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_BWTaskLoad]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_BWTaskLoad]
GO

	


-- =============================================  
-- Author  : Vinay Kumar Prajapati,Ajay Singh 15 Jan  2015
-- purpose :   To get All today's callList And count for  all bikewale dealers Which are related to a user
-- EXECUTE TC_BWTaskLoad 3,null,'2016-01-25 12:28:43:997' 
-- =============================================     
CREATE PROCEDURE [dbo].[TC_BWTaskLoad]
	
	 @UserId  INT,
	 @LoginTime DATETIME = null		
AS
BEGIN 
        SET NOCOUNT ON
	    DECLARE @TempDealer TABLE(DealerId Int)

		INSERT INTO @TempDealer(DealerId)
		SELECT DAU.DealerId
		FROM DCRM_ADM_UserDealers AS DAU WITH(NOLOCK)
		INNER JOIN Dealers AS D WITH(NOLOCK) ON DAU.DealerId=D.ID AND D.ApplicationId=2 -- bike Wale
		WHERE UserId=@UserId


		---Case When User Login First Time In a Day
		IF @LoginTime IS NULL
			BEGIN
			   -- Get count Todays Active calls 
			   SELECT  COUNT(AC.TC_CallsId) AS TodaysCall 
			   FROM TC_ActiveCalls AS AC WITH(NOLOCK) 
			   INNER JOIN TC_Lead AS TL WITH(NOLOCK) ON  TL.TC_LeadId= AC.TC_LeadId AND  AC.ScheduledOn  <  CONVERT(DATE, GETDATE()+1)
			   INNER JOIN  @TempDealer AS TT  ON TT.DealerId = TL.BranchId


			   --GET All Other Details Needed related to Todays Active calls
			   SELECT  TL.BranchId AS DealerId,D.Organization AS DealerName , CD.Id AS CustomerId , CD.CustomerName ,CD.Mobile, TCIL.CarDetails AS InterestedIn ,  CONVERT(VARCHAR(36), AC.ScheduledOn, 113)   AS FollowUP  --AC.ScheduledOn AS FollowUP
			   ,CASE TCIL.TC_LeadInquiryTypeId
				WHEN 1
					THEN 'Used Buy'
				WHEN 2
					THEN 'Used Sell'
				WHEN 3
					THEN 'New Buy'
				END AS InquiryType
				,ISNULL(AC.LastCallComment,'') AS LastCallComment
				,AC.TC_CallsId
				,TCIL.TC_UserId AS UserId
				,TL.TC_LeadId
				,ISNULL(TBLS.Description,'') AS Status
			   FROM TC_ActiveCalls AS AC WITH(NOLOCK) 
			   INNER JOIN TC_Lead AS TL WITH(NOLOCK) ON  TL.TC_LeadId= AC.TC_LeadId AND AC.ScheduledOn  <  CONVERT(DATE, GETDATE()+1)
			   INNER JOIN  @TempDealer AS TT  ON TT.DealerId = TL.BranchId 
			   INNER JOIN Dealers AS D WITH(NOLOCK) ON D.ID= TT.DealerId 
			   INNER JOIN TC_CustomerDetails AS CD WITH(NOLOCK) ON  CD.id =TL.TC_CustomerId
			   INNER JOIN TC_InquiriesLead AS TCIL WITH(NOLOCK) ON TCIL.TC_LeadId= AC.TC_LeadId
			  LEFT JOIN TC_BWLeadStatus AS TBLS WITH(NOLOCK) ON TBLS.TC_BWLeadStatusId=TCIL.TC_BWLeadStatusId
			  ORDER BY AC.ScheduledOn  DESC

			END 
        --Case When user login second time or more in a day
		ELSE
			BEGIN
				-- Get count Todays Active calls which is till the login time  
			   SELECT COUNT(AC.TC_CallsId) AS TodaysCall 
			   FROM TC_ActiveCalls AS AC WITH(NOLOCK) 
			   INNER JOIN TC_Lead AS TL WITH(NOLOCK) ON  TL.TC_LeadId= AC.TC_LeadId AND AC.ScheduledOn  <= @LoginTime
			   INNER JOIN  @TempDealer AS TT  ON TT.DealerId = TL.BranchId 
			   INNER JOIN TC_InquiriesLead AS TCIL WITH(NOLOCK) ON TCIL.TC_LeadId= AC.TC_LeadId AND TCIL.CreatedDate BETWEEN  @LoginTime AND GETDATE() 

			--GET All Other Details Needed related to Todays Active calls
		    SELECT  TL.BranchId AS DealerId,D.Organization AS DealerName , CD.Id AS CustomerId , CD.CustomerName ,CD.Mobile, TCIL.CarDetails AS InterestedIn ,CONVERT(VARCHAR(36), AC.ScheduledOn, 113)  AS FollowUP
		   ,CASE TCIL.TC_LeadInquiryTypeId
			WHEN 1
				THEN 'Used Buy'
			WHEN 2
				THEN 'Used Sell'
			WHEN 3
				THEN 'New Buy'
			END AS InquiryType
			,ISNULL(AC.LastCallComment,'') AS LastCallComment
			,AC.TC_CallsId
			,TCIL.TC_UserId AS UserId
		    ,TL.TC_LeadId
			,ISNULL(TBLS.Description,'') AS Status
		   FROM TC_ActiveCalls AS AC WITH(NOLOCK) 
		   INNER JOIN TC_Lead AS TL WITH(NOLOCK) ON  TL.TC_LeadId= AC.TC_LeadId AND AC.ScheduledOn  <=  @LoginTime 
		   INNER JOIN  @TempDealer AS TT  ON TT.DealerId = TL.BranchId
		   INNER JOIN Dealers AS D WITH(NOLOCK) ON D.ID= TT.DealerId
		   INNER JOIN TC_CustomerDetails AS CD WITH(NOLOCK) ON  CD.id =TL.TC_CustomerId
		   INNER JOIN TC_InquiriesLead AS TCIL WITH(NOLOCK) ON TCIL.TC_LeadId= AC.TC_LeadId AND TCIL.CreatedDate BETWEEN @LoginTime AND GETDATE()
		   LEFT JOIN TC_BWLeadStatus AS TBLS WITH(NOLOCK) ON TBLS.TC_BWLeadStatusId=TCIL.TC_BWLeadStatusId
		   ORDER BY AC.ScheduledOn  DESC
	 END 


END 



