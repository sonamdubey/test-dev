IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_RegisterAPI]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_RegisterAPI]
GO

	

-- =============================================
-- Author      : Vinay Kumar Prajapati
-- Create date : 2nd April 2014
-- Description : Save Registration API  
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_RegisterAPI]
(
@DealerId Int,
@UserId VARCHAR(50),
@Password VARCHAR(50)
)
AS
	BEGIN
  
		SELECT AU.DealerId FROM TC_APIUsers AS AU WITH(NOLOCK) WHERE AU.DealerId=@DealerID
		IF @@ROWCOUNT <> 0
			BEGIN
				UPDATE TC_APIUsers SET UserId=@UserId ,Password=@Password WHERE DealerId=@DealerId
			END 
		ELSE
			BEGIN   			        
				INSERT INTO TC_APIUsers(DealerId, UserId,Password,EntryDate) VALUES(@DealerId,@UserId,@Password,CONVERT(VARCHAR(24),GETDATE(),121))
		    END	  
	END
