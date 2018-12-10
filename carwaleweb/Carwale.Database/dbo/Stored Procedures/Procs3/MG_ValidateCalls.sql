IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[MG_ValidateCalls]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[MG_ValidateCalls]
GO

	CREATE Procedure [dbo].[MG_ValidateCalls]
	@CustomerId	NUMERIC,
	@CityId		NUMERIC,
	@IsValidated	BIT OUTPUT
AS			
	DECLARE @MG_CityId 	NUMERIC, 
		@CurrentVolume	VARCHAR(100)
BEGIN
	
	/* GET CURRENT AVAILABLE VOLUME OF THE MAGAZINE */
	SELECT @CurrentVolume = VolumeId FROM MG_Volumes WHERE CurrentVolume = 1
	IF @@RowCount <> 0
		BEGIN
			SET @IsValidated = 1		
	
			/* CHECK FOR CUSTOMER CITY, IT SHOULD BE FROM MUMBAI&AROUND, DELHI&NCR, BANGALORE ONLY */
			--IF @CityId Not IN(1,6,8,13,40,278,361,395,10,224,225,246,273,313,2) SET @IsValidated = 0
			
			/* CHECK IF CUSTOMER IS BLOCKED FOR Magazine OR CUSTOMER IS NOT INTERSTED TO BUY */
			SELECT CustomerId FROM MG_BlockedCustomers WHERE CustomerId = @CustomerId
			IF @@ROWCOUNT <> 0 SET @IsValidated = 0
		
			/* CHECK IF CUSTOMER IS ALREADY REQUESTED FOR CURRENT VOLUME */
			IF @IsValidated = 1
			BEGIN
				SELECT ID FROM MG_Requests WHERE CustomerId = @CustomerId AND VolumeId = @CurrentVolume
				IF @@ROWCOUNT <> 0 SET @IsValidated = 0
			END
			
			/* CHECK IF CALL ALREADY SCHEDULED FOR CURRENT VOLUME */
			IF @IsValidated = 1
			BEGIN
				SELECT CustomerId FROM MG_Scheduled WHERE CustomerId = @CustomerId AND VolumeId = @CurrentVolume
				IF @@ROWCOUNT <> 0 SET @IsValidated = 0
			END
		
			/* 	IF STILL CALL NOT SCHEDULED THEN MAKE ENTRY TO THE 
				TABLE MG_SCHEDULED BY ASUMING THE CALL IS SCHEDULED 
			*/
			IF @IsValidated = 1
			BEGIN
				INSERT INTO MG_Scheduled(CustomerId, VolumeId) VALUES(@CustomerId, @CurrentVolume)
			END
		END
	ELSE
		BEGIN
			SET @IsValidated = 0
		END
END