IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_TaskListLoadNew]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_TaskListLoadNew]
GO

	
-- =====================================================================================================
-- Author		: Suresh Prajapati
-- Create date	: 30th June, 2016
-- Description	: This SP is used to execute TaskList SP based on @RequestType Parameter
-- Modified By : Ashwini Dhamankar on June 28,2016 (added @LeadInquiryTypeId parameter)
-- Modified By : Deepak on July 27,2016 (Changed @LeadInquiryTypeId parameter to @BusinessTypeId)
-- EXEC TC_TaskListLoadNew 1,20466,88916,1,10,null,null,null,null,null,null,1,null,null,null,null,12,null,null,3,2
-- EXEC TC_TaskListLoadNew 1,5,243,1,10,null,null,null,null,null,null,1,null,null,null,null,10,null,null,3,2
--exec [dbo].[TC_TaskListLoadNew] 2,20553,88927,1,20,null,null,null,null,null,null,1,null,null,null,null,27,null,null,6,null,0
--exec [dbo].[TC_TaskListLoadNew] 2,20466,88916,1,20,null,null,null,null,null,null,1,null,null,null,null,12,null,null,4,null,0,null
-- Modified By : Nilima More,call versionsing tasklist sp's.
-- Modified By : Suresh Prajapati on 15th Sept, 2016
-- Description : Added parameter @MasterDispositionId for Funnel filter
-- ======================================================================================================
CREATE PROCEDURE [dbo].[TC_TaskListLoadNew]
	-- Add the parameters for the stored procedure here 
	@RequestType SMALLINT
	,@BranchId INT
	,@UserId INT
	,@FromIndex INT = NULL
	,@ToIndex INT = NULL
	,@CustomerName VARCHAR(100)
	,@CustomerMobile VARCHAR(50)
	,@CustomerEmail VARCHAR(100)
	,@FromFolloupdate AS DATETIME
	,@ToFollowupdate AS DATETIME
	,@SearchText VARCHAR(50) = NULL
	,@FilterType TINYINT = 1
	,@InqStatus VARCHAR(10) = NULL
	,@InqPriority VARCHAR(10) = NULL
	,@InqAddedDate AS DATETIME = NULL
	,@LeadIds AS VARCHAR(MAX) = NULL
	,@LeadBucketId AS SMALLINT = 1
	,@CarName VARCHAR(100) = NULL
	,@SourceName VARCHAR(50) = NULL
	,@BusinessTypeId TINYINT = 3
	,@TopCount INT = NULL
	,@MasterDispositionId INT = 0
	,@LeadDispositionId INT = NULL
AS
SET NOCOUNT ON

BEGIN
	IF @RequestType = 1 -- Page Load
		BEGIN
			
			IF @BusinessTypeId IN(3,5) --Sales, Advantage -- Auto scheduling of leads.
				EXEC TC_LeadVerificationScheduling @UserId,@BranchId

			EXEC [TC_TaskListPageLoad] @BranchId
				,@UserId
				,@LeadBucketId
				,@BusinessTypeId
				,@MasterDispositionId
				,@LeadDispositionId
		END
	ELSE
		IF @RequestType = 2 -- Pagination
			EXEC TC_TaskListPaging @BranchId
				,@UserId
				,@LeadBucketId
				,@FromIndex
				,@ToIndex
				,@BusinessTypeId
				,@MasterDispositionId
				,@LeadDispositionId
		ELSE
			IF @RequestType = 3 -- Searching
				EXEC [TC_TaskListLoadSearch] @BranchId
					,@UserId
					,@FromIndex
					,@ToIndex
					,@CustomerName
					,@CustomerMobile
					,@CustomerEmail
					,@FromFolloupdate
					,@ToFollowupdate
					,@SearchText
					,@FilterType
					,@InqStatus
					,@InqPriority
					,@InqAddedDate
					,@LeadIds
					,@LeadBucketId
					,@CarName
					,@SourceName
					,@BusinessTypeId
					,@MasterDispositionId
					,@LeadDispositionId
			ELSE
				IF @RequestType = 4 -- Excel Data
					EXEC [TC_TaskData] @UserId
						,@BranchId
						,@TopCount
						,@LeadBucketId
						,@BusinessTypeId
						,@MasterDispositionId
						,@LeadDispositionId
END


