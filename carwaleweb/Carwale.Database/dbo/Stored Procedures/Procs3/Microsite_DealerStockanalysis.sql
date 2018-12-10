IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Microsite_DealerStockanalysis]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Microsite_DealerStockanalysis]
GO

	-- =============================================     
-- Author:    Avishkar Meshram     
-- Create date: Apr 27 2012     
-- Description:  Dealer home page for Stock analysis     
-- EXEC Microsite_DealerStockanalysis 968,'Latest'   
-- Modified by Manish on 06-01-2014 added conditon isActive for consider only active photos.  
-- Modified by Manish on 19-02-2014 remove codition from IsActive from where clause and put with left join for Carphotos
--Modified by Rakesh Yadav on 05 Aug 2015 to get HostURL and OriginalImgPath
-- =============================================     
CREATE PROCEDURE [dbo].[Microsite_DealerStockanalysis]
@DealerId  INT,     
@RecordCount  INT,                                 
@StockFilterCriteria VARCHAR(20)     
AS     
  BEGIN     
      SET NOCOUNT ON;     
    
      IF ( @StockFilterCriteria = 'Latest' )  
		BEGIN   
			SELECT TOP (@RecordCount) TS.Id,     
                     vw.Car,     
                     Datename(month, TS.MakeYear) + ' ' + Cast(     
                     Year(TS.MakeYear)AS CHAR(4)) AS     
                     CarMake,     
                     TS.Colour,     
                     TS.Kms,     
                     CP.HostUrl+CP.DirectoryPath + CP.ImageUrlThumbSmall AS ImageUrl
					 ,CP.HostUrl
					 ,CP.OriginalImgPath
                     ,CF.FuelType,     
                     TS.Price,     
                     TS.EntryDate,     
                     TCC.Owners,     
                     CASE CV.CarTransmission     
                       WHEN 1 THEN 'Automatic'     
                       WHEN 2 THEN 'Manual'     
                     END  AS Transmission     
        FROM   TC_Stock AS TS     
               JOIN TC_CarCondition AS TCC     
                 ON TCC.StockId = TS.Id     
               JOIN vwMMV AS vw     
                 ON vw.VersionId = TS.VersionId     
               JOIN CarVersions AS CV     
                 ON vw.VersionId = CV.ID     
               JOIN CarFuelType CF     
                 ON vw.CarFuelType = CF.FuelTypeId     
               LEFT OUTER JOIN TC_CarPhotos CP     
                 ON CP.StockId = Ts.Id AND CP.IsMain = 1  AND   CP.IsActive=1  ---- Added by Manish on 19-02-2014 
        WHERE  TS.BranchId = @DealerId AND IsApproved = 1 
               AND TS.IsActive = 1 AND TS.StatusId = 1 
			   --AND  CP.IsActive=1      --Added by Manish on 06-01-2014  for consider only active photos.                
        ORDER  BY TS.EntryDate DESC   
        END 
      ELSE IF ( @StockFilterCriteria = 'Price' )   
		BEGIN  
			SELECT TOP (@RecordCount) TS.Id,     
                     vw.Car,     
                     Datename(month, TS.MakeYear) + ' ' + Cast(     
                     Year(TS.MakeYear)AS CHAR(4)) AS     
                     CarMake,     
                     TS.Colour,     
                     TS.Kms,     
                     CP.HostUrl+CP.DirectoryPath + CP.ImageUrlThumbSmall AS ImageUrl, 
					 CP.HostUrl
					 ,CP.OriginalImgPath    
                     ,CF.FuelType,     
                     TS.Price,     
                     TS.EntryDate,     
                     TCC.Owners,     
                     CASE CV.CarTransmission     
                       WHEN 1 THEN 'Automatic'     
                       WHEN 2 THEN 'Manual'     
                     END  AS Transmission     
        FROM   TC_Stock AS TS     
               JOIN TC_CarCondition AS TCC     
                 ON TCC.StockId = TS.Id     
               JOIN vwMMV AS vw     
                 ON vw.VersionId = TS.VersionId     
               JOIN CarVersions AS CV     
                 ON vw.VersionId = CV.ID     
               JOIN CarFuelType CF     
                 ON vw.CarFuelType = CF.FuelTypeId     
               LEFT OUTER JOIN TC_CarPhotos CP     
                 ON CP.StockId = Ts.Id AND CP.IsMain = 1  AND   CP.IsActive=1  ---- Added by Manish on 19-02-2014    
        WHERE  TS.BranchId = @DealerId AND IsApproved = 1    
               AND TS.Price <= 300000 
               AND TS.IsActive = 1 AND TS.StatusId = 1    
			 --  AND  CP.IsActive=1      --Added by Manish on 06-01-2014  for consider only active photos.                 
        ORDER  BY TS.EntryDate DESC   
        END
          
       ELSE IF (ISNUMERIC(@StockFilterCriteria)= 1 AND (CONVERT(INT,@StockFilterCriteria) BETWEEN 1 AND 6))
        BEGIN 
			SELECT TOP (@RecordCount) TS.Id,     
                     vw.Car,     
                     Datename(month, TS.MakeYear) + ' ' + Cast(     
                     Year(TS.MakeYear)AS CHAR(4)) AS     
                     CarMake, TS.Colour,TS.Kms,     
                     CP.HostUrl+CP.DirectoryPath + CP.ImageUrlThumbSmall AS ImageUrl,     
					 CP.HostUrl
					 ,CP.OriginalImgPath    
                     ,CF.FuelType,     
                     TS.Price,     
                     TS.EntryDate,     
                     TCC.Owners,     
                     CASE CV.CarTransmission     
                       WHEN 1 THEN 'Automatic'     
                       WHEN 2 THEN 'Manual'     
       END  AS Transmission     
        FROM   TC_Stock AS TS     
               JOIN TC_CarCondition AS TCC     
                 ON TCC.StockId = TS.Id     
               JOIN vwMMV AS vw     
                 ON vw.VersionId = TS.VersionId     
               JOIN CarVersions AS CV     
                 ON vw.VersionId = CV.ID     
               JOIN CarFuelType CF     
                 ON vw.CarFuelType = CF.FuelTypeId     
               LEFT OUTER JOIN TC_CarPhotos CP     
                 ON CP.StockId = Ts.Id AND CP.IsMain = 1   AND   CP.IsActive=1  ---- Added by Manish on 19-02-2014   
        WHERE  TS.BranchId = @DealerId    
               AND vw.BodyStyleId = 1                
               AND TS.IsActive = 1 AND TS.StatusId = 1 AND IsApproved = 1
			--   AND  CP.IsActive=1      --Added by Manish on 06-01-2014  for consider only active photos.                
        ORDER  BY TS.EntryDate DESC     
        END
  END 

