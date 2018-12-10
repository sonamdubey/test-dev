IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Usedcarupdatedateflag]') 
    AND xtype IN (N'FN', N'IF', N'TF')
)
    DROP FUNCTION [dbo].[Usedcarupdatedateflag]
GO

	-- =============================================       
-- Author:    Avishkar/Manish/Reshma     
-- Create date: 23-nov-12       
-- Description:  Return the flag for need of update of lastupdateddate in livelistings table    
-- AM  modified 28-11-2012 to use ISNULL(@OldDesclength,0)-ISNULL(@desclength,0)   
-- =============================================       
CREATE FUNCTION [dbo].[Usedcarupdatedateflag](@inquiryid  BIGINT,     
                                              @sellertype   varchar(1),     
                                              @price      BIGINT,     
                                              @photocount TINYINT,     
                                              @desclength SMALLINT)     
returns BIT     
AS     
  BEGIN     
      DECLARE @updatebit     AS BIT,     
              @OldPrice      AS BIGINT,     
              @OldPhotocount AS TINYINT,     
              @OldDesclength AS SMALLINT     
    
      SELECT @OldPrice = price,     
             @OldPhotocount = photocount,     
             @OldDesclength = Len(comments)     
      FROM   livelistings     
      WHERE  inquiryid = @inquiryid     
             AND sellertype = @sellertype     
    
      SET @updatebit=0     
    
      -- AM  modified 28-11-2012 to use ISNULL(@OldDesclength,0)-ISNULL(@desclength,0)  
      IF Abs(@OldPrice - @price) >= ( @OldPrice * .01 )     
          OR @photocount <> @OldPhotocount     
          OR ISNULL(@OldDesclength,0)-ISNULL(@desclength,0) >= 10     
        SET @updatebit=1     
    
      RETURN @updatebit     
  END 