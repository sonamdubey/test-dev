IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetUpcomingCarsList]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetUpcomingCarsList]
GO

	-- =============================================
-- Author:			Vikas
-- Create date:	12/11/2012
-- Description:		Get the list of all upcoming cars
-- =============================================
CREATE PROCEDURE GetUpcomingCarsList 

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here	
	SELECT  
		ECL.Id As ExpectedLaunchId,
		MK.Name MakeName, 
		Mo.Name AS ModelName, 
		(MK.Name + ' ' + MO.Name) As CarName,
		ECL.ExpectedLaunch, 
		ECL.EstimatedPriceMin, 
		ECL.EstimatedPriceMax, 
		Mo.HostUrl, 
		Mo.LargePic, 
		Csy.SmallDescription AS Description, 
		dbo.GetDateDescr(UpdatedDate) As UpdatedDate, 
		Case When ECL.CWConfidence = 1 Then 'Low' When ECL.CWConfidence = 2 Then 'Low' When ECL.CWConfidence = 3 Then 'Medium' When ECL.CWConfidence = 4 Then 'High' When ECL.CWConfidence = 5 Then 'High' When ECL.CWConfidence IS NULL Then 'NA' End As CWConfidenceText, 
		Case When ECL.CWConfidence = 1 Then 'cwc_low' When ECL.CWConfidence = 2 Then 'cwc_low' When ECL.CWConfidence = 3 Then 'cwc_medium' When ECL.CWConfidence = 4 Then 'cwc_high' When ECL.CWConfidence = 5 Then 'cwc_high' When ECL.CWConfidence IS NULL Then '' End As CWConfidenceCSS
	FROM 
		ExpectedCarLaunches ECL 
		LEFT JOIN CarSynopsis Csy ON ECL.CarModelId = Csy.ModelId AND Csy.IsActive = 1 
		INNER JOIN CarModels Mo ON ECL.CarModelId = Mo.ID 
		INNER JOIN CarMakes MK ON MK.ID = Mo.CarMakeId
	WHERE
		Mo.Futuristic = 1 AND ECL.isLaunched = 0
	ORDER BY 
		ECL.LaunchDate DESC	
		
END

