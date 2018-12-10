IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_Insurance_GetPaymentCollectionCount_V16]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_Insurance_GetPaymentCollectionCount_V16]
GO

	-- =============================================
-- Author:		Kartik Rathod
-- Create date: 14 sept 2016
-- Description:	to get pending payment collection count for cheque and pay at showroom (120 for chequepickup,121 for pay at showroom)
-- exec [TC_Insurance_GetPaymentCollectionCount_V16.9.1] 20553,88927
-- Modified By : Ashwini Dhamankar on Sept 22,2016 (added IsToday in select query)
-- =============================================
CREATE PROCEDURE [dbo].[TC_Insurance_GetPaymentCollectionCount_V16.9.1] 
@BranchId INT,
@UserId INT
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @TodaysChequePickupCount INT,@PendingChequePickupCount INT,@TodaysPayAtShowroom INT,@PendingPayAtShowroom INT,
			@ChequePickupId INT = 120, @PayAtShowroom  INT = 121

	SELECT II.TC_Insurance_InquiriesId,II.TC_LeadDispositionId, CONVERT(DATE,II.CollectionDateTime) AS CollectionDate
	INTO #TEMP
	FROM TC_Insurance_Inquiries II WITH(NOLOCK) 
	JOIN TC_InquiriesLead IL WITH(NOLOCK) ON II.TC_InquiriesLeadId = IL.TC_InquiriesLeadId AND II.TC_LeadDispositionId IN (120,121) -- 120 for chequepickup,121 for pay at showroom
	WHERE 
		IL.TC_UserId = @UserId AND II.BranchId = @BranchId AND 
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

	DECLARE @TempTable Table(Type VARCHAR(50),Counts INT, LeadDispositionId INT,IsToday BIT)
	
	INSERT INTO @TempTable(Type,Counts, LeadDispositionId,IsToday) VALUES('Cheque pick up due today',@TodaysChequePickupCount,@ChequePickupId,1);
	INSERT INTO @TempTable(Type,Counts, LeadDispositionId,IsToday) VALUES('Pending cheque pick ups',@PendingChequePickupCount,@ChequePickupId,0);
	INSERT INTO @TempTable(Type,Counts, LeadDispositionId,IsToday) VALUES('Pay at showroom due today',@TodaysPayAtShowroom,@PayAtShowroom,1);
	INSERT INTO @TempTable(Type,Counts, LeadDispositionId,IsToday) VALUES('Pending pay at showroom',@PendingPayAtShowroom,@PayAtShowroom,0);

	
	SELECT Type PaymentCollectionType,Counts, LeadDispositionId,IsToday FROM @TempTable
END



