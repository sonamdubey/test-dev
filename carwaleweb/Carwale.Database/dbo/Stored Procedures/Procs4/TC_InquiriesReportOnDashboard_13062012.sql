IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_InquiriesReportOnDashboard_13062012]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_InquiriesReportOnDashboard_13062012]
GO

	



-- Created By:	Surendra
-- Create date: 6th Feb 2012
-- Description:TC_InquiriesReportOnDashboard 5
-- =============================================
CREATE PROCEDURE [dbo].[TC_InquiriesReportOnDashboard_13062012]
(
	@BranchId BIGINT,
	@UserId BIGINT,
	@IsAdmin BIT
)
as
BEGIN
	
	DECLARE @TotalPoolCount INT 
	SELECT @TotalPoolCount=COUNT(*) FROM TC_InquiriesLead 
	WHERE BranchId=@BranchId AND IsActionTaken=0
	-- checking here inquiry source
	SELECT COUNT(*) SourceCount, TC.SourceId, Source AS SourceName
		FROM TC_Inquiries TC
		INNER JOIN TC_InquirySource St ON TC.SourceId = St.Id
		WHERE TC.BranchId = @BranchId
		GROUP BY SourceId, Source
		
		-- checking here inquiry status.
		SELECT COUNT(IL.TC_InquiriesLeadId) StatusCount, IL.TC_InquiryStatusId, Status as StatusName 
				 FROM TC_InquiriesLead IL INNER JOIN TC_InquiryStatus St ON IL.TC_InquiryStatusId = St.TC_InquiryStatusId 
				 Where IL.TC_UserId = @UserId And IsActionTaken=1 AND IL.BranchId =@BranchId AND IL.TC_InquiriesFollowupActionId NOT IN(4,5,6,8)
				 GROUP BY IL.TC_InquiryStatusId, Status
				 
		SELECT COUNT(L.TC_InquiriesLeadId)As followupCount FROM TC_InquiriesLead L WHERE L.TC_InquiriesFollowupActionId<4 
                 AND L.BranchId=@BranchId AND L.NextFollowUpDate<=GETDATE() AND L.IsActionTaken=1
	IF(@IsAdmin=1)
	BEGIN
		SELECT T.*,@TotalPoolCount as InqPoolCount FROM(
		SELECT U.UserName,
		   sum(case IsActionTaken when 0 then 1 else 0 end) as Fresh,
		   sum(case IsActionTaken when 1 then 1 else 0 end) as FollowUp,
		   sum(case TC_InquiriesFollowupActionId when 3 then 1 when 4 then 1 when 5 then 1 else 0 end) as Closed,
		   --MAX(LastFollowupDate) as LastFollowupDate
		   MAX(SUBSTRING(CONVERT(CHAR, LastFollowupDate, 108),0,6)) as LastFollowupDate
			FROM TC_InquiriesLead L 
			INNER JOIN TC_Users U ON L.TC_UserId=U.Id
			WHERE L.BranchId=@BranchId
			group by U.UserName
			) T	
	END
		
	
END
