IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetABCookieList]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetABCookieList]
GO

	-- =============================================
-- Author:		<Anuj Dhar>
-- Create date: <18/10/2016>
-- Description:	<Fetch list of ABCookie values for template of a template group>
-- =============================================
CREATE PROCEDURE [dbo].[GetABCookieList]
	@GroupId INT,
	@TemplateId INT
AS
DECLARE @MappingId INT
BEGIN
	SET NOCOUNT ON

	SET @MappingId = (SELECT Id 
					  FROM TemplateGroupMapping WITH (NOLOCK) 
					  WHERE GroupId = @GroupId AND TemplateId = @TemplateId)

	SELECT ABCookieValue AS ABCookies 
	FROM TemplateGroupABCookieMapping WITH (NOLOCK) 
	WHERE TemplateGroupMappingId = @MappingId
	ORDER BY ABCookies
END

