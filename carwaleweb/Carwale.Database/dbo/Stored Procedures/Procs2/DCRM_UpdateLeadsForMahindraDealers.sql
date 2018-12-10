IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_UpdateLeadsForMahindraDealers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_UpdateLeadsForMahindraDealers]
GO

	-- =============================================
-- Author	:	Sachin Bharti(29th July 2014)
-- Description	:	Set number of leads have to mahindra dealers
--					in that particular city
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_UpdateLeadsForMahindraDealers] 
	@Ids	VARCHAR(500),
	@Leads	VARCHAR(500),
	@Result	INT OUTPUT
AS
BEGIN
	
	SET NOCOUNT ON;

	SET @Result = -1
	--Declare temp tables to contain Ids and Leads
	DECLARE @TempMFCId TABLE (Id INT IDENTITY(1,1),TableId INT)  
	DECLARE @TempLeadsCount TABLE (Id INT IDENTITY(1,1),LeadCount INT)  

	--Declare variable for looping
	DECLARE @TotalIds	INT = 0
	DECLARE	@MFCId		INT
	DECLARE @NumberOfLeads	INT
	DECLARE	@Lead		INT		
	DECLARE @RowCount	INT

	--Fill Ids that we have to update 
	IF @Ids <> ''
	BEGIN
		INSERT INTO @TempMFCId SELECT *FROM SplitText(@Ids,',')
		SET @TotalIds = @@ROWCOUNT
	END

	--Fill leads 
	IF @Leads <> ''
	BEGIN
		INSERT INTO @TempLeadsCount SELECT *FROM SplitText(@Leads,',')
		SET @NumberOfLeads = @@ROWCOUNT
	END

	IF (@TotalIds = @NumberOfLeads)
	BEGIN
		SET @RowCount = 1
		WHILE @RowCount <= @TotalIds
		BEGIN
			SELECT @MFCId =TableId FROM @TempMFCId WHERE Id = @RowCount
			SELECT @Lead =LeadCount FROM @TempLeadsCount WHERE Id = @RowCount
			UPDATE DCRM_MFCMappedCities SET ProcurementLeads = @Lead WHERE id = @MFCId	
			IF @@ROWCOUNT <> 0
				SET @Result = 1
			SET @RowCount += 1
		END 
	END
END
