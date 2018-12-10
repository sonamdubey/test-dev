IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetRelease_SP]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetRelease_SP]
GO

	-- =============================================
-- Author:		Binumon George
-- Create date: 21-12-2011
-- Description:	instead of entry date using displaydate
-- =============================================
-- =============================================
-- Author:		Binumon George
-- Create date: 07-10-2011
-- Description:	get tc updates .it retrive  bugs in month base
-- =============================================
CREATE PROCEDURE [dbo].[TC_GetRelease_SP]
AS
BEGIN
	SELECT DISTINCT DATENAME(MONTH, DisplayDate) + ' ' + CONVERT(VARCHAR,DATEPART(YEAR, DisplayDate))AS BugDate, Content,DisplayDate 
	FROM TC_Release WHERE IsActive=1  Group By Content, DisplayDate ORDER BY DisplayDate DESC 
END

