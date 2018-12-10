IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DeletePrices]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DeletePrices]
GO
-- =============================================
-- Author:		<Anuj Dhar>
-- Create date: <09/08/2016>
-- Description:	<Description: Deletes prices of specified categoryitemids corresponding to carversionid, cityid and ismetallic>
-- =============================================
CREATE PROCEDURE [dbo].[DeletePrices]
	-- Add the parameters for the stored procedure here
	@VersionId INT
	,@CityId INT
	,@IsMetallic INT
	,@CategoryItems VARCHAR(MAX)
	,@LastUpdated DATETIME
	,@UpdatedBy INT
AS
BEGIN
	DELETE NCP
	OUTPUT deleted.CarVersionId, deleted.CityId, deleted.isMetallic, deleted.PQ_CategoryItem, deleted.PQ_CategoryItemValue, deleted.LastUpdated, deleted.UpdatedBy, 'D'
	INTO PricesLog (VersionId, CityId, IsMetallic, PQ_CategoryItemId, PQ_CategoryItemValue, UpdatedOn, UpdatedBy, Status)
	FROM CW_NewCarShowroomPrices NCP WITH (NOLOCK)
		JOIN (SELECT items FROM [dbo].[SplitText](@CategoryItems,',')) AS CI ON CI.items = NCP.PQ_CategoryItem AND NCP.CarVersionId = @VersionId AND NCP.CityId = @CityId AND NCP.isMetallic = @IsMetallic

	IF EXISTS (SELECT items FROM [dbo].[SplitText](@CategoryItems,',') WHERE items = 2)
	BEGIN
		EXEC [dbo].[UpdateNewCarPricesColor]  @VersionId, @CityId, @IsMetallic
	END

	EXEC [dbo].[Con_SaveNewCarNationalPrices] @VersionId, @UpdatedBy, @LastUpdated

	IF (@CityId != 10)
	BEGIN
		EXEC [dbo].[UpdateModelPrices_v16_6_1]  @VersionId, @CityId, @UpdatedBy
	END
END
