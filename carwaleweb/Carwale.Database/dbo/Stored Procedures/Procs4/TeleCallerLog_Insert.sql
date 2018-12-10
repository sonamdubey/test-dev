IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TeleCallerLog_Insert]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TeleCallerLog_Insert]
GO

	-- =============================================
-- Author	:	Sachin Bharti(9th Sep 2014)
-- Description	:	Insert telecaller login time in TeleCallerLog table
-- @CallerType:
--	1 - CRMTeleCaller 
--	2 - CHTeleCaller
--	3 - DCRMTeleCaller
-- =============================================
CREATE PROCEDURE [dbo].[TeleCallerLog_Insert]
	-- Add the parameters for the stored procedure here
	@UserId		INT,
	@Status		BIT OUTPUT,
	@TeleLoginId	INT OUTPUT,
	@IsCRMCaller	BIT OUTPUT,
	@IsCHCaller		BIT OUTPUT,
	@IsDCRMCaller	BIT OUTPUT
AS
BEGIN
	
	SET NOCOUNT ON;

	--Identify is user telecaller or not
	DECLARE @IsTeleCaller	BIT
	DECLARE	@CallerType		SMALLINT 

	SET @IsCRMCaller	=	0
	SET @IsCHCaller		=	0
	SET @IsDCRMCaller	=	0
	SET @Status			=	0
	SET @CallerType		=	0
	SET @IsTeleCaller	=	0
	
	--Check for CRMTeleCallers
	SELECT UserId FROM CRM_Users(NOLOCK) WHERE UserId = @UserId AND IsActive = 1
	IF @@ROWCOUNT <> 0 
	BEGIN
		SET @IsTeleCaller	= 1
		SET @CallerType		= 1
		SET @IsCRMCaller	= 1
	END


	--Check for CHTeleCallers 
	SELECT TCID FROM CH_TeleCallers(NOLOCK) WHERE TCID = @UserId AND IsActive = 1
	IF @@ROWCOUNT <> 0 
	BEGIN
		SET @IsTeleCaller	= 1 
		SET @CallerType		= 2
		SET @IsCHCaller		= 1
	END

	
	--Check for DCRMTeleCallers
	SELECT UserId FROM DCRM_ADM_Users(NOLOCK) WHERE UserId = @UserId AND IsActive = 1
	IF @@ROWCOUNT <> 0 
	BEGIN
		SET @IsTeleCaller	= 1
		SET @CallerType		= 3
		SET @IsDCRMCaller	= 1
	END

	
	--Now log login time of telecallers
	IF @IsTeleCaller = 1 AND @CallerType <> 0
		BEGIN
			INSERT INTO TeleCallerLog (UserID, LoginTime, Type)
			VALUES (@UserId,GETDATE(),@CallerType)
			SET @TeleLoginId = SCOPE_IDENTITY() 
			SET @Status = 1 
			
			IF @IsCRMCaller = 1
			BEGIN
				UPDATE CRM_ADM_PEConsultantRules SET IsActiveLogin = 1 WHERE ConsultantId = @UserId
			END

			IF @IsCHCaller = 2
			BEGIN
				UPDATE CH_TeleCallers SET LastLeadTime = GETDATE(), IsNew = 0, IsActiveLogin = 1
				WHERE TCID = @UserId AND TCID IN(SELECT C.TCID FROM CH_TCAssignedTasks AS C WHERE C.TCID = @UserId AND C.TBCType = 2 AND C.CallType = 1)
			END
			
		END
END
