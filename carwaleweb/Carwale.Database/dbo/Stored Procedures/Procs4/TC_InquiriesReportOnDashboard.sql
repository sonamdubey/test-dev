IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_InquiriesReportOnDashboard]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_InquiriesReportOnDashboard]
GO

	
---Modified binu, Date: 23-07-2012, desc- for added condition for worksheet only dealer
---Modified binu, Date: 06-06-2012, desc- for follow-up missed and pending inquiries to display in dashboard
-- Created By:	Surendra
-- Create date: 6th Feb 2012
-- Description:TC_InquiriesReportOnDashboard 5
-- Modified By: Tejashree Patil On 5 July 2012: WITH (NOLOCK)implementation 
-- =============================================
CREATE PROCEDURE [dbo].[TC_InquiriesReportOnDashboard]
(
	@BranchId BIGINT,
	@UserId BIGINT,
	@IsAdmin BIT
)
as
BEGIN
	DECLARE @TotalPoolCount INT , @IsWorkSheetOnly BIT
	SELECT @TotalPoolCount=COUNT(*) FROM TC_InquiriesLead WITH (NOLOCK)
	WHERE BranchId=@BranchId AND IsActionTaken=0 AND TC_UserId IS NULL
	-- checking here inquiry source
	SELECT COUNT(*) SourceCount, TC.SourceId, Source AS SourceName
		FROM TC_Inquiries TC WITH (NOLOCK)
		INNER JOIN TC_InquirySource St WITH (NOLOCK) ON TC.SourceId = St.Id
		WHERE TC.BranchId = @BranchId
		GROUP BY SourceId, Source
		
		
	IF(@IsAdmin=1)
	BEGIN
		SELECT T.*,@TotalPoolCount as InqPoolCount FROM(
		SELECT U.UserName,
		   sum(case IsActionTaken when 0 then 1 else 0 end) as Fresh,
		   sum(case IsActionTaken when 1 then 1 else 0 end) as FollowUp,
		   sum(case TC_InquiriesFollowupActionId when 3 then 1 when 4 then 1 when 5 then 1 else 0 end) as Closed,
		   --MAX(LastFollowupDate) as LastFollowupDate
		   MAX(SUBSTRING(CONVERT(CHAR, LastFollowupDate, 108),0,6)) as LastFollowupDate
			FROM TC_InquiriesLead L WITH (NOLOCK)
			INNER JOIN TC_Users U WITH (NOLOCK) ON L.TC_UserId=U.Id
			WHERE L.BranchId=@BranchId
			group by U.UserName
			) T	
			--here taking count of missed follow up calls only for not reachable, callback,busy/ringing	 
			SELECT COUNT(L.TC_InquiriesLeadId)As missedFollowupCount FROM TC_InquiriesLead L WITH (NOLOCK) WHERE L.TC_InquiriesFollowupActionId IN(1,2,7) 
            AND L.BranchId=@BranchId AND L.NextFollowUpDate<=GETDATE() AND L.IsActionTaken=1
                 
			--taking here pendig follow up of  current date super admin
			SELECT COUNT(L.TC_InquiriesLeadId)As pendingFollowupCount 
			FROM TC_InquiriesLead L WITH (NOLOCK) WHERE L.TC_InquiriesFollowupActionId IN(1,2,7)
			AND L.BranchId=@BranchId AND  L.NextFollowUpDate >= CONVERT(NCHAR(10), CURRENT_TIMESTAMP, 120) +' 00:01'
			AND L.IsActionTaken=1
			
			-- checking here inquiry status for super admin.
			SELECT COUNT(IL.TC_InquiriesLeadId) StatusCount, IL.TC_InquiryStatusId, Status as StatusName 
			FROM TC_InquiriesLead IL WITH (NOLOCK) INNER JOIN TC_InquiryStatus St WITH (NOLOCK) ON IL.TC_InquiryStatusId = St.TC_InquiryStatusId 
			Where  IsActionTaken=1 AND IL.BranchId =@BranchId AND IL.TC_InquiriesFollowupActionId NOT IN(4,5,6,8)
			GROUP BY IL.TC_InquiryStatusId, Status
	END
	ELSE
	BEGIN
		---Modified binu, Date: 23-07-2012, desc- for added condition for worksheet only dealer
		--checking here dealer using only work sheet or not
		SELECT @IsWorkSheetOnly =isWorksheetOnly FROM TC_DealerConfiguration where DealerId=@BranchId
		IF(@IsWorkSheetOnly=0)--if dealer using both inquiry and worksheet
		BEGIN
			
			--here taking count of missed follow up calls only for not reachable, callback,busy/ringing	 
			SELECT COUNT(L.TC_InquiriesLeadId)As missedFollowupCount FROM TC_InquiriesLead L WITH (NOLOCK) WHERE L.TC_InquiriesFollowupActionId IN(1,2,7) 
			AND L.BranchId=@BranchId AND L.NextFollowUpDate<=GETDATE() AND L.IsActionTaken=1 AND L.TC_UserId=@UserId
	        
			--taking here pendig follow up of  current date particular user
			SELECT COUNT(L.TC_InquiriesLeadId)As pendingFollowupCount 
			FROM TC_InquiriesLead L WITH (NOLOCK)  WHERE L.TC_InquiriesFollowupActionId IN(1,2,7)
			AND L.BranchId=@BranchId AND NextFollowUpDate >= CONVERT(NCHAR(10), CURRENT_TIMESTAMP, 120) +' 00:01'
			AND L.IsActionTaken=1 AND L.TC_UserId=@UserId
		END
		ELSE --if dealer using Only worksheet
		BEGIN
			SELECT COUNT(L.TC_InquiriesLeadId)As missedFollowupCount FROM TC_InquiriesLead L WITH (NOLOCK) WHERE L.TC_InquiriesFollowupActionId IN(1,2,7) 
			AND L.BranchId=@BranchId AND L.NextFollowUpDate<=GETDATE() AND L.IsActionTaken=1-- AND L.TC_UserId=@UserId
	        
			--taking here pendig follow up of  current date particular user
			SELECT COUNT(L.TC_InquiriesLeadId)As pendingFollowupCount 
			FROM TC_InquiriesLead L WITH (NOLOCK)  WHERE L.TC_InquiriesFollowupActionId IN(1,2,7)
			AND L.BranchId=@BranchId AND NextFollowUpDate >= CONVERT(NCHAR(10), CURRENT_TIMESTAMP, 120) +' 00:01'
			AND L.IsActionTaken=1 --AND L.TC_UserId=@UserId
		END
		
		-- checking here inquiry status for normal user.
		SELECT COUNT(IL.TC_InquiriesLeadId) StatusCount, IL.TC_InquiryStatusId, Status as StatusName 
		FROM TC_InquiriesLead IL WITH (NOLOCK) INNER JOIN TC_InquiryStatus St WITH (NOLOCK) ON IL.TC_InquiryStatusId = St.TC_InquiryStatusId 
		Where  TC_UserId=@UserId AND IsActionTaken=1 AND IL.BranchId =@BranchId AND IL.TC_InquiriesFollowupActionId NOT IN(4,5,6,8)
		GROUP BY IL.TC_InquiryStatusId, Status
	END
END

	

