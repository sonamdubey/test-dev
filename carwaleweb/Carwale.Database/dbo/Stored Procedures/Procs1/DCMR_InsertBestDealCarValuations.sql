IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCMR_InsertBestDealCarValuations]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCMR_InsertBestDealCarValuations]
GO

	-- =============================================
-- Author	:	Sachin Bharti(23rd May 2014)
-- Description	:	Insert car valuaiton data in BestDealCarValuations table
-- =============================================
CREATE PROCEDURE [dbo].[DCMR_InsertBestDealCarValuations]
	
	@CarIds			VARCHAR(500),
	@CarValuaitons	VARCHAR(500)
AS
BEGIN
	
	SET NOCOUNT ON;
	DECLARE @CarIdRowCount INT
	DECLARE	@CarValueRowCount INT
	DECLARE @RowCount	INT
	DECLARE	@CarId		INT
	DECLARE	@CarValuation	INT

	--Temp Table containing SellInquiryIds
	DECLARE @TempCarId Table	(	RowID INT IDENTITY(1, 1),	CarId NUMERIC)

	--Temp Table containing CarValuation or Car Price
	DECLARE @TempCarValuation Table	(	RowID INT IDENTITY(1, 1),	CarValuation NUMERIC)
	
	INSERT INTO @TempCarId 
				SELECT *FROM SplitText(@CarIds,',')
	SET @CarIdRowCount = @@ROWCOUNT

	INSERT INTO @TempCarValuation  
		SELECT *FROM SplitText(@CarValuaitons,',')
	SET @CarValueRowCount = @@ROWCOUNT

	PRINT @CarIdRowCount
	PRINT @CarValueRowCount

	IF @CarIdRowCount = @CarValueRowCount
	BEGIN
		SET		@RowCount = 1
		WHILE	@RowCount <= @CarIdRowCount
		BEGIN
			SELECT @CarId = CarId FROM @TempCarId WHERE RowID = @RowCount
			SELECT @CarValuation = CarValuation FROM @TempCarValuation WHERE RowID = @RowCount
			
			SELECT ID FROM BestDealCarValuations WHERE CarId = @CarId
			IF @@ROWCOUNT = 0
			BEGIN
				INSERT INTO BestDealCarValuations (	CarId,	Entrydate ,	isActive,	Valuation, UserType )
							VALUES	(	@CarId,	GETDATE(),	1,	@CarValuation , 1)
			END
			SET @RowCount += 1
		END
	END
END
