IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_SaveIndividualTargets]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_SaveIndividualTargets]
GO

	
-- =============================================
-- Author:		Ruchira Patil
-- Create date: 24th July 2014
-- Description:	to save and update the per day individual targets of leads processed and leads assigned
-- =============================================
CREATE PROCEDURE [dbo].[CRM_SaveIndividualTargets]
	--@Id				INT, 
	@Date			DATETIME,
	@Brand			INT,
	@ConsultantName	VARCHAR(MAX),
	@LeadProcessed	INT,
	@LeadAssigned	INT,
	@CreatedBy		INT,
	@Status			BIT OUTPUT
	--,@IsUpdated		BIT OUTPUT
AS
BEGIN

	DECLARE @TempUserTable TABLE
	(
		ID INT IDENTITY(1,1),
		UserId INT
	)
	DECLARE @UserCounter	INT,
			@i				TINYINT,
			@TempUser		INT,
			@TempUpdatedOn	DATETIME,
			@CurrentBrand	INT,
			@LastBrand		INT
	
	INSERT INTO @TempUserTable(UserId)
	SELECT ListMember FROM fnSplitCSV(@ConsultantName)

	SELECT @UserCounter = COUNT(*) FROM @TempUserTable

	SET @i = 1
	WHILE @UserCounter > 0
	BEGIN
		SELECT @TempUser = UserId FROM @TempUserTable WHERE ID= @i
		SET @i = @i + 1
		SET @UserCounter = @UserCounter - 1

		--INSERTION AND UPDATION IN CRM_IndividualTarget
		--Updation for LeadProcessed and its value
		UPDATE CRM_IndividualTarget
		SET Value = ISNULL(@LeadProcessed,Value),
			UpdatedBy = @CreatedBy,
			UpdatedOn =  GETDATE()
		WHERE UserId = @TempUser AND Type = 4 AND Date = @Date

		--Insertion for LeadProcessed
		IF @@ROWCOUNT = 0
		BEGIN
			INSERT INTO CRM_IndividualTarget(UserId,Brand,Type,Date,Value,CreatedBy)
			VALUES (@TempUser,@Brand,4,@Date,@LeadProcessed,@CreatedBy)
		END

		--Updation for LeadAssigned and its value
		UPDATE CRM_IndividualTarget
		SET Value = ISNULL(@LeadAssigned,Value),
			UpdatedBy = @CreatedBy,
			UpdatedOn =  GETDATE()
		WHERE UserId = @TempUser AND Type = 5 AND Date = @Date

		--Insertion for LeadAssigned
		IF @@ROWCOUNT = 0
		BEGIN
			INSERT INTO CRM_IndividualTarget(UserId,Brand,Type,Date,Value,CreatedBy)
			VALUES (@TempUser,@Brand,5,@Date,@LeadAssigned,@CreatedBy)
		END

		--INSERTION AND UPDATION IN CRM_TargetLog Only allowed for current month
		IF MONTH(@Date) = MONTH(GETDATE()) AND YEAR(@Date) = YEAR(GETDATE())
		BEGIN
			--Updation for LeadProcessed and its value
			UPDATE CRM_TargetLog
			SET Value = ISNULL(@LeadProcessed,Value),
				ActionTakenBy=@CreatedBy,
				ActionTakenOn=GETDATE() 
			WHERE UserId = @TempUser AND Type = 4 AND Date = CAST(GETDATE() AS DATE) AND IsDeleted=0

			--Insertion for LeadProcessed
			IF @@ROWCOUNT = 0
			BEGIN
				INSERT INTO CRM_TargetLog(UserId,Brand,Type,Date,Value,ActionTakenBy,ActionTakenOn)
				VALUES (@TempUser,@Brand,4,GETDATE(),@LeadProcessed,@CreatedBy,GETDATE())
			END

			--Updation for LeadAssigned and its value
			UPDATE CRM_TargetLog
			SET Value = ISNULL(@LeadAssigned,Value),
				ActionTakenBy=@CreatedBy,
				ActionTakenOn=GETDATE()
			WHERE UserId = @TempUser AND Type = 5 AND Date = CAST(GETDATE() AS DATE) AND IsDeleted=0

			--Insertion for LeadAssigned
			IF @@ROWCOUNT = 0
			BEGIN
				INSERT INTO CRM_TargetLog(UserId,Brand,Type,Date,Value,ActionTakenBy,ActionTakenOn)
				VALUES (@TempUser,@Brand,5,GETDATE(),@LeadAssigned,@CreatedBy,GETDATE())
			END
		END
		
		--Check if the consultant is switched to another brand on same day.Only one switch is allowed per day
		SELECT @CurrentBrand = MAX(CurrentBrand),@LastBrand = MAX(LastBrand)
		FROM
		(
			SELECT 
			CASE DATE WHEN CAST(GETDATE() AS DATE) THEN BRAND ELSE NULL END AS CurrentBrand,
			CASE DATE WHEN CAST(GETDATE()-1 AS DATE) THEN BRAND ELSE NULL END AS LastBrand  
			FROM CRM_TargetLog 
			WHERE UserId = @TempUser AND IsDeleted=0
		)AS TempTable
		
		--UPDATION IN CRM_IndividualTarget AND CRM_TargetLog FOR BRAND ONLY
		IF @CurrentBrand = @LastBrand OR @CurrentBrand IS NULL OR @LastBrand IS NULL
		BEGIN
			--Updation for LeadProcessed
			UPDATE CRM_IndividualTarget
			SET Brand = ISNULL(@Brand,Brand)
			WHERE UserId = @TempUser AND Type = 4 AND Date = @Date

			--Updation for LeadAssigned
			UPDATE CRM_IndividualTarget
			SET Brand = ISNULL(@Brand,Brand)
			WHERE UserId = @TempUser AND Type = 5 AND Date = @Date

			IF MONTH(@Date) = MONTH(GETDATE()) AND YEAR(@Date) = YEAR(GETDATE())
			BEGIN
				--Updation for LeadProcessed
				UPDATE CRM_TargetLog
				SET Brand = ISNULL(@Brand,Brand)
				WHERE UserId = @TempUser AND Type = 4 AND Date = CAST(GETDATE() AS DATE) AND IsDeleted=0

				--Updation for LeadAssigned
				UPDATE CRM_TargetLog
				SET Brand = ISNULL(@Brand,Brand)
				WHERE UserId = @TempUser AND Type = 5 AND Date = CAST(GETDATE() AS DATE) AND IsDeleted=0
			END
		END
	END
	SET @Status = 1
END

