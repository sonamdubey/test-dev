IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_DealerVersions_Save]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_DealerVersions_Save]
GO

	
-- =============================================      
-- Author:  Kritika Choudhary
-- Create date:  1st June 2015 
-- Description: To Save Dealer Versions  

-- =============================================      
CREATE PROCEDURE [dbo].[TC_DealerVersions_Save]  
(  
 @ID INT,    
 @DealerId INT,      
 @CWVersionId INT,
 @DWVersionName VARCHAR(50),  
 @DWModelId INT
)      
AS      
BEGIN   

  -- SET NOCOUNT ON added to prevent extra result sets from      
 SET NOCOUNT ON; 
 IF(@ID IS NULL)
 BEGIN  
       INSERT INTO TC_DealerVersions(DealerId,CWVersionId,DWVersionName,DWModelId)    
		VALUES(@DealerId,@CWVersionId,@DWVersionName,@DWModelId)
		   
 END 
ELSE
 BEGIN
	UPDATE TC_DealerVersions
	SET CWVersionId = @CWVersionId, DWVersionName = @DWVersionName,ModifiedDate=GETDATE()
	WHERE ID=@ID;
 END
END
