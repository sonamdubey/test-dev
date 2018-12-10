IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UsedCarResponseUpdate]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UsedCarResponseUpdate]
GO

	-- ============================================= 
-- Author:    Manish 
-- Create date: 20-nov-12 
-- Description: Update the Response of the used car. 
-- Modified by Manish on  08-04-2014 added WITH (NOLOCK) keyword in queries.
-- Modified by Manish on  14-04-2014 Update response as response +1
-- Modified By: Avishkar on 10-4-2014  To set lead score
 --Modified by: Manish on 12-08-2014 commented the sp ComputeLLSortScore execution since it will execute through scheduled job.
-- ============================================= 

CREATE PROCEDURE [dbo].[UsedCarResponseUpdate] @inquiryid  AS BIGINT, 
                                       @Sellertype AS VARCHAR(1) 
AS 
  BEGIN 
      DECLARE @ProfileId VARCHAR(50)
     
      --IF @SELLERTYPE = 1 
      --BEGIN
        UPDATE livelistings  
        SET    responses = ISNULL(responses,0)+1		                  
        WHERE  inquiryid = @inquiryid 
           AND sellertype = @SELLERTYPE 
        
        SELECT  @ProfileId= CASE @SELLERTYPE WHEN 1 THEN 'D'+CAST(@inquiryid AS VARCHAR(50))ELSE 'S'+CAST(@inquiryid AS VARCHAR(50)) END
        
        --Modified By: Avishkar on 10-4-2014  To set lead score 
      --  EXEC ComputeLLSortScore @ProfileId   -- Commented by Manish on 12-08-2014
       
       --END
     
               
  END 

