IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetBugFeedbackCategory]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetBugFeedbackCategory]
GO

	


-- =============================================
-- Author:		Binu
-- Create date: 30 May, 2012
-- Description:	Getiing BugFeedback Category for binding ddl
-- =============================================
CREATE PROCEDURE [dbo].[TC_GetBugFeedbackCategory]
AS
BEGIN
	SELECT TC_BugFeedbackCategory_Id, Category FROM TC_BugFeedbackCategory WHERE IsActive=1
END
