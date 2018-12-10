IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AbSure_SaveWarrantyActivation]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AbSure_SaveWarrantyActivation]
GO

	-- =============================================
-- Author      : Yuga Hatolkar
-- Create date : 23rd Oct, 2015
-- Description : To Save warranty Activation Status.
-- Modified By : Chetan Navin - Mar 1 2016 (To handle logging of cartrade warranty)
-- =============================================
CREATE PROCEDURE [dbo].[AbSure_SaveWarrantyActivation] 
	@AbsureCarId		BIGINT			= NULL,
	@ActivationStatus	INT				= NULL,
	@UserId				INT				= NULL,
	@RejectionComments	VARCHAR(200)	= NULL,
	@CustName			VARCHAR(150)	= NULL,
	@Mobile				VARCHAR(20)		= NULL,
	@Email				VARCHAR(50)		= NULL,
	@Address			VARCHAR(500)	= NULL,
	@Model				VARCHAR(50)		= NULL,
	@AlternatePhone		VARCHAR(20)		= NULL,
	@MakeYear			DATETIME		= NULL,
	@RegNo				VARCHAR(50)		= NULL,
	@RegDate			DATETIME		= NULL,
	@Kilometer			BIGINT			= NULL,
	@WarrantyStartDate	DATETIME		= NULL,
	@WarrantyEndDate	DATETIME		= NULL,
	@DealerId			INT				= NULL,	
	@WarrantyTypeId		INT				= NULL,
	@EngineNo			VARCHAR(50)		= NULL,
	@ChassisNo			VARCHAR(50)		= NULL

AS
BEGIN
DECLARE @EntryDate DATETIME,@IsActivated INT,@ActivationDate DATETIME,@TC_ActionApplicationId INT,@ActivatedBy VARCHAR(50),@OriginalStockId INT, @OrigCarId INT=NULL 
	SET NOCOUNT ON;	
	SELECT @OrigCarId = dbo.Absure_GetMasterCarId(@RegNo,@AbsureCarId)
	IF @ActivationStatus = 3
	BEGIN
		IF EXISTS(SELECT 1 FROM AbSure_WarrantyActivationStatusesLog WITH(NOLOCK) WHERE AbSureCarDetailsId = @OrigCarId AND IsActive = 1 AND ISNULL(IsCarTradeWarranty,0) <> 1)
		BEGIN
			UPDATE AbSure_WarrantyActivationStatusesLog SET IsActive = 0 WHERE AbSureCarDetailsId = @OrigCarId
			
			INSERT INTO AbSure_WarrantyActivationStatusesLog (AbSure_WarrantyActivationStatusesId, AbSureCarDetailsId, UserId, EntryDate, RejectionReason, IsActive)
			VALUES (@ActivationStatus, @OrigCarId, @UserId, GETDATE(), @RejectionComments, 1)

			UPDATE AbSure_CarDetails SET AbSureWarrantyActivationStatusesId = 3 WHERE Id = @OrigCarId
		END
		ELSE
		BEGIN
			UPDATE AbSure_WarrantyActivationStatusesLog SET IsActive = 0 WHERE AbSureCarDetailsId = @OrigCarId AND IsCarTradeWarranty = 1
			
			INSERT INTO AbSure_WarrantyActivationStatusesLog (AbSure_WarrantyActivationStatusesId, AbSureCarDetailsId, UserId, EntryDate, RejectionReason, IsActive,IsCarTradeWarranty)
			VALUES (@ActivationStatus, @OrigCarId, @UserId, GETDATE(), @RejectionComments, 1,1)

			UPDATE TC_CarTradeCertificationRequests SET CertificationStatus = 3,CertificationStatusDesc = 'Rejected after verification' 
			WHERE TC_CarTradeCertificationRequestId = @OrigCarId
		END
	END	
		
	ELSE
	BEGIN
		IF EXISTS(SELECT 1 FROM AbSure_WarrantyActivationStatusesLog WITH(NOLOCK) WHERE AbSureCarDetailsId = @OrigCarId AND IsActive = 1 AND ISNULL(IsCarTradeWarranty,0) <> 1)
		BEGIN
			UPDATE AbSure_WarrantyActivationStatusesLog SET IsActive = 0 WHERE AbSureCarDetailsId = @OrigCarId
				
			INSERT INTO AbSure_WarrantyActivationStatusesLog (AbSure_WarrantyActivationStatusesId, AbSureCarDetailsId, UserId, EntryDate, RejectionReason, IsActive,IsCarTradeWarranty)
			VALUES (@ActivationStatus, @OrigCarId, @UserId, GETDATE(), NULL, 1,1)
		END
		ELSE
		BEGIN
			UPDATE AbSure_WarrantyActivationStatusesLog SET IsActive = 0 WHERE AbSureCarDetailsId = @OrigCarId AND IsCarTradeWarranty = 1
				
			INSERT INTO AbSure_WarrantyActivationStatusesLog (AbSure_WarrantyActivationStatusesId, AbSureCarDetailsId, UserId, EntryDate, RejectionReason, IsActive,IsCarTradeWarranty)
			VALUES (@ActivationStatus, @OrigCarId, @UserId, GETDATE(), NULL, 1,1)
		END
	END	
END

---------------------------------------------------------------------------------------------------------------------------
