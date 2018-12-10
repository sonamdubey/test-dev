IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Con_SaveIHWData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Con_SaveIHWData]
GO

	CREATE Procedure [dbo].[Con_SaveIHWData]
	@Id					NUMERIC,
	@IHId				NUMERIC,
	@IHData				VARCHAR(500),
	@IHDataURL			VARCHAR(200),
	@Status				SMALLINT OUTPUT
AS
	
BEGIN
	SET @Status = 0
	IF @Id = -1

		BEGIN
			INSERT INTO Con_IHWeeklyData
			(
				IHId, IHData, IHDataURL
			) 
			VALUES 
			(
				@IHId, @IHData, @IHDataURL
			)
			SET @Status = 1
		END
	
	ELSE

		BEGIN
			UPDATE Con_IHWeeklyData 
			SET IHData = @IHData, IHDataURL = @IHDataURL
			WHERE Id = @Id

			SET @Status = 1
		END	
END

