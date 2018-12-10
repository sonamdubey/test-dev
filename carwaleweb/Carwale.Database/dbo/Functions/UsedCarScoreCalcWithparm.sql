IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UsedCarScoreCalcWithparm]') 
    AND xtype IN (N'FN', N'IF', N'TF')
)
    DROP FUNCTION [dbo].[UsedCarScoreCalcWithparm]
GO

	-- =============================================  
-- Author:    Manish  
-- Create date: 16-nov-12  
-- Description:  Calculate the score of the used car when all the prameters are available.  
----------------Need to discuss with satish sharma regarding the values of owners field
-- =============================================  
--Modified By
-- =============================================  
-- Author:   Reshma Shetty 
-- Create date: 16-jan-13  
-- Description:  Modified as per the latest changes in the score calculation algorithm.
--              Parts of the code no longer required have been commented.
-- ============================================= 

  
CREATE FUNCTION [dbo].[UsedCarScoreCalcWithparm]
(@price     BIGINT, 
@kms        BIGINT, 
@photocount TINYINT
--@owners     TINYINT, 
--@fueltype   TINYINT
) 
returns FLOAT 
AS 
  BEGIN 
  --new_score=0.27459 -0.000000966583*Kilometers -0.0000000291944*Price+0.07408*photo_grp
  --old_score=112.7 + 1.72*photo_count -0.0003675*Kilometers -0.00003002*Price-9.092*Owners+23.69*diesel_type
  
      DECLARE @addparam  FLOAT = 0.27459, 
              @photomult FLOAT=  0.07408, 
              @kmsmult   FLOAT=  0.000000966583, 
              @pricemult FLOAT=  0.0000000291944,  
              @photo_grp INT,
              @score     FLOAT 
              --@ownermult FLOAT=  9.092, 
              --@fuelmult  FLOAT=  23.69,

            -- SET @fueltype=CASE @fueltype 
            --                     WHEN 2 THEN 1 
            --                     ELSE 0 
            --                   END
            
            --IF isnumeric(@owners)=0
            --SET @owners=0
            --IF (@owners is null or @owners=-1)
            --SET @owners=5
            SET @photo_grp=CASE 
                            WHEN @photocount IS NULL THEN 1
							WHEN @photocount=0 THEN 1
							WHEN @photocount BETWEEN 1 AND 5 THEN 2
							WHEN @photocount > 5 THEN 3
							
							END
            

            SET @score = @addparam + @photomult * @photo_grp - @kmsmult * Isnull(@kms, 0) - @pricemult * Isnull(@Price, 0); -- -  @ownermult * Isnull(@Owners, 0) + @fuelmult * Isnull(@fueltype, 0); 

      RETURN @score 
  END 

