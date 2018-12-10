IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CH_APFollowupEval]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CH_APFollowupEval]
GO

	

CREATE PROCEDURE [dbo].[CH_APFollowupEval]
	
AS

	DECLARE @OBLast NUMERIC
	DECLARE @UpId NUMERIC
	DECLARE @OB NUMERIC
	DECLARE @Paid NUMERIC
	DECLARE @NA NUMERIC
	DECLARE @NAO NUMERIC
	DECLARE @Discarded NUMERIC
	DECLARE @CB NUMERIC
	

BEGIN
		--GET Last day data
		SELECT @OBLast = CF.OB, @UpId = CF.Id FROM CH_FollowEVal AS CF
		WHERE DAY(CF.RecordDate) = DAY(DATEADD(day,-1,GETDATE()))
		AND MONTH(CF.RecordDate) = MONTH(DATEADD(day,-1,GETDATE()))
		AND YEAR(CF.RecordDate) = YEAR(DATEADD(day,-1,GETDATE()))
		
		--Get last days paid data
		SELECT @Paid = COUNT(DISTINCT CSI.Id)
		FROM CustomerSellInquiries AS CSI, ConsumerPackageRequests AS CPR, CH_Calls AS CC
		WHERE CSI.Id = CC.EventId AND CPR.ItemId = CSI.ID AND CSI.PackageType = 2
		AND CPR.PackageId = 1 AND CPR.ActualAmount > 0 AND CPR.isActive = 1 AND CPR.IsApproved = 1
		AND DAY(CPR.EntryDate) = DAY(DATEADD(day,-1,GETDATE()))
		AND MONTH(CPR.EntryDate) = MONTH(DATEADD(day,-1,GETDATE()))
		AND YEAR(CPR.EntryDate) = YEAR(DATEADD(day,-1,GETDATE()))
		AND CC.CallType IN(17,1) AND CC.TBCType = 2 AND CC.TcId <> 251
		AND DATEDIFF(dd,CSI.EntryDate,CPR.EntryDate) > 0
		
		--GET New additions in followup from fresh inquiries
		SELECT @NA = COUNT(CSI.ID) FROM CH_ScheduledCalls AS CH, CustomerSellInquiries AS CSI
		WHERE CSI.ID = CH.EventId
		AND DAY(CSI.EntryDate) = DAY(DATEADD(day,-1,GETDATE()))
		AND MONTH(CSI.EntryDate) = MONTH(DATEADD(day,-1,GETDATE())) 
		AND YEAR(CSI.EntryDate) = YEAR(DATEADD(day,-1,GETDATE()))
		AND CH.TBCType = 2 AND CH.CallType = 7
		
		--GET New additions in followup from old inquiries
		SELECT @NAO = COUNT(CC.id) FROM CH_Calls AS CC, CustomerSellInquiries AS CSI 
		WHERE DAY(EntryDateTime) = DAY(DATEADD(day,-1,GETDATE()))
		AND MONTH(EntryDateTime) = MONTH(DATEADD(day,-1,GETDATE()))
		AND YEAR(EntryDateTime) = YEAR(DATEADD(day,-1,GETDATE()))
		AND CallType = 7 AND CSI.ID = CC.EventId
		AND DATEDIFF(dd,CSI.EntryDate, CC.EntryDateTime) > 0

		--GET Discarded Data
		SELECT @Discarded = COUNT(DISTINCT CL.CallId)
		FROM CH_Logs AS CL, CH_Calls AS CC, CustomerSellInquiries AS CSI
		WHERE CL.CallId = CC.ID AND CC.EventId = CSI.ID AND
		CL.ActionId IN(2,53,54,55,56,57,12,13,59,63,64)
		AND DAY(CL.CalledDateTime) = DAY(DATEADD(day,-1,GETDATE()))
		AND MONTH(CL.CalledDateTime) = MONTH(DATEADD(day,-1,GETDATE()))
		AND YEAR(CL.CalledDateTime) = YEAR(DATEADD(day,-1,GETDATE()))
		AND DATEDIFF(dd, CSI.EntryDate, CL.CalledDateTime) > 0
		AND CC.TBCType = 2 AND CC.CallType = 7
		
		--GET Closing Balance
		SET @CB = (@OBLast - @Paid) + ((@NA + @NAO) - @Discarded)
		
		UPDATE CH_FollowEVal SET Paid = @Paid, NewAddition = (@NA + @NAO), Discarded = @Discarded, CB = @CB
		WHERE ID = @UpId
		
		--GET OPENING BALANCE OF CALLS
		--SELECT @OB = COUNT(DISTINCT CSI.ID) FROM CH_ScheduledCalls AS CH, CustomerSellInquiries AS CSI
		--WHERE CSI.ID = CH.EventId AND CH.TBCType = 2 AND CH.CallType = 7
		
		--Save next day opening balance data
		INSERT INTO CH_FollowEVal(OB) VALUES(@CB)
END

