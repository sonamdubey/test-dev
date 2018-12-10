IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_FunnelMovement]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_FunnelMovement]
GO

	-- =============================================
-- Author	:	Sachin Bharti(6th May 2014)
-- Description	:	Funnel movement in terms of auto movement downwards.
--					70% to 50% ‐ in 15 days
--					50% to 30% ‐ in 15 days
-- =============================================
CREATE	PROCEDURE	[dbo].[DCRM_FunnelMovement]
	
AS
BEGIN
	
	SET NOCOUNT ON;

	DECLARE @NumberOfRecords	AS INT
	DECLARE @RowCount			AS INT
	DECLARE @CurrentDate		AS DATETIME
		
	--SET Current DateTime
	SET @CurrentDate = GETDATE()
		
	DECLARE @TempDealers Table	(	RowID INT IDENTITY(1, 1),	DealerId NUMERIC,	SalesDealerId INT,	
									PackageId INT,	PresentClsPrblty TINYINT , DayDifference INT	) 
		
	INSERT INTO @TempDealers	(	DealerId ,	SalesDealerId ,	PackageId ,	PresentClsPrblty ,DayDifference	)
								
								SELECT	DSD.DealerId , DSD.Id AS SlsDlrId,	DSD.PitchingProduct,	DSD.ClosingProbability,	DATEDIFF(DAY,DSD.UpdatedOn,GETDATE()) AS DayDiff
								FROM	DCRM_SalesDealer DSD WITH (NOLOCK)
								WHERE	((DSD.ClosingProbability		= 70 AND DATEDIFF(DAY,DSD.UpdatedOn,GETDATE()) = 16)	
										OR
										(DSD.ClosingProbability		= 50 AND DATEDIFF(DAY,DSD.UpdatedOn,GETDATE()) = 16))	
										AND	DSD.LeadStatus = 1
								ORDER BY DealerId

	-- Get the number of records in the temporary table
	SET		@NumberOfRecords = @@ROWCOUNT
	PRINT	@NumberOfRecords 
	SET		@RowCount = 1

	DECLARE @DealerId		INT,	
			@DealerSalesId	INT,
			@PitchProduct	INT,		
			@NumberOfDays	INT,	
			@OldClosingPrblty	TINYINT,
			@NewClosingPrblty	TINYINT = NULL
	
	WHILE @RowCount <= @NumberOfRecords
		BEGIN
			
			--Retrieve all the dealer details from temporary table
			SELECT	@DealerId = DealerId , @DealerSalesId = SalesDealerId , @PitchProduct = PackageId,
					@OldClosingPrblty = PresentClsPrblty , @NumberOfDays = DayDifference
			FROM @TempDealers WHERE RowID = @RowCount

			--Set new closing probability accordingly
			--Added one more day because SP is schedule after 12.00 am
			IF @OldClosingPrblty = 70 AND @NumberOfDays = 16	--Change closing probability from 70 to 50 if it is not changed since last 15 days
				SET @NewClosingPrblty = 50
			ELSE IF @OldClosingPrblty = 50 AND @NumberOfDays = 16	--Change closing probability from 50 to 30 if it is not changed since last 15 days
				SET @NewClosingPrblty = 30

			IF @NewClosingPrblty IS NOT NULL
			BEGIN
				--Update dealers package closing probability with new one
				UPDATE DCRM_SalesDealer SET	ClosingProbability = @NewClosingPrblty , UpdatedOn = GETDATE(),UpdatedBy = 13
				WHERE Id = @DealerSalesId

				--Log the record for change in package closing probability in DCRM_CPLog
				INSERT	INTO	DCRM_CPLog	( DealerId,	SalesDealerId,	OldValue,	NewValue,	UpdatedOn,	UpdatedBy	)
							VALUES		( @DealerId, @DealerSalesId , @OldClosingPrblty , @NewClosingPrblty , GETDATE(), 13)
			
			END
			SET @RowCount = @RowCount + 1
		END
END
