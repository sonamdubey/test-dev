IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetBikeComparisonMin]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetBikeComparisonMin]
GO

	-- =============================================
-- Author:		Umesh Ojha
-- Create date: 24 Aug 2012
-- Description:	Get Top Lalest Records(Bike Comparison) For Camparison min controls 
-- =============================================
Create PROCEDURE [dbo].[GetBikeComparisonMin] 
	-- Add the parameters for the stored procedure here
	@TopCount SMALLINT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT Top (@TopCount)
	CL.ID as ID, CL.VersionId1 as VersionId1, CL.VersionId2 as VersionId2,
	CMA1.Name + ' ' + CMO1.Name + ' ' + CV1.Name as Bike1, CMA2.Name + ' ' + CMO2.Name + ' ' +  CV2.Name as Bike2 
	FROM Con_BikeComparisonList CL, BikeVersions AS CV1,  BikeModels AS CMO1,
	BikeMakes AS CMA1 , BikeVersions AS CV2,  BikeModels AS CMO2, BikeMakes AS CMA2 
	WHERE Cv1.ID = CL.VersionId1 AND CMO1.ID = Cv1.BikeModelId AND cMa1.ID = cMo1.BikeMakeId 
	AND Cv2.ID = CL.VersionId2 AND cMo2.ID = Cv2.BikeModelId AND cMa2.ID = cMo2.BikeMakeId AND CL.IsActive=1
	ORDER BY ID DESC  
	
END

