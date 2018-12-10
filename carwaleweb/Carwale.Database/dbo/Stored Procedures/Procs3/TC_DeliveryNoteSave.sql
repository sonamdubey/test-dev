IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_DeliveryNoteSave]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_DeliveryNoteSave]
GO
	  
-- =============================================  
-- Author:  <Author: Nilesh Utture>  
-- Create date: <Create Date: 26/11/2012>  
-- Description: <Description: Saves Delivery note details for specific to dealers>  
-- =============================================  
CREATE PROCEDURE [dbo].[TC_DeliveryNoteSave]   
 -- Add the parameters for the stored procedure here  
 @BranchId BIGINT,  
 @Content VARCHAR(700)  
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT OFF;  
  
    -- Insert statements for procedure here  
 SELECT DealerId FROM TC_DeliveryNoteDetails WITH (NOLOCK) WHERE DealerId = @BranchId  
   
 IF @@ROWCOUNT = 0  
  BEGIN  
   INSERT INTO TC_DeliveryNoteDetails (DealerId, content) VALUES (@BranchId, @Content)  
  END  
 ELSE  
  BEGIN  
   UPDATE TC_DeliveryNoteDetails SET content = @Content WHERE DealerId = @BranchId  
  END  
END  