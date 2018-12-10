IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetNewCarDealerMakeByCity]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetNewCarDealerMakeByCity]
GO

	-- =============================================          
-- Author:  <Supriya K>          
-- Description: <Get the List of New Car Dealer Makes By City>          
-- Tables Used : Dealer_NewCar,CarMakes          
-- Create By: Supriya on <29/05/2014>     
-- Approved by: Manish Chourasiya on 01-07-2014 06:00 pm.
-- =============================================          
CREATE PROCEDURE [dbo].[GetNewCarDealerMakeByCity] (@CityId SMALLINT)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT Ma.ID AS Value
		,Ma.NAME AS TEXT
	FROM CarMakes Ma WITH (NOLOCK)
	WHERE Ma.IsDeleted = 0
		AND Ma.Id IN (
			SELECT MakeId
			FROM Dealer_NewCar WITH (NOLOCK)
			WHERE CityId = @CityId
				AND IsActive = 1
			)
	ORDER BY TEXT
END

