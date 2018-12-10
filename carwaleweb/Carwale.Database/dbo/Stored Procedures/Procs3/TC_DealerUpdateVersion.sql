IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_DealerUpdateVersion]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_DealerUpdateVersion]
GO

	
-- =============================================  
-- Author:  Kritika Choudhary
-- Create date: 2nd June 2015
-- Description: To Activate And DeActivate Dealer Car Version  
-- =============================================  
CREATE PROCEDURE [dbo].[TC_DealerUpdateVersion]   
(  
 @Id INT
)  
AS  
DECLARE @Activation BIT 
BEGIN   
	
	SET @Activation = (SELECT IsDeleted FROM TC_DealerVersions WHERE ID = @Id)
	
	IF(@Activation = 0)
		BEGIN  
			UPDATE TC_DealerVersions SET IsDeleted=1
			WHERE ID=@Id 
		END 
	ELSE
		BEGIN
			UPDATE TC_DealerVersions SET IsDeleted=0
			WHERE ID=@Id 
		END 
END  

