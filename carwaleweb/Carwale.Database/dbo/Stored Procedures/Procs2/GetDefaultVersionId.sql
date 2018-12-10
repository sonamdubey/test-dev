IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetDefaultVersionId]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetDefaultVersionId]
GO

	-- =============================================
-- Author:		<Sanjay Soni>
-- Create date: <14/07/2015>
-- Description:	<Description:For getting default Version Id on basis of minimum price for that model>
-- Modified By Sourav Roy on 9/3/2015: Added If condition If VersionId is Null
-- Modified By Sourav Roy on 9/24/2015: Added IsDeleted flag condition 
-- Modified : Vicky Lund, 25/08/2016, Added IsDeleted and New flags
-- =============================================
CREATE PROCEDURE [dbo].[GetDefaultVersionId] @ModelId INT
	,@CityId INT
	,@VersionId INT OUTPUT
AS
BEGIN
	SELECT TOP (1) @VersionId = carVersionId
	FROM CW_NewCarShowroomPrices AS PN WITH (NOLOCK)
	JOIN CarVersions AS V WITH (NOLOCK) ON PN.CarVersionId = V.ID
	WHERE V.CarModelId = @ModelId
		AND V.New = 1
		AND PN.PQ_CategoryItem = 2
		AND PN.CityId = @CityId
		AND V.IsDeleted = 0 --Added IsDeleted flag condition 
	ORDER BY PN.PQ_CategoryItemValue

	--Added If condition If VersionId is Null
	IF @VersionId IS NULL
		SELECT TOP (1) @VersionId = Id
		FROM CarVersions WITH (NOLOCK)
		WHERE CarModelId = @ModelId
			AND IsDeleted = 0
			AND New = 1
END
