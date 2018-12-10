IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetCarModels]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetCarModels]
GO

	

-- Author:		Surendra
-- Create date: 20 Jan 2012
-- Description:	TC_GetCarModels 2
--				This Procedure will return model tabele base on Make
-- =============================================
CREATE PROCEDURE [dbo].[TC_GetCarModels] 
@MakeId INT
AS
BEGIN
	SELECT ID,Name FROM CarModels 
	WHERE CarMakeId=@MakeId AND IsDeleted = 0 ORDER BY Name
END



