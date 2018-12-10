IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_ClientSaveApprovals]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_ClientSaveApprovals]
GO

	

--Name of SP/Function				: CarWale.dbo CRM_ClientSaveApprovals
--Applications using SP/Function	: Dealer Panel website
--Modules using the SP/Function		: 
--Business department				: Dealer Panel
--Technical department				: Database
--Modification history				: 1. Dilip V. 03 Jan 2012 (Validation of dupicate record before insert by select statement)
--									: 2. Dilip V. 03-Jan-2012 (Insert UpdatedOn)
--									: 3. Amit Kumar 17th may 2012(Added  new datafield @ChangedSubEventType )
--									: 4. Dilip V. 24-Sep-2012 (For General Feedback)
--									: 5. Amit Kumar 5th december 2013 (Added @subDispositionId)

CREATE PROCEDURE [dbo].[CRM_ClientSaveApprovals]
	
	@ClientId				NUMERIC,
	@ClientType				SMALLINT,
	@LeadId					NUMERIC,
	@CBDId					NUMERIC,
	@CurrentEventType		INT,
	@ChangedEventType		INT,
	@ChangedSubEventType    INT,
	@IsApproved				BIT,
	@IsDCApproved			BIT,
	@IsValid				BIT,
	@CreatedOn				DATETIME,
	@UpdatedOn				DATETIME,
	@UpdatedByDealer		VARCHAR(50),
	@Comments				VARCHAR(1000),
	@DateValue				DATETIME,
	@subDispositionId		INT,
	@Id						NUMERIC OUTPUT
				
 AS

BEGIN
	SET NOCOUNT ON
	DECLARE @NumberRecords AS INT = 0;
	SET @Id = -1
	IF @CurrentEventType <> 0
		BEGIN
			PRINT @ClientType
			IF @IsDCApproved = 0
				SELECT Id 
				FROM CRM_CientPendingApprovals WITH(NOLOCK) 
				WHERE CBDId = @CBDId AND LeadId = @LeadId AND ChangedEventType = @ChangedEventType AND IsDCApproved = @IsDCApproved
			SET @NumberRecords = @@ROWCOUNT
		END
	PRINT @NumberRecords
	IF(@NumberRecords = 0)
		BEGIN
			INSERT INTO CRM_CientPendingApprovals
			(
				ClientId, ClientType, LeadId, CBDId, CurrentEventType, DateValue,
				ChangedEventType, IsApproved,IsDCApproved,IsValid, CreatedOn,UpdatedOn, UpdatedByDealer, Comments, ChangedSubEventType,SubDispositionId				
			)
			VALUES
			(
				@ClientId, @ClientType, @LeadId, @CBDId, @CurrentEventType, @DateValue,
				@ChangedEventType, @IsApproved,@IsDCApproved,@IsValid, @CreatedOn,@UpdatedOn, @UpdatedByDealer, @Comments, @ChangedSubEventType,@subDispositionId
			)
			
			SET @Id = SCOPE_IDENTITY()
		END		
END







