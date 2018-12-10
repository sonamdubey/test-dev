IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_LeadFollowupLoad]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_LeadFollowupLoad]
GO

	-- ======================================================================
-- Author : Suresh Prajapati
-- Created On : 28th June, 2016
-- Description : To Get Add Follow up page details
-- ======================================================================
create PROCEDURE [dbo].[TC_LeadFollowupLoad]
	-- Add the parameters for the stored procedure here  
	@BranchId INT
	,@CustomerId INT
	,@UserId INT
	,@LeadId INT
	,@leadOwnerId INT = NULL
	,@ApplicationId TINYINT = 1
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from  
	-- interfering with SELECT statements.  
	SET NOCOUNT OFF;

	DECLARE @IsBuyerAuthorized BIT = 0
	DECLARE @IsSellerAuthorized BIT = 0
	DECLARE @IsNewAuthorized BIT = 0
	DECLARE @IsUserAuthorized BIT = 0

	-- Modified By: Nilesh Utture on 24th April, 2013
	EXEC TC_GetUserAuthorization @UserId
		,@BranchId
		,@LeadId
		,@IsBuyerAuthorized OUTPUT
		,@IsSellerAuthorized OUTPUT
		,@IsNewAuthorized OUTPUT -- true if user is having Admin/reporting Sales manager rights

	IF (
			@IsBuyerAuthorized = 1
			OR @IsSellerAuthorized = 1
			OR @IsNewAuthorized = 1
			)
	BEGIN
		SET @IsUserAuthorized = 1
	END

	SELECT @IsUserAuthorized AS IsVerified --tABLE [0]

	SELECT L1.TC_InquiryStatusId --tABLE [1]
		,L.TC_LeadStageId AS TC_LeadStageId
	FROM TC_InquiriesLead AS L1 WITH (NOLOCK)
	INNER JOIN TC_Lead AS L WITH (NOLOCK) ON L.TC_LeadId = L1.TC_LeadId
	WHERE L1.BranchId = @BranchId
		AND L1.TC_LeadId = @LeadId

	DECLARE @IsLeadVerified BIT = 0

	IF EXISTS (
			SELECT TOP 1 TC_LeadId
			FROM TC_Lead L WITH (NOLOCK)
			WHERE L.TC_CustomerId = @CustomerId
				AND BranchId = @BranchId
				AND L.TC_LeadStageId = 2
			)
	BEGIN
		SET @IsLeadVerified = 1
	END

	EXEC TC_INQActivityFeedLoad @LeadId;--tABLE [2]

	-- Modified By : Nilesh Utture on 27th Feb, 2013
	SELECT UserName --tABLE [3]
		,@IsUserAuthorized AS IsUserAuthorized
	FROM TC_Users U WITH (NOLOCK)
	WHERE U.Id = @UserId --WHERE TC_LeadId = @LeadId -- Modified By: Nilesh Utture on 24th April, 2013

	SELECT A.TC_LeadDispositionId --tABLE [4]
		,A.VisitDate
		,A.Purpose
		,U.UserName
	FROM TC_Appointments A WITH (NOLOCK)
	INNER JOIN TC_Users U WITH (NOLOCK) ON U.Id = A.TC_UsersId
	WHERE TC_LeadId = @LeadId;
END
