IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_UpdateMostInterestedInquiry]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_UpdateMostInterestedInquiry]
GO

	-- =============================================
-- Author:		Vivek Gupta 
-- Create date: 29-09-2015
-- Description:	It will update most interested flag in newcarbuyer and usedcarbuyer inquiries tables only not seller inquiries table
-- Ruchira Patil on 5th Oct 2016 (Avoided "SELECT * INTO Temp" and Used "CREATE TEMP, Then Insert")
-- =============================================
CREATE PROCEDURE [dbo].[TC_UpdateMostInterestedInquiry]
	 @BranchId INT
	,@InquiryId INT
	,@InquiryType TINYINT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT OFF;	

	DECLARE @TC_InquiriesLeadId INT
	DECLARE @TC_LeadId INT
	
	CREATE TABLE #TempInquiriesLeadId (Id INT)

	IF @InquiryType = 1
	BEGIN
		SET @TC_InquiriesLeadId = (SELECT TC_InquiriesLeadId FROM TC_BuyerInquiries WITH(NOLOCK) WHERE TC_BuyerInquiriesId = @InquiryId)
		SET @TC_LeadId = (SELECT TC_LeadId FROM TC_InquiriesLead WITH(NOLOCK) WHERE TC_InquiriesLeadId = @TC_InquiriesLeadId AND BranchId = @BranchId)

		INSERT into #TempInquiriesLeadId (id)
		SELECT TC_InquiriesLeadId
		FROM TC_InquiriesLead WITH(NOLOCK)
		WHERE TC_LeadId = @TC_LeadId
		AND BranchId = @BranchId
		
		UPDATE TC_BuyerInquiries SET MostInterested = 0 WHERE TC_InquiriesLeadId IN (SELECT Id FROM #TempInquiriesLeadId)
		UPDATE TC_NewCarInquiries SET MostInterested = 0 WHERE TC_InquiriesLeadId IN (SELECT Id FROM #TempInquiriesLeadId)

		UPDATE TC_BuyerInquiries SET MostInterested = 1 WHERE TC_BuyerInquiriesId = @InquiryId
	END;

	ELSE IF @InquiryType = 3
	BEGIN
		SET @TC_InquiriesLeadId = (SELECT TC_InquiriesLeadId FROM TC_NewCarInquiries WITH(NOLOCK) WHERE TC_NewCarInquiriesId = @InquiryId)
		SET @TC_LeadId = (SELECT TC_LeadId FROM TC_InquiriesLead WITH(NOLOCK) WHERE TC_InquiriesLeadId = @TC_InquiriesLeadId AND BranchId = @BranchId)

		INSERT into #TempInquiriesLeadId (id)
		SELECT TC_InquiriesLeadId 
		FROM TC_InquiriesLead WITH(NOLOCK)
		WHERE TC_LeadId = @TC_LeadId
		AND BranchId = @BranchId

		UPDATE TC_BuyerInquiries SET MostInterested = 0 WHERE TC_InquiriesLeadId IN (SELECT Id FROM #TempInquiriesLeadId)
		UPDATE TC_NewCarInquiries SET MostInterested = 0 WHERE TC_InquiriesLeadId IN (SELECT Id FROM #TempInquiriesLeadId)

		UPDATE TC_NewCarInquiries SET MostInterested = 1 WHERE TC_NewCarInquiriesId = @InquiryId
	END

	DROP TABLE #TempInquiriesLeadId
END
