IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Microsite_UsedModelSlider]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Microsite_UsedModelSlider]
GO

	-- Author:  Chetan Kane    
-- Create date: 30th May 2012    
-- Description: This sp will be used for the UCD Widget 'UsedModelSlider'    
--If the database have the no of Featured car defined only featured cars will be fetch other wise the less     
--no of featured cars will be fullfill with the latest cars    
--Modified By: Rakesh Yadav on 05 Aug 2015 to get OriginalImgPath and HostUrl
-- =============================================    
CREATE PROCEDURE [dbo].[Microsite_UsedModelSlider]     
 -- Add the parameters for the stored procedure here    
@DealerId INT,    
@Count INT,
@ORDER TINYINT=1    
AS    
BEGIN    
--DECLARE  @CarCount INT=0     
--SET @carCount = (SELECT COUNT(*) FROM TC_Stock WHERE BranchId = @DealerId AND IsApproved = 1 AND IsFeatured = 1)    
   
  SELECT TOP (@Count) 
        TS.Id,
        vw.Car,
        CONVERT(VARCHAR(12), TS.MakeYear, 107) AS [CarMake],
        dbo.TitleCase(TS.Colour) Colour,
        TS.Kms,    
        CP.HostUrl+CP.DirectoryPath+CP.ImageUrlThumbSmall AS ImageURL,CP.HostUrl,CP.OriginalImgPath,
        dbo.GetFuelType(vw.CarFuelType) AS CarFuelType,    
        TS.Price,dbo.GetTransmissionType(vw.CarTransmission) AS CarTransmission,
        TS.EntryDate         
       FROM TC_Stock AS TS WITH(NOLOCK) 
       LEFT OUTER JOIN TC_CarPhotos CP WITH(NOLOCK) 
                                       ON CP.StockId=Ts.Id AND CP.IsMain=1 AND Cp.IsActive = 1    
       JOIN vwMMV AS vw 
                                       ON vw.VersionId=TS.VersionId    
      WHERE TS.BranchId = @DealerId
         AND TS.IsActive = 1 
         AND TS.StatusId = 1 
         AND IsApproved = 1 
      --   AND IsFeatured = 1   ---Condition maintained through order by clause
         ORDER BY   IsFeatured DESC,
                  CASE  WHEN @ORDER=2 THEN  TS.EntryDate 
                        WHEN @ORDER=4 THEN  TS.LastUpdatedDate
                          END  DESC,
                  CASE  WHEN @ORDER=1 THEN  TS.Price 
                        WHEN @ORDER=3 THEN  TS.KMS END DESC  
END 