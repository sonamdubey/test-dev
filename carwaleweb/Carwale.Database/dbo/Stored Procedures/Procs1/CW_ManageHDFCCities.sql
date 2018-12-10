IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CW_ManageHDFCCities]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CW_ManageHDFCCities]
GO

	CREATE PROCEDURE [dbo].[CW_ManageHDFCCities]
@CityId Numeric(18,0)
,@SpokeCityId Numeric(18,0)
,@CatId Numeric(18,0)
,@RecordExists bit output
,@UpdatedBy INT = NULL
,@UpdatedOn DATETIME = NULL
,@CarCityId INT = -1
AS
--author:Rakesh Yadav on 02 Aug 2015
--Desc: add city and spoke
BEGIN
	SET @RecordExists=1	

	IF @CarCityId IS NOT NULL AND @CarCityId <> -1
	BEGIN
		IF NOT EXISTS(Select Id from CW_CarCities WHERE CW_CityId=@CityId AND SpokeCityId=@SpokeCityId)
		BEGIN
			UPDATE CW_CarCities
			SET CW_CityId = @CityId,
				SpokeCityId = @SpokeCityId,
				CatId = @CatId,
				UpdatedBy = @UpdatedBy,
				UpdatedOn = @UpdatedOn
			WHERE Id = @CarCityId
			
			SET @RecordExists=0
		END
		ELSE
		BEGIN
			UPDATE CW_CarCities
			SET CatId = @CatId,
				UpdatedBy = @UpdatedBy,
				UpdatedOn = @UpdatedOn
			WHERE Id = @CarCityId
			
			SET @RecordExists=1
		END
	END
	ELSE
	BEGIN
		IF NOT EXISTS(Select Id from CW_CarCities WHERE CW_CityId=@CityId AND SpokeCityId=@SpokeCityId)
		BEGIN
			INSERT INTO CW_CarCities (CW_CityId,SpokeCityId,CatId, UpdatedBy)
			VALUES (@CityId,@SpokeCityId,@CatId, @UpdatedBy)
			
			SET @RecordExists=0			
		END
	END

	/*
	IF NOT EXISTS(Select Id from CW_CarCities WHERE CW_CityId=@CityId AND SpokeCityId=@SpokeCityId)
	BEGIN
		
		IF @CarCityId IS  NULL OR @CarCityId = -1
		BEGIN
			INSERT INTO CW_CarCities (CW_CityId,SpokeCityId,CatId, UpdatedBy)
			VALUES (@CityId,@SpokeCityId,@CatId, @UpdatedBy)
			
			SET @RecordExists=0
		END
		ELSE
		BEGIN
			UPDATE CW_CarCities
			SET CW_CityId = @CityId,
				SpokeCityId = @SpokeCityId,
				CatId = @CatId,
				UpdatedBy = @UpdatedBy,
				UpdatedOn = @UpdatedOn
			WHERE Id = @CarCityId
		END
	END*/
END
