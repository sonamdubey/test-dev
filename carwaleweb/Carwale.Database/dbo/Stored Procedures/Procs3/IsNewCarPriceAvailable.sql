IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[IsNewCarPriceAvailable]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[IsNewCarPriceAvailable]
GO

	-- =============================================      
-- Author:  
-- Create date: 
-- Description: 
-- DECLARE @Bit BIT EXEC IsNewCarPriceAvailable 263, 1,@Bit OUTPUT SELECT @Bit 
-- =============================================      
CREATE PROCEDURE [dbo].[IsNewCarPriceAvailable] 
	@ModelId INT = 0, --Model Id of car for which ex showroom price needed
	@CityId  INT = 0, --city Id in which price is needed
	@Bit BIT = 0 OUTPUT
AS
BEGIN IF EXISTS(
	SELECT NCP.Id
	FROM CarVersions AS CV WITH(NOLOCK) 
	INNER JOIN NewCarShowroomPrices NCP WITH(NOLOCK) ON NCP.CarVersionId=CV.ID 
	WHERE CV.IsDeleted=0 
	AND CV.CarModelId=@ModelId
    AND NCP.CityId=@CityId )
    SET @Bit=1
	ELSE
	SET @Bit=0
END
