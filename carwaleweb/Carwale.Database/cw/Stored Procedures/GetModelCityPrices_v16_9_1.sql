IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[GetModelCityPrices_v16_9_1]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[GetModelCityPrices_v16_9_1]
GO

	
-- =============================================      
-- Author:  <Ravi koshal>
-- Create date: <24/04/2014>      
-- Description: <Returns the list of cities with the price ranges for the model>
-- Modified By: jitendra solanki fetch state name and cityMaking name 
-- =============================================      
CREATE PROCEDURE [cw].[GetModelCityPrices_v16_9_1] -- execute cw.GetModelCityPrices 263,1
	@ModelId NUMERIC(18, 0)
	,@CityId INT = - 1
AS
BEGIN
	SET NOCOUNT ON;

	IF @CityId <> - 1
	BEGIN
		DECLARE @Latt DECIMAL = NULL
		DECLARE @Long DECIMAL = NULL
		DECLARE @lattSecPerKm DECIMAL = 32.57940665
		DECLARE @longSecPerKm DECIMAL = 34.63696611
		DECLARE @ProxDist INT = 300
		DECLARE @TempLatt DECIMAL = NULL
		DECLARE @TempLong DECIMAL = NULL
		DECLARE @ConstLt FLOAT = 0.030694236 
	    DECLARE @ConstLg FLOAT= 0.028870889 -- // constants 

		SELECT @Latt = Lattitude
			,@Long = Longitude
		FROM Cities with (nolock)
		WHERE ID = @CityId

		SET @TempLatt = @ProxDist * @lattSecPerKm
		SET @TempLong = @ProxDist * @longSecPerKm


		SELECT  TOP 6 ct.Name AS CityName
			,min(price) AS MinPrice
			,max(price) AS MaxPrice
			,st.Name AS StateName
			,ct.CityMaskingName AS CityMaskingName
			,Sqrt( Power( ((ISNULL(@Latt,0) - CT.Lattitude) * @ConstLt), 
										   2 
										  ) 
									 + Power( (( ISNULL(@Long,0) - CT.Longitude) * @ConstLg ), 
											2)
								   )  AS Distance
		FROM NewCarShowroomPrices AS n WITH (NOLOCK)
		JOIN CarVersions AS cv WITH (NOLOCK) ON n.CarVersionid = cv.ID
		JOIN CarModels AS cm WITH (NOLOCK) ON cm.id = cv.CarModelId
		JOIN Cities AS ct WITH (NOLOCK) ON ct.ID = n.CityId
		JOIN States As st WITH (NOLOCK) ON st.ID = ct.StateId
		WHERE n.isactive = 1
			AND cv.new = 1
			AND cv.isdeleted = 0
			AND cm.isdeleted = 0
			AND cm.ID = @ModelId
			AND Lattitude BETWEEN ABS((@TempLatt - @Latt))
							AND (@TempLatt + @Latt)
			AND Longitude BETWEEN ABS((@TempLong - @Long))
							AND (@TempLong + @Long)
			AND CT.IsDeleted = 0
			AND CT.ID <> @CityId 
         GROUP BY cm.id
			,n.cityid
			,ct.Name
			,st.Name
			,ct.CityMaskingName
			,CT.Lattitude
			,CT.Longitude
			ORDER BY Distance 
	END
	ELSE
	BEGIN
		SELECT MinPrice
			,MaxPrice
			,C.NAME AS CityName
			,S.Name AS StateName
			,C.CityMaskingName AS CityMaskingName
		FROM ModelMetroCityPrices MMCP WITH (NOLOCK)
		INNER JOIN cities C WITH (NOLOCK) ON MMCP.CityId = C.Id
		INNER JOIN States S WITH (NOLOCK) ON C.StateId = S.ID
		WHERE CarModelId = @ModelId
		ORDER BY MinPrice
	END
END

