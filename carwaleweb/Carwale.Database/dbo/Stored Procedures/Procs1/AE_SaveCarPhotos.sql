IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AE_SaveCarPhotos]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AE_SaveCarPhotos]
GO

	
CREATE PROCEDURE [dbo].[AE_SaveCarPhotos]
(
	@Id AS BIGINT =0,
	@CarId AS BIGINT =0,
	@IsMainPhoto AS BIT =0,
	@Caption AS VARCHAR(150) =NULL,
	@UpdatedOn AS DATETIME,
	@UpdatedBy AS SMALLINT,
	@Operation AS SMALLINT,
	@RetId		AS NUMERIC OUTPUT
)
AS
	DECLARE @PId AS NUMERIC
BEGIN
	IF(@IsMainPhoto = 1)
	BEGIN
		SELECT @PId=Id FROM AE_CarPhotos WHERE IsMainPhoto = 1 AND CarId = @CarId
		IF @@ROWCOUNT <> 0
		BEGIN
			UPDATE AE_CarPhotos 
			SET IsMainPhoto = 0 , UpdatedOn = @UpdatedOn, UpdatedBy = @UpdatedBy
			WHERE Id = @PId
		END
	END	

	IF (@Operation = 1 )
	BEGIN
			--Making an entry to database
			INSERT INTO AE_CarPhotos 
			(
				CarId, IsMainPhoto, Caption, UpdatedOn, UpdatedBy
			)
			Values
			(
				@CarId , @IsMainPhoto, @Caption, @UpdatedOn, @UpdatedBy
			)
			
			SET @RetId = SCOPE_IDENTITY()
			--Updatating details to cardetails table

			UPDATE AE_CarDetails SET PhotoAvailable = 1 WHERE CarId = @CarId
	END				

	IF (@Operation = 2 )
	BEGIN
		UPDATE AE_CarPhotos 
		SET Caption = @Caption, IsMainPhoto = @IsMainPhoto , UpdatedOn = @UpdatedOn, 
			UpdatedBy = @UpdatedBy
		WHERE Id = @Id
		
		SET @RetId = @Id
	END				

	
END
