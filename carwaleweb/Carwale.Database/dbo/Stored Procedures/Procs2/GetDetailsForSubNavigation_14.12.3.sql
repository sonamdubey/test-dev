IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetDetailsForSubNavigation_14]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetDetailsForSubNavigation_14]
GO

	
--writtenby Natesh kumar for getting bool values or the need in subnavigation
-- written on 13/10/14
--modified by Rohan Sapkal on 22-12-2014 , LEFT JOIN on ModelOffers
CREATE PROCEDURE [dbo].[GetDetailsForSubNavigation_14.12.3] @ModelId INT
AS
BEGIN
	SELECT Mo.Discontinuation_date
		,mo.IsDeleted
		,Mo.Futuristic
		,mo.New
		,mo.ReviewCount
		,mo.Used
		,mo.VideoCount
		,mo.UsedCarRating
		,MOF.ModelId as OfferExists
	FROM CarModels Mo
	INNER JOIN CarMakes Mk WITH(NOLOCK) ON Mk.ID = Mo.CarMakeId
	LEFT JOIN ModelOffers MOF WITH(NOLOCK) on MOF.ModelId=Mo.ID
	WHERE Mo.CarMakeId = Mk.Id
		AND Mo.ID = @ModelId
END
