IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetStockDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetStockDetails]
GO

	---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
-- Author:        Surendra Chouksey
-- Create date: 10th July 2012
-- Description:    Getting Stockdetails
-- =============================================
CREATE PROCEDURE [dbo].[TC_GetStockDetails]
(
@BranchId BIGINT,
@StockId BIGINT
)
AS
BEGIN
    -- interfering with SELECT STatements.
    SET NOCOUNT ON;

        SELECT ST.ID,ST.IsBooked, CM.ID AS MakeId, CMO.ID AS ModelId, CV.ID AS VersionId, 
        CM.Name AS Make, CM.LogoUrl AS LogoUrl, CMO.Name AS Model, 
        CV.Name AS Version, CV.largePic AS CarLargePicUrl, ST.Price AS Price, ST.StatusId, 
        ST.Kms AS Kilometers, ST.MakeYear AS MakeYear, ST.Colour, ST.LastUpdatedDate, ST.RegNo, 
        SS.Status, IsNull(CP.ImageUrlThumb, '') As ImageUrlThumb,IsNull(CP.ImageUrlFull, '') As ImageUrlFull,
        CC.Owners, CC.Insurance, CC.InsuranceExpiry, 
        CC.OneTimeTax Tax, CC.RegistrationPlace, 
        CC.InteriorColor, CC.InteriorColorCode, CC.CityMileage, 
        CC.AdditionalFuel, CC.CarDriven, CC.Accidental, CC.FloodAffected, 
        CC.Warranties, CC.Modifications, CC.Comments, CC.BatteryCondition, 
        CC.BrakesCondition, CC.ElectricalsCondition, 
        CC.EngineCondition, CC.ExteriorCondition, 
        CC.SeatsCondition, CC.SuspensionsCondition, 
        CC.TyresCondition, CC.OverallCondition, 

        CC.Features_SafetySecurity, CC.Features_Comfort, CC.Features_Others, 
        CC.InteriorCondition, CC.ACCondition, 
        D.ContactEmail, D.Organization,D.MobileNo, D.FaxNo,D.WebsiteUrl,CP.DirectoryPath,
        ST.IsSychronizedCW ,ST.IsFeatured,CB.BookingDate,CP.HostUrl
        
        FROM TC_Stock ST WITH(NOLOCK)
        LEFT JOIN TC_CarCondition CC WITH(NOLOCK) On ST.Id = CC.StockId 
        INNER JOIN TC_StockStatus SS WITH(NOLOCK) On SS.Id = ST.StatusId 
        INNER JOIN Dealers D WITH(NOLOCK) On ST.BranchId = D.Id 
        LEFT JOIN TC_CarPhotos CP WITH(NOLOCK) On CP.StockId = ST.Id And CP.IsMain = 1 And CP.IsActive = 1 
        INNER JOIN CarVersions CV WITH(NOLOCK) On CV.Id = ST.VersionId 
        INNER JOIN CarModels CMO WITH(NOLOCK) On CMO.Id = CV.CarModelId 
        INNER JOIN CarMakes CM WITH(NOLOCK) On CM.Id = CMO.CarMakeId 
        LEFT JOIN TC_CarBooking CB WITH(NOLOCK) On ST.Id = CB.StockId 

        WHERE ST.ID = @StockId AND ST.BranchId=@BranchId 
            AND ST.IsActive=1 AND ST.IsApproved=1    
     
END