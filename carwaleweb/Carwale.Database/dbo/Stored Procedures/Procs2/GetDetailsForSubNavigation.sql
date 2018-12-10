IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetDetailsForSubNavigation]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetDetailsForSubNavigation]
GO

	

--writtenby Natesh kumar for getting bool values or the need in subnavigation
-- written on 13/10/14
CREATE procedure [dbo].[GetDetailsForSubNavigation]
@ModelId Int

AS
 
 BEGIN
  SELECT Mo.Discontinuation_date, mo.IsDeleted,Mo.Futuristic, mo.New, mo.ReviewCount , mo.Used,mo.VideoCount, mo.UsedCarRating  
  FROM CarModels Mo
	INNER JOIN CarMakes Mk ON Mk.ID = Mo.CarMakeId
	WHERE Mo.CarMakeId = Mk.Id
		AND Mo.ID = @ModelId
 END
