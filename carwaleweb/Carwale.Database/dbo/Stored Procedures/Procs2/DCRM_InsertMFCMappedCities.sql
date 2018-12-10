IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_InsertMFCMappedCities]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_InsertMFCMappedCities]
GO

	-- =============================================
-- Author	:	Sachin Bharti(9th July)
-- Description	:	Insert mapped cities for Mahindra first choice in DCRM_MFCMappedCities
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_InsertMFCMappedCities]
	@Cities	VARCHAR(1000),
	@AddedBy	INT,
	@Result		INT OUTPUT
AS
BEGIN
	
	SET NOCOUNT ON;
	SET @Result = -1

	DECLARE @TempTable TABLE(RowId INT IDENTITY(1,1), CityId INT)
	DECLARE @RowCount	INT
	DECLARE	@TotalCities	INT
	DECLARE	@MapedCityId	INT

	--Store all the cities into temporary table
	INSERT INTO @TempTable SELECT *FROM SplitText(@Cities,',')
	SET @TotalCities = @@ROWCOUNT

	--Start looping to get all the cities
	SET @RowCount = 1
	WHILE(@RowCount <= @TotalCities)
	BEGIN
		--Get city from temp table
		SELECT  @MapedCityId = CityId FROM @TempTable WHERE RowId = @RowCount
		
		SELECT Id FROM DCRM_MFCMappedCities WHERE CityID = @MapedCityId 
		--If city all ready mapped then make it active
		IF @@ROWCOUNT <> 0
		BEGIN
			UPDATE DCRM_MFCMappedCities SET IsActive = 1 WHERE CityID = @MapedCityId
			SET @Result = 1
		END
		--If not make a new entry
		ELSE
		BEGIN
			INSERT	INTO	DCRM_MFCMappedCities(CityID,UpdatedBy,UpdatedOn,IsActive)
					VALUES	(@MapedCityId,@AddedBy,GETDATE(),1)
			SET @Result = 1
		END 
		SET @RowCount = @RowCount + 1
	END
END
