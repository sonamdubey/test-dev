IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetCarVersions]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetCarVersions]
GO
	
-- Author:		Surendra
-- Create date: 20 Jan 2012
-- Description:	TC_GetCarModels 2
--				This Procedure will return model tabele base on Make
-- Satish 05-03-2012 Added CarFuelType and CarTransmission
-- =============================================
CREATE PROCEDURE [dbo].[TC_GetCarVersions]
@ModelId INT
AS
BEGIN
	SELECT ID,Name,CV.CarFuelType,CV.CarTransmission FROM CarVersions as CV
	WHERE CarModelId=@ModelId AND IsDeleted = 0 ORDER BY Name
END


