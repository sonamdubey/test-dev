IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[LL_UpdateCities]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[LL_UpdateCities]
GO

	
------------------------------------------------------------
-- Modified By : Sadhana Upadhyay on 27 Apr 2015
-- Summary : Removing useless code for update
------------------------------------------------------------

CREATE PROCEDURE [dbo].[LL_UpdateCities] 
	@CityId AS NUMERIC
	,@CityName AS VARCHAR(100)
	,@Lattitude AS DECIMAL(18, 4)
	,@Longitude AS DECIMAL(18, 4)
AS
BEGIN
	--first update the listing. if it is not there then insert the data
	--UPDATE LL_Cities
	--SET CityName = @CityName
	--	,Lattitude = @Lattitude
	--	,Longitude = @Longitude
	--WHERE CityId = @CityId

	--IF @@ROWCOUNT = 0
	--BEGIN
	--	IF @CityId > 0
	--	BEGIN
	--		--since the record is not there. hence add the data
	--		INSERT INTO LL_Cities (
	--			CityId
	--			,CityName
	--			,Lattitude
	--			,Longitude
	--			)
	--		VALUES (
	--			@CityId
	--			,@CityName
	--			,@Lattitude
	--			,@Longitude
	--			)
	--	END
	--END

	IF NOT EXISTS(SELECT CityId FROM LL_Cities WITH(NOLOCK) WHERE CityId = @CityId)
	--BEGIN
	--	UPDATE LL_Cities
	--	SET CityName = @CityName
	--		,Lattitude = @Lattitude
	--		,Longitude = @Longitude
	--	WHERE CityId = @CityId
	--END 
	--ELSE 
		BEGIN
			INSERT INTO LL_Cities (
				CityId
				,CityName
				,Lattitude
				,Longitude
				)
			VALUES (
				@CityId
				,@CityName
				,@Lattitude
				,@Longitude
				)
		END
END

