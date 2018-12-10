IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_TaskListLoadNew_V16]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_TaskListLoadNew_V16]
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
-- exec [dbo].[TC_TaskListLoadNew_V16.9.1] 1,20553,88927,1,20,null,null,null,null,null,null,1,null,null,null,null,33,null,null,6,null,2,null,0   --from dashboard
-- Modified By : Nilima More,call versionsing tasklist sp's.
-- Modified By : Suresh Prajapati on 15th Sept, 2016
-- Description : Added parameter @MasterDispositionId for Funnel filter
-- Modified By : Ashwini Dhamankar on Sept 22,2016 (Added @IsToday Parameter)
-- Modified By : Tejashree Patil on 28 Sept 2016, SP versioning from v16.9.1 to v16.10.1.
-- ======================================================================================================
CREATE PROCEDURE[dbo].[TC_TaskListLoadNew_V16.10.1]
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
	,@IsToday BIT = 0
AS
SET NOCOUNT ON

BEGIN
	IF @RequestType = 1 -- Page Load
		BEGIN
			
			IF @BusinessTypeId IN(3,5) --Sales, Advantage -- Auto scheduling of leads.
				EXEC TC_LeadVerificationScheduling @UserId,@BranchId

			EXEC [TC_TaskListPageLoad_V16.10.1] @BranchId-- Added By : Tejashree Patil on 28 Sept 2016
				,@UserId
				,@LeadBucketId
				,@BusinessTypeId
				,@MasterDispositionId
				,@LeadDispositionId
				,@IsToday
		END
	ELSE
		IF @RequestType = 2 -- Pagination
			EXEC [TC_TaskListPaging_V16.10.1] @BranchId-- Added By : Tejashree Patil on 28 Sept 2016
				,@UserId
				,@LeadBucketId
				,@FromIndex
				,@ToIndex
				,@BusinessTypeId
				,@MasterDispositionId
				,@LeadDispositionId
				,@IsToday
		ELSE
			IF @RequestType = 3 -- Searching
				EXEC [TC_TaskListLoadSearch_V16.10.1] @BranchId-- Added By : Tejashree Patil on 28 Sept 2016
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
					,@IsToday
			ELSE
				IF @RequestType = 4 -- Excel Data
					EXEC [TC_TaskData_V16.10.1] @UserId-- Added By : Tejashree Patil on 28 Sept 2016
						,@BranchId
						,@TopCount
						,@LeadBucketId
						,@BusinessTypeId
						,@MasterDispositionId
						,@LeadDispositionId
						,@IsToday
END

