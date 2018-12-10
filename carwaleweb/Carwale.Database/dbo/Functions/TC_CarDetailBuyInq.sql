IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_CarDetailBuyInq]') 
    AND xtype IN (N'FN', N'IF', N'TF')
)
    DROP FUNCTION [dbo].[TC_CarDetailBuyInq]
GO

	-- ============================================= 
-- Author:    Manish Chourasiya 
-- Create date: 16-01-13
-- Description:  Return the CarDetail for loose buyer inquriry in TC.
-- Modified By : Tejashree Patil on 2 April 2013, Higher preference given to ModelNames instead of Price.
-- Modified By : Manish on 23 April 2013, Prefernce login has been changed.
-- Modified By : Nilesh Utture on 10th May, 2013 Addded extra check on MinPrice AND MakeYearFrom for default values
-- ============================================= 
CREATE FUNCTION [dbo].[TC_CarDetailBuyInq](@PriceMin     INT, 
                                          @PriceMax     INT,
                                          @MakeYearFrom SMALLINT, 
                                          @MakeYearTo   SMALLINT, 
                                          @ModelName    VARCHAR(MAX), 
                                          @BodyStyle   VARCHAR(MAX), 
                                          @FuelType   VARCHAR(MAX)
                                       ) 
returns VARCHAR(MAX)
AS 
  BEGIN 
      DECLARE @CarDetail varchar(MAX) 
      DECLARE @PriceFlag BIT = 1
      
      -- Modified By : Nilesh Utture on 10th May, 2013   
      IF(@PriceMin = 0 AND @PriceMax = 10000000) -- If PriceMin and PriceMax are having their Default values then dont assign @CarDetail
      BEGIN
			SET @PriceFlag = 0
      END 
	  
      IF @ModelName IS NOT NULL
      
           SET @CarDetail='Model-'+@ModelName 
           
           
       ELSE IF       @BodyStyle IS NOT NULL  
                         
          SET @CarDetail=       'Body Style-'
                               +isnull(@BodyStyle,'')  
                               
      -- Modified By : Nilesh Utture on 10th May, 2013             
      ELSE IF   @PriceMin IS NOT NULL AND @PriceMax  IS NOT NULL AND @PriceFlag <> 0 
           
           SET @CarDetail=      'Price Range '
                               +CONVERT(varchar,@PriceMin)
                               + '-' 
                               + CONVERT(varchar,@PriceMax)
      
      -- Modified By : Nilesh Utture on 10th May, 2013                        
      ELSE IF        (@MakeYearFrom IS NOT NULL AND @MakeYearFrom <> 1900)  -- If MakeYearFrom is 1900(Default value) then dont assign @CarDetail
               AND   @MakeYearTo  IS NOT NULL    
           
           SET @CarDetail=       'Year Range '
                                +CONVERT(varchar,@MakeYearFrom)
                                +'-'
                                +CONVERT(varchar,@MakeYearTo)
                   
                                  
      ELSE IF   @FuelType IS NOT NULL    
                        
           SET @CarDetail=       'Fuel Type - '
                                +CONVERT(varchar,@FuelType)
                                    

      RETURN @CarDetail
  END 