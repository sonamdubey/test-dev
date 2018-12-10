IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetSimilarCarModels]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetSimilarCarModels]
GO

	
-- =============================================
-- Author:		Shalini Nair
-- Create date: 10/07/14
-- Description:	Gets the similar car models(Alternate models) based on the ModelId passed 
-- Modified By :Shalini Nair
--Description : Added order by clause and added columns ModelSequence,SortCol,IsFeatured,featuredModelId 
--[dbo].[GetSimilarCarModels] 236
-- =============================================
CREATE PROCEDURE [dbo].[GetSimilarCarModels]
	-- Add the parameters for the stored procedure here
	@ModelId INT
AS
DECLARE @ModelIds VARCHAR(30)
,@Model varchar(30)

BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT @ModelIds = SCM.SimilarModels
	FROM SimilarCarModels SCM WITH (NOLOCK)
	WHERE SCM.ModelId = @ModelId

	PRINT @ModelIds

	Select Top 1 @Model=CVF.CarModelId 
	FROM CompareFeaturedCar Cf WITH (NOLOCK)
	JOIN  CarVersions AS CVF WITH (NOLOCK) ON CF.VersionId = CVF.ID 
	                          AND CVF.ID = CF.FeaturedVersionId 
	 JOIN  fnSplitCSV(@ModelIds) MN ON  CVF.CarModelId=MN.ListMember 
	 WHERE 	CF.IsActive = 1 
	 AND CF.IsResearch = 1 

	

	SET @ModelIds=ISNULL(@ModelIds,0)+ ISNULL(@Model,0)

	-- Insert statements for procedure here
	SELECT TOP 3 
	      CMA.NAME AS Make
		,CMO.ID AS ModelId
		,CMO.NAME AS Model
		,CMO.MaskingName
		,CMO.SmallPic
		,CMO.HostURL
		,CMO.MinPrice
		,CMO.MaxPrice
		,CMO.ReviewRate
		,CMO.ReviewCount
		,CMO.LargePic
		,CMO.CarVersionID_Top
		,M.id  ModelSequence		-- modified by shalini
		,-1 AS SortCol, 0 as IsFeatured --modified by shalini
		,@Model as featuredModelId		--modified by shalini
	FROM CarMakes CMA WITH(NOLOCK)
	 JOIN CarModels CMO WITH(NOLOCK) ON CMO.CarMakeId = CMA.ID
	 JOIN CarVersions CV WITH(NOLOCK) ON CMO.ID = CV.CarModelId
	  JOIN fnSplitCSVValuesWithIdentity(@ModelIds) M ON CMO.ID = M.ListMember
	WHERE 
		CV.New = 1
		AND CV.IsDeleted = 0
	GROUP BY CMA.NAME
		,CMO.ID
		,CMO.NAME
		,CMO.SmallPic
		,CMO.HostURL
		,CMO.MinPrice
		,CMO.MaxPrice
		,CMO.MaskingName
		,CMO.ReviewRate
		,CMO.ReviewCount
		,CMO.LargePic
		,CMO.CarVersionID_Top
		,M.id
		order by m.id 

	 Select Cf.SpotlightUrl				-- added by shalini
	FROM CompareFeaturedCar Cf WITH (NOLOCK)
	JOIN  CarVersions AS CVF WITH (NOLOCK) ON CF.VersionId = CVF.ID 
	                          AND CVF.ID = CF.FeaturedVersionId 
	 JOIN  fnSplitCSV(@ModelIds) MN ON  CVF.CarModelId=MN.ListMember 
	 WHERE 	CF.IsActive = 1 
	 AND CF.IsResearch = 1
	--print @Model
END


