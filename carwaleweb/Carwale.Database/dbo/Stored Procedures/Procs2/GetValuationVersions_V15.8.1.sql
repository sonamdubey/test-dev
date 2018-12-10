IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetValuationVersions_V15]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetValuationVersions_V15]
GO

	-- =============================================
-- Author:		<Kirtan Shetty>
-- Create date: <15/7/2014>
-- Description:	<Get car Versions on Model selection during valuation>
-- =============================================
CREATE PROCEDURE [dbo].[GetValuationVersions_V15.8.1]
	@modelId SMALLINT,
	@carYear SMALLINT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT DISTINCT Ve.ID AS Value
	,Ve.Name AS Text
	,Ve.SmallPic as SmallPic
	,Ve.LargePic as LargePic
	,Ve.HostURL
	,Ve.OriginalImgPath 
	FROM CarVersions Ve  WITH (NOLOCK)
	WHERE Ve.IsDeleted = 0 AND CarModelId=@modelId
	AND Ve.Id IN ( SELECT CarVersionId FROM CarValues WITH (NOLOCK) WHERE CarYear=@carYear) ORDER BY Text
END


