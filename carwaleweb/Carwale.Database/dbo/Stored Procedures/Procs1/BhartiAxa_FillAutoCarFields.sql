IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BhartiAxa_FillAutoCarFields]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BhartiAxa_FillAutoCarFields]
GO

	-- =============================================
-- Author:		<Author,,Ashish Verma>
-- Create date: <Create Date,17/04/1991,>
-- Description:	<Description,for auto fill cardata based on versionId,>
-- =============================================
CREATE PROCEDURE  [dbo].[BhartiAxa_FillAutoCarFields]  
	-- Add the parameters for the stored procedure here
	@versionId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	--Select CM.Name AS makeName,
	--CM.ID as makeId,
	--CMO.Name AS modelName,CMO.ID AS modelId, 
	--CV.Name as versionName,
	--CV.ID as versionId 
	--from CarVersions CV inner join CarModels CMO on CMO.ID= CV.CarModelId
	-- inner join CarMakes CM on CM.ID=CMO.ID where CV.ID=@versionId

	select CMV.id as versionId,CMO.id as modelId, CM.id as makeId
	from carversions CMV with(nolock) inner join CarModels CMO with(nolock) on CMV.CarModelId = CMO.ID
	inner join carmakes CM with(nolock) on CMO.CarMakeId=CM.ID
	where CMV.id=@versionId
	--select MakeId as makeId,ModelId as modelId from  [CD].[vwMMV] where VersionId = @versionId

	 
END

