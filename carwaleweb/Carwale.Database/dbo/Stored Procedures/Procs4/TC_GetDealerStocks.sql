IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetDealerStocks]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetDealerStocks]
GO

	
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
-- Author:		Surendra Chouksey
-- Create date: 15th Nov 2011
-- Description:	Getting All STock for dealer
-- =============================================
CREATE PROCEDURE [dbo].[TC_GetDealerStocks]
(
@UserId VARCHAR(50),
@Password VARCHAR(50)
)
AS
BEGIN
	-- interfering with SELECT STatements.
	SET NOCOUNT ON;
	DECLARE @DealerId BIGINT=NULL
	
	SELECT @DealerId=DealerId FROM TC_APIUsers 
	WHERE UserId=@UserId AND Password=@Password AND IsActive=1
	
	IF(@DealerId IS NOT NULL)
	BEGIN
		Select ST.Id StockId, MK.Name Make, CM.Name Model,CV.Name Version,ST.MakeYear,ST.RegNo,
		ST.Kms,ST.Price,ST.Colour Color,CC.RegistrationPlace RegPlace,CC.Owners OwnerType,
		CC.OneTimeTax,CC.Insurance,CC.InsuranceExpiry,CC.InteriorColor,CC.CityMileage,
		CC.AdditionalFuel,CC.CarDriven,CC.Accidental ISCarAccidental,CC.FloodAffected IsCarFloorAffected,
		CC.Features_SafetySecurity SafetyAndSecurity,CC.Features_Comfort ComfortAndConvenience,CC.Features_Others OtherFeatures,
		CC.ACCondition,CC.BatteryCondition,CC.BrakesCondition,CC.ElectricalsCondition,CC.EngineCondition,CC.SeatsCondition,
		CC.SuspensionsCondition,CC.SuspensionsCondition,CC.TyresCondition,CC.InteriorCondition,
		CC.ExteriorCondition,CC.OverallCondition,CC.Warranties,CC.Modifications,CC.Comments
		From TC_Stock ST
		Inner Join TC_CarCondition CC On CC.STockId = ST.Id	
		Left Join Dealers Db On Db.Id = ST.BranchId
		Left Join CarVersions CV On CV.Id=ST.VersionId
		Left Join CarModels CM On CM.Id=CV.CarModelId
		Left Join CarMakes MK On MK.Id=CM.CarMakeId 
		Where ST.BranchId=@DealerId AND ST.StatusId=1	
	END			 
END


