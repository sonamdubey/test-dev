IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_UpdateDealerStatus]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_UpdateDealerStatus]
GO

	-- =============================================
-- Author		: Vaibhav K
-- Create date	: 11-June-2012
-- Description	: Update record about sales dealer status from DCRM_SalesDealer
--				: Also if lead status changed to converted schedule welcome call for service BO executive
--				: Also if sales appointment is scheduled insert record in DCRM_SalesMeeting
--Modifier		: 1.Vaibhav K (16-10-2012)
--				: Add SalesDealerId in CP LOG & DCRM_SalesMeeting
--Modifier		: 2.Sachin Bharti(25-Jan-2013)
--				: Call [DCRM].[AssignDealerOnUserRole] to check
--				: for any User exist to that Dealer if not then insert new entry
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_UpdateDealerStatus]
	-- Add the parameters for the stored procedure here
	@Id					NUMERIC,
	@DealerId			NUMERIC,
	@ClosingProbability	INT,
	@ClosingDate		DATETIME,
	@ClosingAmount		NUMERIC,
	@PitchingProduct	INT,
	@PitchDuration		INT,
	@LeadStatus			INT,
	@UpdatedBy			INT,
	@UpdatedOn			DATETIME,
	@Status				INT OUTPUT,
	@BOExec				INT = -1,
	@SFieldExec			INT = -1,
	@OldValue			INT = -1,
	@NewValue			INT = -1,
	@LostReasons		INT = -1,
	--Parameters for scheduling appointment
	@IsScheduled		BIT = 0,
	@AtMeeting			BIT = 0,
	@ScheduledTo		INT = -1,
	@ScheduledDate		DATETIME = NULL,
	@IsActionTaken		BIT = 0,
	@ActionTakenOn		DATETIME = NULL,
	@DealerStatus		TINYINT = NULL,
	@ActionComments		VARCHAR(500) = NULL
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET @Status = -1
    -- Insert statements for procedure here
	UPDATE DCRM_SalesDealer SET ClosingProbability = @ClosingProbability , ClosingDate = @ClosingDate , 
		ClosingAmount = @ClosingAmount , PitchingProduct = @PitchingProduct , PitchDuration = @PitchDuration , 
		LeadStatus = @LeadStatus , UpdatedBy = @UpdatedBy , UpdatedOn = @UpdatedOn
	WHERE Id = @Id
	
	--Commented By Sachin Bharti (3rd April 2013) Because no call assign and transfer of calls 
	--To insert/update record into DCRM_ADM_UserDealers to assign role to service executives
	--only if lead status is changed to Converted
	--IF @LeadStatus = 2
	--	BEGIN
	--		--To set executive record in DCRM_SalesDealer
	--		UPDATE DCRM_SalesDealer
	--			SET BOExecutive = @BOExec,
	--				FieldExecutive = @SFieldExec
	--		WHERE Id = @Id
			
	--		DECLARE @OldCallerId AS INT
	--		DECLARE @OldCallerName AS VARCHAR(100)
	--		DECLARE @Subject AS VARCHAR(200)
	--		DECLARE @NewCallerName AS VARCHAR(100)
			
	--		SELECT @OldCallerId = UserId, @OldCallerName = OU.UserName  FROM DCRM_ADM_UserDealers DAU WITH (NOLOCK)
	--			INNER JOIN OprUsers OU WITH (NOLOCK) ON  OU.Id = DAU.UserId  
	--			WHERE DealerId = @DEALERID AND RoleId = 5
				
	--		IF @@ROWCOUNT = 0
	--			BEGIN
	--				-- Dealer doesn't belong to anyone, Assign Dealer to new user 
	--				--Assign Service Field Executive
	--				EXECUTE [DCRM].[AssignDealerOnUserRole] @SFieldExec,5,@DealerId,@UpdatedBy
	--			END
	--		ELSE
	--			BEGIN
	--				--Dealer belongs to someone, First handle the exisitng calls and alerts then proceed
	--				SELECT @NewCallerName = UserName FROM OprUsers WHERE Id = @BOExec
	--				SET @Subject = 'Transfer From ' + @OldCallerName + ' to ' + @NewCallerName
					
					
	--				--EXEC [dbo].[DCRM_UpdateDCRMCalls] @DealerId, 5, @OldCallerId, @SFieldExec, @OldCallerName, @NewCallerName, @UpdatedBy, @Subject
	--				--EXECUTE [DCRM].[AssignDealerOnUserRole] @SFieldExec,5,@DealerId,@UpdatedBy
	--			END
				
			
		
	--	END
		
	--Record lost region if lead status is changed to Lost
	IF @LeadStatus = 3
		BEGIN
			UPDATE DCRM_SalesDealer
				SET LostReason = @LostReasons
			WHERE Id = @Id
		END
	
	--Scheduling sales field meeting if checkbox is clicked
	--Vaibhav K (16-10-2012) : Also add SalesDealerId into table
	--Commented By Sachin Bharti(4th April 2013)Now insert and update both possilbe
	--IF @IsScheduled = 1
	--	BEGIN
	--		INSERT INTO DCRM_SalesMeeting (DealerId,ScheduledBy,ScheduledTo,ScheduledDate,IsActionTaken,SalesDealerId)
	--		VALUES (@DealerId,@UpdatedBy,@UpdatedBy,@ScheduledDate,0,@Id)
			
	--		UPDATE DCRM_SalesDealer
	--			SET IsSalesAppointment = 1
	--		WHERE Id = @Id
	--	END	
		
	----If user is of sales field then update record from DCRM_SalesMeeting
	--IF @AtMeeting = 1
	--	BEGIN
	--		UPDATE DCRM_SalesMeeting
	--			SET IsActionTaken = @IsActionTaken,
	--				ActionTakenOn = @ActionTakenOn,
	--				ActionTakenBy = @UpdatedBy,
	--				ActionComments = @ActionComments
	--			WHERE DealerId = @DealerId AND ScheduledTo = @UpdatedBy AND IsActionTaken = 0
				
	--			UPDATE DCRM_SalesDealer
	--				SET IsSalesAppointment = 0
	--			WHERE Id = @Id
	--	END
	
	
	SELECT *FROM DCRM_SalesMeeting WHERE DealerId = @DealerId AND ScheduledTo = @ScheduledTo AND IsActionTaken = 0
	IF @@ROWCOUNT = 0  --If user has scheduled new Sales meeting with Dealer 
		BEGIN
			INSERT INTO DCRM_SalesMeeting (DealerId,ScheduledBy,ScheduledTo,ScheduledDate,IsActionTaken,SalesDealerId,ActionTakenOn,ActionComments,ActionTakenBy,DealerStatus)
			VALUES (@DealerId,@UpdatedBy,@UpdatedBy,@ScheduledDate,0,@Id,@ActionTakenOn,@ActionComments,@UpdatedBy,@DealerStatus)
			SET @Status = 3 --If meeting is open and new Meeting want to scheduled
		END
	ELSE 
		BEGIN
			UPDATE DCRM_SalesMeeting
				SET IsActionTaken = @IsActionTaken,
					ActionTakenOn = @ActionTakenOn,
					ActionTakenBy = @UpdatedBy,
					ActionComments = @ActionComments,
					DealerStatus =	@DealerStatus
				WHERE DealerId = @DealerId AND ScheduledTo = @UpdatedBy AND IsActionTaken = 0
				
				UPDATE DCRM_SalesDealer
					SET IsSalesAppointment = 0
				WHERE Id = @Id
		END
	--Commented by Sachin Bharti(3rd April 2013)Because no call transfer and assignment call is done
	--To maintain log of closing probabilities for dealer
	--Vaibhav K (16-10-2012) : Also add SalesDealerId into CP Log
	--IF @OldValue <> @NewValue
	--	BEGIN
	--		INSERT INTO DCRM_CPLog (DealerId,OldValue,NewValue,UpdatedOn,UpdatedBy,SalesDealerId)
	--		VALUES (@DealerId,@OldValue,@NewValue,@UpdatedOn,@UpdatedBy,@Id)
	--	END
	SET @Status = 1
END
