IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_NewSaveVerificationOtherDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_NewSaveVerificationOtherDetails]
GO

	-- =============================================
-- Author:		Vaibhav K
-- Create date: 20-11-2013
-- Description:	Save verification other details from new crm flow
-- Modifier:	Vaibhav K 5-2-2014
--				Update CRM_Calls on insert also
--				Chetan Navin 04-11-2014
--				Added purchase mode and IsHDFC in insert statement
-- =============================================
CREATE PROCEDURE [dbo].[CRM_NewSaveVerificationOtherDetails]
	-- Add the parameters for the stored procedure here
	@Type					INT,
	@LeadId					BIGINT,
	@CallId					NUMERIC(18, 0) = -1,
	@CallConnected			BIT = NULL,
	@GoodTimeToTalk			BIT = NULL,
	@Language				VARCHAR(15) = NULL,
	@LookingForCar			BIT = NULL,
	@IsSameMake				BIT = NULL,
	@BuyingSpan				INT = NULL,
	@PurchaseMode			INT	= NULL,
	@Eagerness				INT = NULL,
	@NotConnectedReason		INT = NULL,
	@CallBackRequest		BIT = NULL,
	@IsPriorityCall			BIT = NULL,
	@UnavailabilityReason	INT = NULL,
	@NotIntReason			INT = NULL,
	@IsCarBookedAlready		BIT = NULL,
	@BookedCarVersion		INT = NULL,
	@BookedCarDate			DATE = NULL,
	@BookedCarProblem		VARCHAR(50) = NULL,
	@IsFuturePlanToBuy		BIT = NULL,
	@FuturePurchaseDate		DATE = NULL,
	@SpecialComments		VARCHAR(5000) = NULL,
	@UpdatedBy				BIGINT = -1,
	@IsHDFC					BIT = NULL

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	DECLARE @VerificationId BIGINT = -1

	SELECT @VerificationId = Id FROM CRM_VerificationOthersLog WITH (NOLOCK) WHERE LeadId = @LeadId
	
	IF @VerificationId = -1
		BEGIN
			INSERT INTO CRM_VerificationOthersLog 
				( LeadId, CallConnected, GoodTimeToTalk, Language, LookingForCar, IsSameMake, BuyingSpan,
				  NotConnectedReason, UnavailabilityReason, CallBackRequest, NotIntReason,
				  IsCarBookedAlready, BookedCarVersion, BookedCarDate, SpecialComments, UpdatedOn, UpdatedBy,PurchaseMode,IsHDFC
				)
			VALUES
				( @LeadId, @CallConnected, @GoodTimeToTalk, @Language, @LookingForCar, @IsSameMake, @BuyingSpan,
				  @NotConnectedReason, @UnavailabilityReason, @CallBackRequest, @NotIntReason,
				  @IsCarBookedAlready, @BookedCarVersion, @BookedCarDate, @SpecialComments, GETDATE(), @UpdatedBy,@PurchaseMode,@IsHDFC
				)

			--Update CRM_Calls about the current call status
			UPDATE CRM_Calls
				SET DispType = CASE @CallConnected WHEN 1 THEN 1 ELSE 2 END
			WHERE Id = @CallId
		END
	ELSE
		BEGIN
			IF @Type = 1
				BEGIN
					UPDATE CRM_VerificationOthersLog
						SET CallConnected = @CallConnected,
							UpdatedOn = GETDATE(),
							UpdatedBy = @UpdatedBy
					WHERE Id = @VerificationId

					UPDATE CRM_Calls
						SET DispType = CASE @CallConnected WHEN 1 THEN 1 ELSE 2 END
					WHERE Id = @CallId
				END
			ELSE IF @Type = 2
				BEGIN
					UPDATE CRM_VerificationOthersLog
						SET GoodTimeToTalk = @GoodTimeToTalk,
							UpdatedOn = GETDATE(),
							UpdatedBy = @UpdatedBy
					WHERE Id = @VerificationId

					UPDATE CRM_Calls
						SET RightTime = @GoodTimeToTalk
					WHERE Id = @CallId
				END
			ELSE IF @Type = 3
				BEGIN
					UPDATE CRM_VerificationOthersLog
						SET Language = @Language,
							UpdatedOn = GETDATE(),
							UpdatedBy = @UpdatedBy
					WHERE Id = @VerificationId
				END
			ELSE IF @Type = 4
				BEGIN
					UPDATE CRM_VerificationOthersLog
						SET LookingForCar = @LookingForCar,
							UpdatedOn = GETDATE(),
							UpdatedBy = @UpdatedBy
					WHERE Id = @VerificationId
				END
			ELSE IF @Type = 5
				BEGIN
					UPDATE CRM_VerificationOthersLog
						SET IsSameMake = @IsSameMake,
							UpdatedOn = GETDATE(),
							UpdatedBy = @UpdatedBy
					WHERE Id = @VerificationId
				END
			ELSE IF @Type = 6
				BEGIN
					UPDATE CRM_VerificationOthersLog
						SET BuyingSpan = @BuyingSpan,
							UpdatedOn = GETDATE(),
							UpdatedBy = @UpdatedBy
					WHERE Id = @VerificationId
					
					UPDATE CRM_InterestedIn SET ClosingDate = GETDATE() + @BuyingSpan
					WHERE LeadId = @LeadId
				END
			ELSE IF @Type = 7
				BEGIN
					UPDATE CRM_VerificationOthersLog
						SET NotConnectedReason = @NotConnectedReason,
							UpdatedOn = GETDATE(),
							UpdatedBy = @UpdatedBy
					WHERE Id = @VerificationId
					
					-- If not the fake lead AND not invalid number then update CRM_Calls with the DispType
					IF @NotConnectedReason <> 8 AND @NotConnectedReason <> 76
						BEGIN
							UPDATE CRM_Calls
								SET DispType = @NotConnectedReason
							WHERE Id = @CallId
						END
					ELSE
						BEGIN
							
							UPDATE CRM_Calls
								SET DispType = 2
							WHERE Id = @CallId
						END

					--IF @NotConnectedReason = 3
					--	BEGIN
					--		UPDATE CRM_VerificationOthersLog
					--			SET CallBackRequest = 1 
					--		WHERE Id = @VerificationId
					--	END
					--ELSE
					--	BEGIN
					--		UPDATE CRM_VerificationOthersLog
					--			SET CallBackRequest = 0 
					--		WHERE Id = @VerificationId
					--	END
				END
			ELSE IF @Type = 8
				BEGIN
					UPDATE CRM_VerificationOthersLog
						SET IsPriorityCall = @IsPriorityCall,
							UpdatedOn = GETDATE(),
							UpdatedBy = @UpdatedBy
					WHERE Id = @VerificationId
					UPDATE CRM_Calls SET IsPriorityCall = @IsPriorityCall WHERE Id = @CallId
				END
			ELSE IF @Type = 9
				BEGIN
					UPDATE CRM_VerificationOthersLog
						SET UnavailabilityReason = @UnavailabilityReason,
							UpdatedOn = GETDATE(),
							UpdatedBy = @UpdatedBy
					WHERE Id = @VerificationId
				END
			ELSE IF @Type = 10
				BEGIN
					UPDATE CRM_VerificationOthersLog
						SET NotIntReason = @NotIntReason,
							UpdatedOn = GETDATE(),
							UpdatedBy = @UpdatedBy
					WHERE Id = @VerificationId
				END
			ELSE IF @Type = 11
				BEGIN
					UPDATE CRM_VerificationOthersLog
						SET IsCarBookedAlready = @IsCarBookedAlready,
							UpdatedOn = GETDATE(),
							UpdatedBy = @UpdatedBy
					WHERE Id = @VerificationId
				END
			ELSE IF @Type = 12
				BEGIN
					UPDATE CRM_VerificationOthersLog
						SET BookedCarVersion = @BookedCarVersion,
							BookedCarDate = @BookedCarDate,
							UpdatedOn = GETDATE(),
							UpdatedBy = @UpdatedBy
					WHERE Id = @VerificationId
				END
			ELSE IF @Type = 13
				BEGIN
					UPDATE CRM_VerificationOthersLog
						SET BookedCarProblem = @BookedCarProblem,
							UpdatedOn = GETDATE(),
							UpdatedBy = @UpdatedBy
					WHERE Id = @VerificationId
				END
			ELSE IF @Type = 14
				BEGIN
					UPDATE CRM_VerificationOthersLog
						SET IsFuturePlanToBuy = @IsFuturePlanToBuy,
							UpdatedOn = GETDATE(),
							UpdatedBy = @UpdatedBy
					WHERE Id = @VerificationId
				END
			ELSE IF @Type = 15
				BEGIN
					UPDATE CRM_VerificationOthersLog
						SET FuturePurchaseDate = @FuturePurchaseDate,
							UpdatedOn = GETDATE(),
							UpdatedBy = @UpdatedBy
					WHERE Id = @VerificationId
				END
			ELSE IF @Type = 16
				BEGIN
					UPDATE CRM_VerificationOthersLog
						SET SpecialComments = @SpecialComments,
							UpdatedOn = GETDATE(),
							UpdatedBy = @UpdatedBy
					WHERE Id = @VerificationId
				END
			ELSE IF @Type = 17
				BEGIN
					UPDATE CRM_VerificationOthersLog
						SET PurchaseMode = @PurchaseMode,
							UpdatedOn = GETDATE(),
							UpdatedBy = @UpdatedBy
					WHERE Id = @VerificationId
				END
			ELSE IF @Type = 18
				BEGIN
					UPDATE CRM_VerificationOthersLog
						SET Eagerness = @Eagerness,
							UpdatedOn = GETDATE(),
							UpdatedBy = @UpdatedBy
					WHERE Id = @VerificationId

					UPDATE CRM_InterestedIn SET ClosingProbability = @Eagerness 
					WHERE LeadId = @LeadId
				END
			ELSE IF @Type = 19
				BEGIN
					UPDATE CRM_VerificationOthersLog
						SET IsHDFC = @IsHDFC,
							UpdatedOn = GETDATE(),
							UpdatedBy = @UpdatedBy
					WHERE Id = @VerificationId
				END
		END
END

