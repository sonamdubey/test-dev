IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetDefaultModelVersion]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetDefaultModelVersion]
GO

	--Author : Tejashree Patil on 7 Oct 2014
--Description: To get random default modelId and versionId from make
CREATE PROCEDURE [dbo].[TC_GetDefaultModelVersion]
	@DealerId INT,
	@ModelId INT = NULL
AS
BEGIN

	DECLARE @MakeId INT

	SELECT  TOP 1 @MakeId = DM.MakeId
	FROM    TC_DealerMakes DM WITH(NOLOCK)
	WHERE	DM.DealerId = @DealerId
	ORDER BY NEWID()

	SELECT  TOP 1 V.ModelId, V.VersionId 
	FROM	vwMMV V 
	WHERE	V.MakeId=@MakeId
			AND (@ModelId IS NULL OR V.ModelId=@ModelId)
			AND V.IsModelNew = 1 
	ORDER BY NEWID()
END
