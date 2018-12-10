IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BW_SaveBikeAvailability]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BW_SaveBikeAvailability]
GO

	-- =============================================
-- Author:		Suresh Prajapati
-- Create date: 11th Nov, 2014
-- Description:	To Add Availability Days for Bike Booking
-- Modified By : Suresh Prajapati on 09th Jan, 2015
-- Summary : Added option of showing bike availability days equal to 0
-- Modified By : Suresh Prajapati on 27th Jan, 2015
-- Summary     : To make Add/Update Availability days bike version specific
-- =============================================
CREATE PROCEDURE [dbo].[BW_SaveBikeAvailability] @DealerId INT
	,@BikeVersionId INT
	,@NumOfDays INT
AS
BEGIN
	SET NOCOUNT OFF;

	IF EXISTS (
			SELECT ID
			FROM BW_BikeAvailability WITH (NOLOCK)
			WHERE BikeVersionId = @BikeVersionId
				AND DealerId = @DealerId
				AND IsActive = 1
			)
	BEGIN
		UPDATE BW_BikeAvailability
		SET NumOfDays = @NumOfDays
		WHERE BikeVersionId = @BikeVersionId
			AND DealerId = @DealerId
			AND IsActive = 1
	END
	ELSE
		IF EXISTS (
				SELECT ID
				FROM BW_BikeAvailability WITH (NOLOCK)
				WHERE BikeVersionId = @BikeVersionId
					AND DealerId = @DealerId
					AND IsActive = 0
				)
		BEGIN
			UPDATE BW_BikeAvailability
			SET IsActive = 1, NumOfDays=@NumOfDays
			WHERE BikeVersionId = @BikeVersionId
				AND DealerId = @DealerId
				AND IsActive = 0
		END
		ELSE
			INSERT INTO BW_BikeAvailability (
				DealerId
				,BikeVersionId
				,NumOfDays
				,IsActive
				)
			VALUES (
				@DealerId
				,@BikeVersionId
				,@NumOfDays
				,1
				)
END
