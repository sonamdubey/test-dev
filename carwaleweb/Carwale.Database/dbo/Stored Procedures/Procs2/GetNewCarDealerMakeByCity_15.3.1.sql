IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetNewCarDealerMakeByCity_15]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetNewCarDealerMakeByCity_15]
GO

	
-- =============================================          
-- Author:  <Supriya K>          
-- Description: <Get the List of New Car Dealer Makes By City>          
-- Tables Used : Dealer_NewCar,CarMakes          
-- Create By: Supriya on <29/05/2014>     
-- Approved by: Manish Chourasiya on 01-07-2014 06:00 pm.
-- =============================================          
CREATE PROCEDURE [dbo].[GetNewCarDealerMakeByCity_15.3.1] (@CityId SMALLINT)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT Ma.ID AS Value
		,Ma.NAME AS TEXT
	FROM CarMakes Ma WITH (NOLOCK)
	WHERE Ma.IsDeleted = 0
		AND Ma.Id IN (
			SELECT MakeId
			FROM Dealer_NewCar AS DNC WITH (NOLOCK)
			WHERE CityId = @CityId
				AND DNC.IsActive = 1
				AND DNC.IsNewDealer= 1 -- added by sanjay
			)
	ORDER BY TEXT
END

