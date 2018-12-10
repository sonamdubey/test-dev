IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_SetUniqueIdForUsers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_SetUniqueIdForUsers]
GO

	-- =============================================
-- Author:		Avishkar
-- Create date: 09-10-2012
-- Description:	Assign UniqueId in TC_Users
-- =============================================
CREATE PROCEDURE [dbo].[TC_SetUniqueIdForUsers]	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	--select CONVERT(VARCHAR(20), RIGHT(NEWID(),12)) +CONVERT(VARCHAR(8),t1.Id) +CONVERT(VARCHAR(8),GETDATE(),112)
	--from TC_USERS t1

    UPDATE t1
	SET UniqueId=CONVERT(VARCHAR(20), RIGHT(NEWID(),12)) +CONVERT(VARCHAR(8),t1.Id) +CONVERT(VARCHAR(8),GETDATE(),112)
	FROM TC_USERS as t1
		join TC_USERS as t2 on t1.Id=t2.Id and t1.IsActive=1
    
END
