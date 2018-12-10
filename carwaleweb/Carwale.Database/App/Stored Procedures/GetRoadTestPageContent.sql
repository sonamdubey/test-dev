IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[App].[GetRoadTestPageContent]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [App].[GetRoadTestPageContent]
GO

	
-- =========================================================
-- Author:		Supriya
-- Create date: 10/31/2012
-- Description:	SP for fetching contents of a roadtest page
-- ==========================================================

CREATE PROCEDURE [App].[GetRoadTestPageContent]
	@BasicId Integer,
	@Priority Integer=-1
AS
BEGIN

	SET NOCOUNT ON;
	IF @Priority=1
	BEGIN
		SELECT	
			PC.Data,P.Priority,P.PageName 
		FROM	
			Con_EditCms_Pages P 
			LEFT JOIN Con_EditCms_PageContent PC 
			ON P.Id = PC.PageId AND P.Priority=@Priority
		WHERE	P.BasicId = @BasicId 
    END
    ELSE
    BEGIN
		SELECT	
			PC.Data,P.Priority,P.PageName 
		FROM	
			Con_EditCms_Pages P, Con_EditCms_PageContent PC 
		WHERE	P.BasicId = @BasicId AND P.Id = PC.PageId AND P.Priority=@Priority	
    END
END

