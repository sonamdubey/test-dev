IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_Insurance_FetchEmployeeDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_Insurance_FetchEmployeeDetails]
GO

	
--============================================
-- Author:		Nilima More 
-- Create date: Oct 19,2016
-- Description:	Get Employee performance details
--exec [TC_Insurance_FetchEmployeeDetails] 20553,'2016-10-19','2016-10-20 23:59:59.657',null,8
-- Modified By : Nilima More On 8th Nov 2016,modified join for leadvalue = 6
-- =============================================
CREATE PROCEDURE [dbo].[TC_Insurance_FetchEmployeeDetails]
	-- Add the parameters for the stored procedure here
	@BranchId INT	
	,@FromDate DATETIME = NULL
	,@ToDate DATETIME = NULL
	,@UserIdList VARCHAR(MAX) = NULL
	,@LeadValue TINYINT = NULL
AS
BEGIN
	DECLARE @CheckId TINYINT = NULL

	IF(@LeadValue = 2) --Confirmation Pending
		SET @CheckId = 137
	IF(@LeadValue = 3) --payment pending
		SET @CheckId = 2		
	IF(@LeadValue = 4) --payment failed
		SET @CheckId = 135	
	IF(@LeadValue = 5)  --renewal pending
		SET @CheckId = 3
	IF(@LeadValue = 6) --renewal complete
		SET @CheckId =5	--renewal complete
	IF(@LeadValue = 7) --Lost leads
		SET @CheckId = 4
	IF(@LeadValue = 8) --pending first follow up
		SET @CheckId = 26
	IF(@LeadValue = 9) --missed follow up
		SET @CheckId = 28

	--Insert all active records in temp table awithin daterange and branch and user
	CREATE TABLE #TempActiveInquiries(CustomerName VARCHAR(100),CustomerMobile VARCHAR(100),ExpiryDate DATETIME,TC_LeadDispositionId INT,BucketTypeId INT,Car VARCHAR(300))

	INSERT	INTO #TempActiveInquiries (CustomerName,CustomerMobile,ExpiryDate,TC_LeadDispositionId,BucketTypeId,Car)
	SELECT	DISTINCT TL.CustomerName,TL.CustomerMobile,II.ExpiryDate,TL.TC_LeadDispositionId,TL.BucketTypeId,TL.Car
	FROM	TC_TaskLists TL WITH(NOLOCK) 
			LEFT JOIN TC_Insurance_Inquiries II WITH(NOLOCK) ON TL.TC_InquiriesLeadId = II.TC_InquiriesLeadId
	WHERE	TL.BranchId = @BranchId 
			AND (TL.UserId IN (SELECT ListMember FROM fnsplitcsv(@UserIdList))OR @UserIdList IS NULL)
			AND TL.TC_InquiriesLeadCreateDate BETWEEN @FromDate AND @ToDate

	--Fetch records depending on leadValues
	IF(@LeadValue IN (1,2,4,8,9)) --1:Active inquiries, 2:Confirmation Pending, 4:payment failed, 8:pending first follow up --9:missed follow up
	BEGIN
		SELECT	CustomerName,CustomerMobile,ExpiryDate,Car
		FROM	#TempActiveInquiries 
		WHERE	(@LeadValue IN (1,2,4) AND (@CheckId IS NULL OR TC_LeadDispositionId = @CheckId))
				OR 
				(@LeadValue IN (8,9) AND (BucketTypeId = @CheckId))
	END

	IF(@LeadValue IN(3,5)) ---- 3:payment pending,5:renewal pending,6:renewal complete
	BEGIN
		SELECT	T.CustomerName,T.CustomerMobile,T.ExpiryDate,Car
		FROM	#TempActiveInquiries T 
				INNER JOIN TC_LeadDisposition LD WITH(NOLOCK) ON LD.TC_LeadDispositionId = T.TC_LeadDispositionId
		WHERE  LD.TC_MasterDispositionId = @CheckId
		
	END
	IF(@LeadValue = 7 OR @LeadValue = 6) --7:Lost leads,6:renewal complete --added by Nilima More On 8th Nov 2016,modified join for leadvalue = 6
	BEGIN
		SELECT	Distinct CustomerName,MobileNumber as CustomerMobile,ExpiryDate,VW.Car AS Car
		FROM	TC_InquiriesLead IL WITH(NOLOCK)
				LEFT JOIN TC_Insurance_Reminder ISR WITH(NOLOCK) ON ISR.CustomerId = IL.TC_CustomerId
				INNER JOIN TC_LeadDisposition LD WITH(NOLOCK) ON LD.TC_LeadDispositionId = IL.TC_LeadDispositionId
				INNER JOIN VWALLMMV VW WITH(NOLOCK) ON VW.VersionId = ISR.VersionId
		WHERE	LD.TC_MasterDispositionId = @CheckId
				AND IL.BranchId = @BranchId 
				AND (IL.TC_UserId IN (SELECT ListMember FROM fnsplitcsv(@UserIdList))OR @UserIdList IS NULL)
				AND IL.CreatedDate BETWEEN  @FromDate AND @ToDate
	END
	
	DROP TABLE #TempActiveInquiries
		
END
