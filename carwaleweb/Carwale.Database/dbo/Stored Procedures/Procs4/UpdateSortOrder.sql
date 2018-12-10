IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UpdateSortOrder]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UpdateSortOrder]
GO

	-- =============================================
-- Author:		<Anuj Dhar>
-- Create date: <15-09-2016>
-- Description:	<To update the sort order for model-city combination in opr>
-- =============================================
create PROCEDURE [dbo].[UpdateSortOrder]
	@ModelId INT,
	@CityId INT,
	@SortOrder BIT
AS
DECLARE @Id INT
BEGIN
	SET  @Id = 0
	SELECT @Id = Id FROM ModelCitySortOrder WITH (NOLOCK) WHERE ModelId = @ModelId AND CityId = @CityId

	IF (@Id = 0)
	BEGIN
		INSERT INTO ModelCitySortOrder (ModelId, CityId, ToggleFlag)
								VALUES (@ModelId, @CityId, @SortOrder)
	END
	ELSE
	BEGIN
		UPDATE ModelCitySortOrder 
		SET ToggleFlag = @SortOrder
		WHERE Id = @Id
	END
END
