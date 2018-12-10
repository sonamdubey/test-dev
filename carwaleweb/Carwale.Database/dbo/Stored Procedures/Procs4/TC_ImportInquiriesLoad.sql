IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_ImportInquiriesLoad]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_ImportInquiriesLoad]
GO

	-- =============================================
-- Author:		Tejashree Patil
-- Create date: 4 March 2013
-- Description:	This procedure will return user list having new car task 
-- =============================================
CREATE PROCEDURE [dbo].[TC_ImportInquiriesLoad] 
	@BranchId BIGINT  
AS
BEGIN
	
	SET NOCOUNT ON;
	--User list
	SELECT	DISTINCT U.Id,U.UserName 
	FROM	TC_Users U 
			INNER JOIN TC_RoleTasks R ON U.RoleId=R.RoleId    
	WHERE	U.IsActive=1 
			AND U.BranchId=@BranchId 
			AND R.TaskId =7 
			AND IsCarwaleUser=0 
		
	-- Inquiry Source	
	EXECUTE TC_InquirySourceSelect NULL
	
END
