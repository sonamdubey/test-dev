IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[FetchCarVersions]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[FetchCarVersions]
GO

	-- =============================================
-- Author:		Shikhar
-- Create date: 07-01-2013
-- Description:	Returns Car Versions for a model id 
-- edited by :  amit verma on 3 september 2013  -- added column makeid
-- edited by :  amit verma on 23 september 2013  -- added columns New,IsSpecsAvailable
-- =============================================
CREATE PROCEDURE [cw].[FetchCarVersions]
	@ModelId SMALLINT = 0
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
SELECT 
	CM.Name + ' ' + Mo.Name + ' ' + CV.Name AS FullCarName,
	CV.ID AS Value,
	CV.Name AS [Text],
	CV.Futuristic,
	CM.ID MakeId,		--amit verma on 3 september 2013  -- added column makeid
	CV.New,				--amit verma on 23 september 2013  -- added column New
	CV.IsSpecsAvailable	--amit verma on 23 september 2013  -- added column IsSpecsAvailable
FROM 
	CarVersions CV WITH(NOLOCK)
	INNER JOIN CarModels Mo WITH(NOLOCK)
		ON CV.CarModelId = MO.ID
	INNER JOIN CarMakes CM WITH(NOLOCK)
		ON CM.ID = Mo.CarMakeId
WHERE 
	(CV.CarModelId = @ModelId OR @ModelId = 0)
AND
	CV.IsDeleted = 0	
END
