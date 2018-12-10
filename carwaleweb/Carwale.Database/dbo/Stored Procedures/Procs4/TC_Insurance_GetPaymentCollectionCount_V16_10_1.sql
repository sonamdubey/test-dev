IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_Insurance_GetPaymentCollectionCount_V16_10_1]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_Insurance_GetPaymentCollectionCount_V16_10_1]
GO

	-- =============================================
-- Author:		Kartik Rathod
-- Create date: 14 sept 2016
-- Description:	to get pending payment collection count for cheque and pay at showroom (120 for chequepickup,121 for pay at showroom)
-- exec [TC_Insurance_GetPaymentCollectionCount_V16.9.1] 20553,88927
-- Modified By : Ashwini Dhamankar on Sept 22,2016 (added IsToday in select query)
-- exec [TC_Insurance_GetPaymentCollectionCount_V16.9.1] 20553,88927
-- EXEC TC_Insurance_GetPaymentCollectionCount_V16_10_1 20553,'88929,88930'
-- Modified By : Khushaboo Patil on 20th oct 2016 added reporting users to get count of all leads of users reporing to logged in user
-- Modified By : Nilima More On 3rd Nov 2016,label change in #TempTable  
-- =============================================
CREATE PROCEDURE [dbo].[TC_Insurance_GetPaymentCollectionCount_V16_10_1] 
@BranchId INT,
@ReportingUsersList VARCHAR(MAX) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @TodaysChequePickupCount INT,@PendingChequePickupCount INT,@TodaysPayAtShowroom INT,@PendingPayAtShowroom INT,
			@ChequePickupId INT = 120, @PayAtShowroom  INT = 121
	
	CREATE TABLE #TempUsers (UserId INT)
		
	IF (@ReportingUsersList IS NOT NULL)
		BEGIN 
			INSERT INTO #TempUsers (UserId)
			SELECT ListMember AS UserId FROM fnSplitCSV(@ReportingUsersList)
		END 

		CREATE TABLE #TEMP (TC_Insurance_InquiriesId INT,TC_LeadDispositionId INT,CollectionDate DATE)

		INSERT INTO #TEMP(TC_Insurance_InquiriesId,TC_LeadDispositionId,CollectionDate)
		SELECT II.TC_Insurance_InquiriesId,II.TC_LeadDispositionId, CONVERT(DATE,II.CollectionDateTime) AS CollectionDate
	
		FROM TC_Insurance_Inquiries II WITH(NOLOCK) 
		JOIN TC_InquiriesLead IL WITH(NOLOCK) ON II.TC_InquiriesLeadId = IL.TC_InquiriesLeadId AND II.TC_LeadDispositionId IN (120,121) -- 120 for chequepickup,121 for pay at showroom
		WHERE 
			(IL.TC_UserId IN (SELECT UserId FROM #TempUsers) OR @ReportingUsersList IS NULL) AND II.BranchId = @BranchId AND 
			DATEDIFF(dd,II.CollectionDateTime, GETDATE()) >= 0

	SELECT	@TodaysChequePickupCount = SUM(CASE WHEN DATEDIFF(dd,CollectionDate, GETDATE()) = 0 THEN 1 ELSE 0 END),
			@PendingChequePickupCount = SUM(CASE WHEN DATEDIFF(dd,CollectionDate, GETDATE()) > 0 THEN 1 ELSE 0 END) 
	FROM #TEMP WITH(NOLOCK) 
	WHERE TC_LeadDispositionId = @ChequePickupId -- 120 for checkpickup,
	GROUP BY TC_LeadDispositionId

	SELECT	@TodaysPayAtShowroom = SUM(CASE WHEN DATEDIFF(dd,CollectionDate, GETDATE()) = 0 THEN 1 ELSE 0 END),
			@PendingPayAtShowroom = SUM(CASE WHEN DATEDIFF(dd,CollectionDate, GETDATE()) > 0 THEN 1 ELSE 0 END) 
	FROM #TEMP WITH(NOLOCK) 
	WHERE TC_LeadDispositionId = @PayAtShowroom -- 121 for pay at showroom
	GROUP BY TC_LeadDispositionId

	DROP TABLE #TEMP

	create table #TempTable (Type VARCHAR(50),Counts INT, LeadDispositionId INT,IsToday BIT)
	
	INSERT INTO #TempTable(Type,Counts, LeadDispositionId,IsToday) VALUES('Cheque pick ups due today',@TodaysChequePickupCount,@ChequePickupId,1)
	INSERT INTO #TempTable(Type,Counts, LeadDispositionId,IsToday) VALUES('Pending cheque pick ups',@PendingChequePickupCount,@ChequePickupId,0)
	INSERT INTO #TempTable(Type,Counts, LeadDispositionId,IsToday) VALUES('Payments scheduled at showroom today',@TodaysPayAtShowroom,@PayAtShowroom,1)
	INSERT INTO #TempTable(Type,Counts, LeadDispositionId,IsToday) VALUES('Pending payments scheduled at showroom',@PendingPayAtShowroom,@PayAtShowroom,0)
	
	SELECT Type PaymentCollectionType,Counts, LeadDispositionId,IsToday FROM #TempTable
	DROP TABLE #TempUsers
	DROP TABLE #TempTable
END



