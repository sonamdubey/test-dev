IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UsedCarScoreCalc]') 
    AND xtype IN (N'FN', N'IF', N'TF')
)
    DROP FUNCTION [dbo].[UsedCarScoreCalc]
GO

	
-- ============================================= 
-- Author:    Avishkar/Manish 
-- Create date: 15-nov-12 
-- Description:  Calculate the score of the used car. 
-- ============================================= 
----------------Need to discuss with satish sharma regarding the values of owners field 
CREATE FUNCTION [dbo].[UsedCarScoreCalc](@inquiryid BIGINT, 
                                        @isdealer  BIT) 
returns FLOAT 
AS 
  BEGIN 
      DECLARE @price      BIGINT, 
              @kms        BIGINT, 
              @photocount TINYINT, 
              @owners     TINYINT, 
              @fueltype   DECIMAL(1), 
              @ADDPARAM   FLOAT = 112.7, 
              @PHOTOMULT  FLOAT=1.72, 
              @KMSMULT    FLOAT=0.0003675, 
              @PRICEMULT  FLOAT= 0.00003002, 
              @OWNERMULT  FLOAT=9.092, 
              @FUELMULT   FLOAT=23.69, 
              @SCORE      FLOAT 

      IF @isdealer = 1 
        BEGIN 
            SELECT @price = SI.price, 
                   @kms = SI.kilometers, 
                   @FUELTYPE = CASE CV.carfueltype 
                                 WHEN 2 THEN 1 
                                 ELSE 0 
                               END, 
                   @owners = SD.owners 
            FROM   sellinquiries AS SI WITH (nolock) 
                   INNER JOIN carversions AS CV WITH (nolock) 
                           ON SI.carversionid = CV.id 
                   INNER JOIN sellinquiriesdetails SD WITH (nolock) 
                           ON SI.id = SD.sellinquiryid 
            WHERE  SI.id = @inquiryid 
                   AND CV.isdeleted = 0 
                   -- Discuss with Satish Sharma 
                   AND Isnumeric(sd.owners) = 1 
                   AND SD.owners <>- 1 

            SET @PHOTOCOUNT= (SELECT Count(*) 
                              FROM   carphotos AS CP WITH (nolock) 
                                     INNER JOIN sellinquiries AS SI WITH (nolock 
                                                ) 
                                             ON CP.inquiryid = SI.id 
                                                AND SI.id = @inquiryid 
                                                AND isdealer = 1 
                                                AND isactive = 1 
                                                AND isapproved = 1 
                              GROUP  BY CP.inquiryid) 
        END 
      ELSE 
        BEGIN 
            SELECT @price = CSI.price, 
                   @kms = CSI.kilometers, 
                   @FUELTYPE = CASE CV.carfueltype 
                                 WHEN 2 THEN 1 
                                 ELSE 0 
                               END, 
                   @owners = CSD.owners 
            FROM   customersellinquiries AS CSI WITH (nolock) 
                   INNER JOIN carversions AS CV WITH (nolock) 
                           ON CSI.carversionid = CV.id 
                   INNER JOIN customersellinquirydetails CSD WITH (nolock) 
                           ON CSI.id = CSD.inquiryid 
            WHERE  CSI.id = @inquiryid 
                   AND CSI.isapproved = 1 
                   AND CSI.isfake = 0 
                   AND CV.isdeleted = 0 
                  -- AND Isnumeric(csd.owners) = 1 
                   --AND csd.owners <>- 1 

            SET @PHOTOCOUNT= (SELECT Count(*) 
                              FROM   carphotos AS CP WITH (nolock) 
                                     INNER JOIN customersellinquiries AS CSI 
                                                WITH 
                                                ( 
                                                nolock) 
                                             ON CP.inquiryid = CSI.id 
                                                AND CSI.id = @inquiryid 
                                                AND CSI.isapproved = 1 
                                                AND CSI.isfake = 0 
                                                AND CP.isdealer = 0 
                                                AND CP.isactive = 1 
                                                AND CP.isapproved = 1 
                              GROUP  BY CP.inquiryid) 
        END 

 if isnumeric(@owners)=0
            set @owners=0
            if (@owners is null or @owners=-1)
            SET @owners=5

      SET @score=@ADDPARAM + @PHOTOMULT * Isnull(@photocount, 0) - 
                 @KMSMULT * Isnull(@kms, 0) 
                            - @PRICEMULT * Isnull(@Price, 0) - 
                 @OWNERMULT * Isnull(@Owners, 0) + @FUELMULT * Isnull(@fueltype, 
                                                               0 
                                                               ); 

      RETURN @score 
  END 
