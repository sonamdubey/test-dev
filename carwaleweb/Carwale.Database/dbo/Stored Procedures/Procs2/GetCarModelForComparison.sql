IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetCarModelForComparison]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetCarModelForComparison]
GO

	-- =============================================      
-- Author:  <Vikas J>
-- Create date: <27/01/2013>      
-- Description: <Returns the data of all the models available for the provided makeIds. It will have discontinued cars data along with new cars if>      
-- modified on 5 march 2014 by amit verma: added masking name column in select statement
-- =============================================      
CREATE PROCEDURE [dbo].[GetCarModelForComparison] 
	@MakeId1 INT = 0, --Make Id of first car selected by user
	@MakeId2 INT = 0, --Make Id of second car selected by user
	@MakeId3 INT = 0, --Make Id of third car selected by user
	@MakeId4 INT = 0, --Make Id of fourth car selected by user
	@OnlyNew INT 	  --Whether only new cars required or discontinued car needed as well
AS
BEGIN
	--Returns the data containing all the model available of makes provided as parameter. If any makeId is not provided the default value 0 will be
	--considered and no rows for that will be present in the returning dataset
	IF @OnlyNew = 1
		SELECT ID AS Value, Name AS [Text], MO.CarMakeId, MO.MaskingName 
		FROM CarModels MO WITH(NOLOCK)
		WHERE IsDeleted = 0 
		AND Futuristic = 0 
		AND  CarMakeId IN (@MakeId1,@MakeId2,@MakeId3,@MakeId4)
		AND New =1				 
		ORDER BY MO.CarMakeId, [Text]
		
	ELSE
	--Returns data even for discontinued models
		SELECT ID AS Value, (Mo.Name + ' ' + (CASE WHEN (SELECT CM.Name FROM CarModels CM WHERE Mo.Id=CM.Id AND CM.New=1) IS NULL THEN '*' ELSE '' END)) AS [Text], MO.CarMakeId, MO.MaskingName 
		FROM CarModels Mo WITH(NOLOCK)
		WHERE IsDeleted = 0 
		AND Futuristic = 0 
		AND  CarMakeId IN (@MakeId1,@MakeId2,@MakeId3,@MakeId4)
		ORDER BY Mo.New DESC, MO.CarMakeId, [Text]
	
END
