IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Microsite_ConversionDetails_Save]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Microsite_ConversionDetails_Save]
GO

	-- =============================================      
-- Author:  Kritika Choudhary
-- Create date:  16th July 2015 
-- Description: To Save Dealer Conversion details  

-- =============================================      
CREATE PROCEDURE [dbo].[Microsite_ConversionDetails_Save]  
(  
 @ID INT,
 @DealerId INT,    
 @ConversionId INT,      
 @ConversionLabel varchar(50),
 @UserId INT,  
 @PageId INT,
 @ModifiedDate datetime=NULL
)      
AS      
BEGIN   

  -- SET NOCOUNT ON added to prevent extra result sets from      
 SET NOCOUNT ON; 
 IF(@ID IS NULL)
 BEGIN  
       INSERT INTO Microsite_DealerConversionDetails(DealerId,ConversionId,ConversionLabel,UserId,PageId)    
	  VALUES(@DealerId,@ConversionId,@ConversionLabel,@UserId,@PageId)
		   
 END 
 ELSE
 BEGIN
	UPDATE Microsite_DealerConversionDetails
	SET ConversionId = @ConversionId, ConversionLabel = @ConversionLabel,ModifiedDate=GETDATE()
	WHERE ID=@ID;
 END

END