IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[IndividualClassifiedPredictiveDialerCalls]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[IndividualClassifiedPredictiveDialerCalls]
GO

	-- =============================================
-- Author:		Avishkar
-- Create date: 16/03/2012
-- Description:	Set records for Payment reminder and 
-- Inquiry pool for Individual classified
-- EXEC IndividualClassifiedPredictiveDialerCalls '2012-02-18 00:00:00.002','2012-03-06 12:09:16.587'
-- =============================================
CREATE PROCEDURE [dbo].[IndividualClassifiedPredictiveDialerCalls]
	-- Add the parameters for the stored procedure here
	 @fromdate datetime,@todate datetime
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	DECLARE @cnt BIGINT
	
	CREATE TABLE #TempICPredDialerCalls
	(
	  Name varchar(100),
	  Mobile varchar(21),
	  ContactNo varchar(21),
	  CategoryId Char(10),
	  ReferenceId varchar(100),
	  Comments varchar(200),
	  TZ char(2),
	  CallType char(1) -- 1 for  PaymentReminder and 2 for InquiryPool 
	)

     INSERT INTO #TempICPredDialerCalls(Name,Mobile,ContactNo,CategoryId,ReferenceId,Comments,TZ,CallType)
	 SELECT DISTINCT CC.Name, 
		   '0' + CC.Mobile, 
		    '' AS ContactNo, 
		   '4' AS CategoryId,
		    CC1.Id AS ReferenceId, 
		   'CustomerPaymentReminder : ' + vw.Make + ' ' + vw.Model + ' ' + vw.Version AS Comments, 
		   'TZ' AS TZ,
		   1 as CallType  -- 1 for  PaymentReminder
		   FROM CustomerSellInquiries AS CSI WITH(NOLOCK)
			   JOIN Customers AS CC  WITH(NOLOCK) ON  CC.Id = CSI.CustomerId 
			   JOIN vwMMV as vw ON CSI.CarVersionId = vw.VersionId
			   JOIN CH_Calls AS CC1  WITH(NOLOCK) ON CC1.EventId = CSI.ID 
			   JOIN CH_ScheduledCalls AS CSC  WITH(NOLOCK) ON CSC.CallID=CC1.ID					                             
		   WHERE CC1.CallType = 7 
		   AND CC1.TBCType = 2
		   AND CSI.IsFake = 0                   
		   AND (CSI.IsVerified = 1 OR CSI.IsApproved = 1)
		   AND CSI.PackageType <> 2  
		   AND  CSI.EntryDate  between @fromdate and @todate 
		   AND CC1.Id NOT IN( 
							   SELECT CallId 
							   FROM CH_Logs AS CL1 
							   WHERE CL1.CallId = CC1.ID 
							   AND ActionId IN(2,12,13,53,54,55,56,57,59)
							  )
           AND CC1.TcId!=464 -- To avoid duplicate calls in Aspect   
		
		
		INSERT INTO #TempICPredDialerCalls(Name,Mobile,ContactNo,CategoryId,ReferenceId,Comments,TZ,CallType)
		SELECT DISTINCT CC.Name,
		     '0'+CC.Mobile as Mobile,
		      '' AS ContactNo,
		      4 AS CategoryId, 
		      CC1.Id as ReferenceId,
		      'SellInquiryVerification: ' + vw.Make + ' ' + vw.Model + ' ' + vw.Version AS Comments, 
		      'TZ' as TZ,
		       2 as CallType -- 2 for InquiryPool 
		FROM CustomerSellInquiries AS CSI 
		     JOIN Customers AS CC ON CC.Id = CSI.CustomerId
		     JOIN Cities AS C ON  C.Id = CC.CityId
		     JOIN States AS S ON  S.Id = C.StateId
		     JOIN vwMMV AS vw ON vw.VersionId=CSI.CarVersionId
		     JOIN CH_Calls AS CC1 ON CC1.EventId = CSI.ID
		     JOIN CH_ScheduledCalls AS CSC ON CC1.ID = CSC.CallId		    
		     LEFT JOIN OprUsers AS OU ON CC1.TcId = OU.Id
		WHERE CC1.CallType = 1 AND CC1.TBCType = 2
		AND  CSI.EntryDate  between @fromdate and @todate
		AND CC1.TcId!=464 -- To avoid duplicate calls in Aspect
		
		 --Assign calls to Individual Predictive User
		UPDATE CH_Calls
		set TcId=464
		from CH_Calls as CC
		   join #TempICPredDialerCalls as temp on temp.ReferenceId=CC.ID
		where temp.CallType=1
		
		UPDATE CH_ScheduledCalls
		set TcId=464
		from CH_ScheduledCalls as CC
		   join #TempICPredDialerCalls as temp on temp.ReferenceId=CC.CallID
		where temp.CallType=1
		
		UPDATE CH_Calls
		set TcId=464
		from CH_Calls as CC
		   join #TempICPredDialerCalls as temp on temp.ReferenceId=CC.ID
		where temp.CallType=2
		
		UPDATE CH_ScheduledCalls
		set TcId=464
		from CH_ScheduledCalls as CC
		   join #TempICPredDialerCalls as temp on temp.ReferenceId=CC.CallID
		where temp.CallType=2
		
		SELECT Name,Mobile,ContactNo,CategoryId,ReferenceId,Comments,TZ  
		FROM #TempICPredDialerCalls
		
		SELECT @cnt=COUNT(*)
		FROM #TempICPredDialerCalls
		
		insert into IClDialerCallUploadDeatails values(GETDATE(),@fromdate ,@todate,@cnt)
		
		insert into IClDialerCallDeatails
		select GETDATE(),Mobile,ReferenceId as CallId,Comments
		from #TempICPredDialerCalls

		
		DROP TABLE #TempICPredDialerCalls
		   
END
