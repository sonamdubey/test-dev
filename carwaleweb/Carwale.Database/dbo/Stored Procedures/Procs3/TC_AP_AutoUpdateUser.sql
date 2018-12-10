IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_AP_AutoUpdateUser]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_AP_AutoUpdateUser]
GO

	-- =============================================
-- Author:		Nilima More 
-- Create date: 22nd Sept 2016
-- Description:	Autoassign users to leads. 
-- =============================================
CREATE PROCEDURE [dbo].[TC_AP_AutoUpdateUser]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	--SET NOCOUNT ON;
	----assign users to lead depends on call to avoid mismatch in usersId
	UPDATE TIL
	SET TIL.TC_UserId = TT.UserId
	FROM TC_TaskLists TT WITH(NOLOCK) 
			INNER JOIN TC_InquiriesLead TIL WITH(NOLOCK) ON TT.TC_InquiriesLeadId = TIL.TC_InquiriesLeadId  
	WHERE TT.UserId <> TIL.TC_UserId
	
	 
END
