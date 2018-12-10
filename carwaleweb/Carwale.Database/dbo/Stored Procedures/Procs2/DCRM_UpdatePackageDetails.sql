IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_UpdatePackageDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_UpdatePackageDetails]
GO

	-- =============================================          
-- Author:  <Dipti Bhoir>          
-- Create date: <24-feb-2012>          
-- Description: <This sp used to insert log details and update existing package details.>          
-- =============================================  
CREATE PROCEDURE [dbo].[DCRM_UpdatePackageDetails]  
   
 @ConsumerId			NUMERIC,   
 @ActualValidity		INT,  
 @ActualInquiryPoints   INT,  
 @ActualAmount			NUMERIC,   
 @ExpDate				DATETIME,  
 @EnteredById			NUMERIC,  
 @CCPId					NUMERIC,
 @CPRId					NUMERIC,
 @Status				INTEGER OUTPUT --return value, -1 for unsuccessfull attempt, and 1 for success  
   
 AS  
   
BEGIN  
   
 UPDATE ConsumerPackageRequests SET  ActualValidity=@ActualValidity,   
   ActualInquiryPoints=@ActualInquiryPoints, ActualAmount=@ActualAmount
   WHERE Id=@CPRId  
   
   --mysql sync
	begin try
		exec SyncConsumerPackageRequestsWithMysqlUpdate null,null,null,@ActualValidity,@ActualInquiryPoints,@ActualAmount,null,null,null,null,null,null,@CPRId,4
 	end try
	BEGIN CATCH
		INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
		VALUES('MysqlSync','DCRM_UpdatePackageDetails',ERROR_MESSAGE(),'SyncConsumerPackageRequestsWithMysqlUpdate',@CPRId,GETDATE(),4)
	END CATCH	
 UPDATE ConsumerCreditPoints SET Points = @ActualInquiryPoints, ExpiryDate = @ExpDate 
 WHERE Id=@CCPId  
 --mysql syncing
 begin try
	exec SyncConsumerCreditPointsWithMysqlUpdate null,null,@ActualInquiryPoints,null,@ExpDate,null,null,null,7,@CCPId
end try
BEGIN CATCH
	INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
	VALUES('MysqlSync','DCRM_UpdatePackageDetails',ERROR_MESSAGE(),'SyncConsumerCreditPointsWithMysqlUpdate',@CPRId,GETDATE(),7)
END CATCH	
 INSERT INTO DCRM_TrackPkgUpdate(ReqId, DealerId, PkgUpdatedOn, PkgUpdatedBy) VALUES (@CPRId, @ConsumerId, GETDATE(),@EnteredById)  
 SET @Status = 1  
    
END  
  

