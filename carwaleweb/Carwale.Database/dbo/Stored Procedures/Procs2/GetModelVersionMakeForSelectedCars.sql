IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetModelVersionMakeForSelectedCars]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetModelVersionMakeForSelectedCars]
GO

	
-- =============================================      
-- Author:  <Vikas J>
-- Create date: <27/01/2013>      
-- Description: <Returns the data (make, model and version) of all the the carIds(ie. versionIds) provided. It will have discontinued cars data along with new cars if user chooses for all cars.      
-- =============================================    
CREATE PROCEDURE [dbo].[GetModelVersionMakeForSelectedCars]
@VersionId1 INT = 0,--Version Id of first car selected by user
@VersionId2 INT = 0,--Version Id of second car selected by user
@VersionId3 INT = 0,--Version Id of third car selected by user
@VersionId4 INT = 0 --Version Id of fourth car selected by user

AS
BEGIN
--Returns the data (make, model and version) for cars selected by user for comparision
SELECT VE.ID Version, MO.ID Model, MA.ID Make 
FROM CarMakes MA WITH(NOLOCK)
INNER JOIN CarModels MO WITH(NOLOCK) ON MO.CarMakeId=MA.ID 
INNER JOIN CarVersions VE WITH(NOLOCK) ON VE.CarModelId=MO.Id 
AND VE.ID IN (@VersionId1,@VersionId2,@VersionId3,@VersionId4);

END
