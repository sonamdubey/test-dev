IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetValuationVersions]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetValuationVersions]
GO

	-- =============================================
-- Author:		<Kirtan Shetty>
-- Create date: <15/7/2014>
-- Description:	<Get car Versions on Model selection during valuation>
-- =============================================
CREATE PROCEDURE [dbo].[GetValuationVersions]
	@modelId SMALLINT,
	@carYear SMALLINT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT DISTINCT Ve.ID AS Value
	,Ve.Name AS Text
	,'http://' + Ve.HostURL + Ve.SmallPic as SmallPic
	,'http://' + Ve.HostURL + Ve.LargePic as LargePic 
	FROM CarVersions Ve  WITH (NOLOCK)
	WHERE Ve.IsDeleted = 0 AND CarModelId=@modelId
	AND Ve.Id IN ( SELECT CarVersionId FROM CarValues WITH (NOLOCK) WHERE CarYear=@carYear) ORDER BY Text
END



/****** Object:  StoredProcedure [dbo].[WA_FetchCarVersions]    Script Date: 6/9/2015 7:23:43 AM ******/
SET ANSI_NULLS ON
