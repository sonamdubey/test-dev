IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AbSure_GetCarFuelType]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AbSure_GetCarFuelType]
GO

	

-- ===============================================================================================================================
-- Author:		Ashwini Dhamankar
-- Create date: 31st july 2015
-- Description:	To fetch Car FuelType 
-- ===============================================================================================================================
CREATE  PROCEDURE [dbo].[AbSure_GetCarFuelType]	
AS
BEGIN
      SELECT CFT.CarFuelTypeId AS Id , CFT.Descr AS Name  FROM  CarFuelTypes AS CFT WITH(NOLOCK)
END

