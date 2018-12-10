IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetCompareCarList_v16_8_6]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetCompareCarList_v16_8_6]
GO

	-- =============================================
-- Author:		Jitendra solanki
-- Create date: 21 mar 2016
-- Description:	This SP will return comparision car list based on start and last index
-- Modidied By Ajay Singh on 23 august 2016 fetched modelId for comparision cars
-- =============================================
CREATE PROCEDURE [dbo].[GetCompareCarList_v16_8_6]
	-- Add the parameters for the stored procedure here
	@StartIndex INT = NULL,
	@LastIndex INT = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT TempView.* 
	FROM
		(SELECT 
				ROW_NUMBER() OVER (
								ORDER BY CASE 
										WHEN DISPLAYPRIORITY IS NULL
											THEN 1
										ELSE 0
										END
									,DISPLAYPRIORITY ASC
								) AS Row
				,CL.ID AS ID
				,CMA1.NAME + ' ' + CMO1.NAME + ' ' + CV1.NAME AS Car1
				,CMA1.NAME AS Make1
				,CMA2.NAME AS Make2
				,CMO1.NAME AS Model1
				,CMO2.NAME AS Model2
				,CMA2.NAME + ' ' + CMO2.NAME + ' ' + CV2.NAME AS Car2
				,CL.EntryDate
				,CL.IsActive
				,CL.DisplayPriority
				,CL.HostUrl
				,CL.VersionId1
				,CL.VersionId2
				,CL.OriginalImgPath
				,CL.ImageName
				,CMO1.ID AS ModelId1 --added by ajay singh
				,CMO2.ID AS ModelId2 --added by ajay singh
			FROM 
				Con_CarComparisonList CL WITH(NOLOCK) 
				INNER JOIN CarVersions AS CV1 WITH(NOLOCK) ON CV1.ID = CL.VersionId1 
				INNER JOIN CarModels AS CMO1 WITH(NOLOCK) ON CMO1.ID = CV1.CarModelId 
				INNER JOIN CarMakes AS CMA1 WITH(NOLOCK) ON  CMA1.ID = CMO1.CarMakeId
				INNER JOIN CarVersions AS CV2 WITH(NOLOCK) ON CV2.ID = CL.VersionId2 
				INNER JOIN CarModels AS CMO2 WITH(NOLOCK) ON CMO2.ID = CV2.CarModelId
				INNER JOIN CarMakes AS CMA2 WITH(NOLOCK) ON CMA2.ID = CMO2.CarMakeId 
			WHERE 
				CL.IsArchived = 0
				AND CL.IsActive = 1
				AND CV1.New=1
				AND CV2.New=1
				AND CMO1.New=1
				AND CMO2.New=1
				AND CV1.IsDeleted=0
				AND CV2.IsDeleted=0
				AND CMO1.IsDeleted=0
				AND CMO2.IsDeleted=0) AS TempView
		WHERE
			TempView.Row >=  @StartIndex  AND TempView.Row <=  @LastIndex

END

