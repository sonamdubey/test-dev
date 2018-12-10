IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Con_SaveSimilarCar]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Con_SaveSimilarCar]
GO

	CREATE Procedure [dbo].[Con_SaveSimilarCar]
	@ID					NUMERIC,
	@VersionId			NUMERIC,
	@SimilarVersions	VARCHAR(8000),
	@UpdatedOn			DATETIME,
	@Status				BIT OUTPUT

AS
	
BEGIN
	SET @Status = 0
	IF @ID = -1
		BEGIN
			INSERT INTO Similarcars
			(
				VersionId, SimilarVersions, UpdatedOn, IsActive
			) 
			VALUES 
			(
				@VersionId, @SimilarVersions, @UpdatedOn, 1
			)
		
			SET @Status = 1
		END

	ELSE

		BEGIN
			UPDATE Similarcars
			SET SimilarVersions = @SimilarVersions, UpdatedOn = @UpdatedOn
			WHERE ID = @ID AND VersionId = @VersionId

			SET @Status = 1 
		END
END


