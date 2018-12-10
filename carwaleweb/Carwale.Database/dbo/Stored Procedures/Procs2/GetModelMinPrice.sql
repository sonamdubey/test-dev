IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetModelMinPrice]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetModelMinPrice]
GO

	CREATE PROCEDURE GetModelMinPrice 
(@ModelId VARCHAR(100))
AS
BEGIN
DECLARE @iModelId VARCHAR(100)
SELECT @iModelId= CASE WHEN LEN(@ModelId)>0 THEN @ModelId ELSE 0 END

SELECT Top 1 MinPrice 
FROM Con_NewCarNationalPrices Ncp WITH(NOLOCK) , CarVersions Vs WITH(NOLOCK) 
WHERE Vs.ID = Ncp.VersionId 
AND Vs.IsDeleted = 0 
AND Ncp.IsActive = 1 
AND Vs.CarModelId = @iModelId  
AND Vs.New = 1

END
