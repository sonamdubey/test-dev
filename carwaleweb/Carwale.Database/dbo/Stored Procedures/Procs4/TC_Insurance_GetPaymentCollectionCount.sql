IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_Insurance_GetPaymentCollectionCount]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_Insurance_GetPaymentCollectionCount]
GO

	-- =============================================
-- Author:		Kartik Rathod
-- Create date: 14 sept 2016
-- Description:	to get pending payment collection count for cheque and pay at showroom (120 for chequepickup,121 for pay at showroom)
-- exec TC_Insurance_GetPaymentCollectionCount 20553,88927
-- =============================================
CREATE PROCEDURE [dbo].[TC_Insurance_GetPaymentCollectionCount]
@BranchId INT,
@UserId INT
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @Today DAte= getdate(), @TodaysChequePickupCount iNT,@PendingChequePickupCount int,@TodaysPayAtShowroom int,@PendingPayAtShowroom int,
			@ChequePickupId INT = 120, @PayAtShowroom  INT = 121

	SELECT II.TC_Insurance_InquiriesId,II.TC_LeadDispositionId, CONVERT(DATE,II.CollectionDateTime) AS CollectionDate
	INTO #TEMP
	FROM TC_Insurance_Inquiries II WITH(NOLOCK) 
	JOIN TC_InquiriesLead IL WITH(NOLOCK) ON II.TC_InquiriesLeadId = IL.TC_InquiriesLeadId AND II.TC_LeadDispositionId IN (120,121) -- 120 for chequepickup,121 for pay at showroom
	WHERE 
		IL.TC_UserId = @UserId AND II.BranchId = @BranchId AND CONVERT(DATE,II.CollectionDateTime) <= @Today
		
	
	SELECT	@TodaysChequePickupCount = sum(case when CollectionDate = @Today then 1 else 0 end),
			@PendingChequePickupCount = sum(case when CollectionDate < @Today then 1 else 0 end) 
	FROM #TEMP WITH(NOLOCK) 
	WHERE TC_LeadDispositionId = @ChequePickupId -- 120 for checkpickup,
	GROUP BY TC_LeadDispositionId

	SELECT	@TodaysPayAtShowroom = sum(case when  CollectionDate = @Today then 1 else 0 end) ,
			@PendingPayAtShowroom = sum(case when  CollectionDate < @Today then 1 else 0 end)  
	FROM #TEMP WITH(NOLOCK) 
	WHERE TC_LeadDispositionId = @PayAtShowroom -- 121 for pay at showroom
	GROUP BY TC_LeadDispositionId

	DROP TABLE #TEMP

	DECLARE @TempTable Table(Type VARCHAR(50),Counts INT, LeadDispositionId INT)
	
	INSERT INTO @TempTable(Type,Counts, LeadDispositionId ) VALUES('Cheque pick up due today',@TodaysChequePickupCount,@ChequePickupId);
	INSERT INTO @TempTable(Type,Counts, LeadDispositionId ) VALUES('Pending cheque pick ups',@PendingChequePickupCount,@ChequePickupId);
	INSERT INTO @TempTable(Type,Counts, LeadDispositionId ) VALUES('Pay at showroom due today',@TodaysPayAtShowroom,@PayAtShowroom);
	INSERT INTO @TempTable(Type,Counts, LeadDispositionId ) VALUES('Pending pay at showroom',@PendingPayAtShowroom,@PayAtShowroom);

	
	SELECT Type PaymentCollectionType,Counts, LeadDispositionId  FROM @TempTable


END


