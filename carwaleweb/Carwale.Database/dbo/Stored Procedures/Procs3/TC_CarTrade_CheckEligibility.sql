IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_CarTrade_CheckEligibility]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_CarTrade_CheckEligibility]
GO

	-- =============================================
-- Author      : Chetan Navin
-- Create date : 9th Feb 2015
-- Description : To check if the car is eligible for warranty/inspection
-- =============================================
CREATE PROCEDURE [dbo].[TC_CarTrade_CheckEligibility] 
	@StockId INT,
	@IsEligibleCertification BIT = 0 OUTPUT 
AS
BEGIN
	SET NOCOUNT ON;
	SELECT 	@IsEligibleCertification = AE.IsEligibleCertification
	FROM	TC_Stock ST WITH(NOLOCK)
			INNER JOIN CarVersions V WITH(NOLOCK) ON  V.ID = ST.VersionId  
			INNER JOIN AbSure_EligibleModels AE WITH(NOLOCK) ON AE.ModelId = V.CarModelId AND AE.IsActive = 1
			INNER JOIN Dealers D WITH(NOLOCK) ON D.ID = ST.BranchId 
			INNER JOIN AbSure_EligibleCities AEC WITH(NOLOCK) ON AEC.CityId = D.CityId
	WHERE   ST.Id = @StockId
END
