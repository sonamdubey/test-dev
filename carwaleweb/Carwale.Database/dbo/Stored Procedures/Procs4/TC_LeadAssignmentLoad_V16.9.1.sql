IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_LeadAssignmentLoad_V16]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_LeadAssignmentLoad_V16]
GO

	-- =============================================     
--- Author:  Surendra    
-- Create date:   10-01-2013  
-- Description:    
--execute [TC_LeadAssignmentLoad_V16.9.1] 5,1,null,null,null,null,null,null,null,243
-- Modified By : Umesh Ojha on 24 jul 2013 for fetching data for logged in user & its team 
--Modified by: Tejashree on 26-08-2013, Fetched complete name of a customer.
--Modified By Vivek Gupta on 21-01-2015, fetched Isverified from customerdetails to know if he customer is verified or not
--Modified by : Ashwini Dhamankar on Sept 28,2016 (Added condition for fromindex and toindex null)
-- =============================================     
CREATE PROCEDURE [dbo].[TC_LeadAssignmentLoad_V16.9.1]
	-- Add the parameters for the stored procedure here     
	@BranchId BIGINT
	,@UserId BIGINT
	,@FromIndex INT = NULL
	,@ToIndex INT = NULL
	,
	--@Type TINYINT , 
	@CustomerName VARCHAR(100) = NULL
	,@CustomerMobile VARCHAR(50) = NULL
	,@CustomerEmail VARCHAR(100) = NULL
	,@FromDate AS DATETIME = NULL
	,@ToDate AS DATETIME = NULL
	,@LogginUserId BIGINT 
AS
BEGIN
	--  Lead Scheduling for verifications
	DECLARE @FromFollowupDate DATETIME
	DECLARE @ToFollowupDate DATETIME
	DECLARE @FromInqDate DATETIME
	DECLARE @ToInqDate DATETIME
	DECLARE @TblAllChild TABLE (Id INT)

	IF (@UserId IS NULL)
	BEGIN
		SET @FromInqDate = @FromDate

		IF (@ToDate IS NOT NULL)
		BEGIN
			SET @ToInqDate = DATEADD(MINUTE, 59, (DATEADD(HH, 23, @ToDate)))
		END
	END
	ELSE
	BEGIN
		SET @FromFollowupDate = @FromDate

		IF (@ToDate IS NOT NULL)
		BEGIN
			SET @ToFollowupDate = DATEADD(MINUTE, 59, (DATEADD(HH, 23, @ToDate)))
		END

		SET @LogginUserId = NULL
	END;

	INSERT INTO @TblAllChild
	EXEC TC_GetALLChild @LogginUserId;

	WITH CT
	AS (
		SELECT (ISNULL(C.Salutation, '') + ' ' + C.CustomerName + ' ' + ISNULL(C.LastName, '')) CustomerName
			,C.Email
			,C.Mobile
			,L.TC_InquiriesLeadId TC_InquiryStatusId
			,ScheduledOn AS [NextFollowUpDate]
			,L.TC_InquiriesLeadId
			,L.CarDetails AS [InterestedIn]
			,InqType = CASE L.TC_LeadInquiryTypeId
				WHEN 1
					THEN 'Buyer'
				WHEN 2
					THEN 'Seller'
				ELSE 'New Buyer'
				END
			,U.UserName
			,ROW_NUMBER() OVER (
				ORDER BY L.LatestInquiryDate DESC
				) rownumber
			,C.IsVerified
		FROM TC_InquiriesLead L WITH (NOLOCK)
		INNER JOIN TC_CustomerDetails C WITH (NOLOCK) ON L.TC_CustomerId = C.Id
		LEFT JOIN TC_ActiveCalls A WITH (NOLOCK) ON L.TC_UserId = A.TC_UsersId
			AND L.TC_LeadId = A.TC_LeadId
		LEFT JOIN TC_Users U ON A.TC_UsersId = U.Id
		WHERE
			--TCAC.ScheduledON BETWEEN  @FromFolloupdate AND @ToFollowupdate 
			(
				@FromFollowupDate IS NULL
				OR (A.ScheduledON >= @FromFollowupDate)
				)
			AND (
				@ToFollowupdate IS NULL
				OR (A.ScheduledON <= @ToFollowupdate)
				)
			AND (
				@FromInqDate IS NULL
				OR (L.LatestInquiryDate >= @FromInqDate)
				)
			AND (
				@ToInqDate IS NULL
				OR (L.LatestInquiryDate <= @ToInqDate)
				)
			AND (
				L.TC_LeadStageId <> 3
				OR L.TC_LeadStageId IS NULL
				)
			AND L.BranchId = @BranchId
			-- AND (@UserId IS NULL OR (L.TC_UserId = @UserId ))
			AND (
				L.TC_UserId IN (
					SELECT ID
					FROM @TblAllChild
					)
				OR L.TC_UserId = @LogginUserId
				OR (L.TC_UserId = @UserId)
				)
			AND (
				(@CustomerName IS NULL)
				OR (C.CustomerName LIKE '%' + @CustomerName + '%')
				)
			AND (
				(@CustomerMobile IS NULL)
				OR (C.Mobile = @CustomerMobile)
				)
			AND (
				(@CustomerEmail IS NULL)
				OR (C.Email = @CustomerEmail)
				)
		)
	SELECT *
	INTO #tblTemp
	FROM CT

	SELECT *
	FROM #tblTemp
	WHERE (rownumber BETWEEN @FromIndex
			AND @ToIndex)
			 OR
			 (@FromIndex IS NULL AND @ToIndex IS NULL)   --added by : Ashwini Dhamankar on Sept 28,2016

	SELECT COUNT(*) AS RecordCount
	FROM #tblTemp

	DROP TABLE #tblTemp
END

