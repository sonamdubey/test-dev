IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AE_SaveCarVideos]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AE_SaveCarVideos]
GO

	

CREATE PROCEDURE [dbo].[AE_SaveCarVideos]
(
	@Id AS BIGINT =0,
	@CarId AS BIGINT,
	@VideoLocation AS VARCHAR(150),
	@IsMainVideo AS BIT,
	@Caption AS VARCHAR(150),
	@UpdatedOn AS DATETIME,
	@UpdatedBy AS SMALLINT,
	@Operation AS SMALLINT
)
AS
BEGIN

	IF EXISTS(SELECT CarId FROM AE_CarVideos WHERE IsMainVideo = @IsMainVideo)
	BEGIN
		IF(@IsMainVideo = 1)
		BEGIN
				UPDATE AE_CarVideos 
				SET IsMainVideo = 0 , UpdatedOn = @UpdatedOn, UpdatedBy = @UpdatedBy
		END
	END	

	IF (@Operation = 1 )
	BEGIN
			INSERT INTO 
			AE_CarVideos 
			(
				CarId, VideoLocation, IsMainVideo, Caption, UpdatedOn, UpdatedBy
			)
			Values
			(
				@CarId , @VideoLocation, @IsMainVideo, @Caption, @UpdatedOn, @UpdatedBy
			)
			--Updatating details to cardetails table
			UPDATE AE_CarDetails SET VideoAvailable = 1 WHERE CarId = @CarId
			
	END
	
	IF (@Operation = 2 )
	BEGIN
		UPDATE AE_CarVideos 
		SET Caption = @Caption, IsMainVideo = @IsMainVideo , UpdatedOn = @UpdatedOn, UpdatedBy = @UpdatedBy
		WHERE Id = @Id
	END		
END




