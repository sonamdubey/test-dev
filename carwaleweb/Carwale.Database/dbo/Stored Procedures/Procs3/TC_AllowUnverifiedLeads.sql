IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_AllowUnverifiedLeads]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_AllowUnverifiedLeads]
GO

	-- =============================================
-- Author:		Vivek Gupta
-- Create date: 19-01-2015
-- Description: To save permission of dealer if he wants to receive unverified leads or not
--Modified By Deepak on 9th Sept 2016 - Addon Package for unverified leads
-- =============================================
CREATE PROCEDURE [dbo].[TC_AllowUnverifiedLeads]
	@BranchId INT,
	@SendUnVerifiedLead BIT = 0,
	@SendSMSUnVerifiedLead BIT = 0,
	@SendEmailUnVerifiedLead BIT = 0
AS
BEGIN	
	SET NOCOUNT ON;
	IF @BranchId IS NOT NULL
		BEGIN
			--IF @SendUnVerifiedLead = 1 AND NOT EXISTS (SELECT Id FROM TC_MappingDealerFeatures WITH(NOLOCK) WHERE BranchId = @BranchId AND TC_DealerFeatureId =  4)
			IF @SendUnVerifiedLead = 1 --Added By Deepak on 9th Sept 2016 
				BEGIN
					IF NOT EXISTS (SELECT Id FROM CT_AddOnPackages WITH(NOLOCK) WHERE CWDealerId = @BranchId AND AddOnPackageId =  101)
						BEGIN
							declare @curdate datetime =GetDate();
							--INSERT INTO TC_MappingDealerFeatures (BranchId, TC_DealerFeatureId)	VALUES (@BranchId, 4)
							INSERT INTO CT_AddOnPackages(AddOnPackageId, CWDealerId, StartDate, EndDate, IsActive, CreatedOn)
							VALUES(101, @BranchId, @curdate, @curdate, 1, @curdate) --Added By Deepak on 9th Sept 2016
							declare @lastRowId int =SCOPE_IDENTITY();
							--mysql syncing
							begin try
								exec SyncCTAddOnPackagesWithMysql @lastRowId,@BranchId,101,@curdate,@curdate,@curdate,1;
							end try
							BEGIN CATCH
								INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
								VALUES('MysqlSync','TC_AllowUnverifiedLeads',ERROR_MESSAGE(),'SyncCTAddOnPackagesWithMysql',@lastRowId,GETDATE(),1)
							END CATCH		
						END
					ELSE
						BEGIN
							UPDATE CT_AddOnPackages SET IsActive = 1 WHERE CWDealerId = @BranchId AND AddOnPackageId = 101
							--mysql syncing
							BEGIN TRY
								exec SyncCTAddOnPackagesWithMysqlUpdate @BranchId,101,null,null,null,1,null,2;
							end try
							BEGIN CATCH
								INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
								VALUES('MysqlSync','TC_AllowUnverifiedLeads',ERROR_MESSAGE(),'SyncCTAddOnPackagesWithMysqlUpdate',@BranchId,GETDATE(),2)
							END CATCH	
						END
				END
			ELSE IF @SendUnVerifiedLead = 0
				BEGIN
					--DELETE FROM TC_MappingDealerFeatures WHERE BranchId = @BranchId AND TC_DealerFeatureId = 4
					--DELETE FROM CT_AddOnPackages WHERE CWDealerId = @BranchId AND AddOnPackageId = 101 --Added By Deepak on 9th Sept 2016
					UPDATE CT_AddOnPackages SET IsActive = 0 WHERE CWDealerId = @BranchId AND AddOnPackageId = 101
					--mysql syncing
					begin try
						exec SyncCTAddOnPackagesWithMysqlUpdate @BranchId,101,null,null,null,0,null,2;
					end try
					BEGIN CATCH
						INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
						VALUES('MysqlSync','TC_AllowUnverifiedLeads',ERROR_MESSAGE(),'SyncCTAddOnPackagesWithMysqlUpdate',@BranchId,GETDATE(),2)
					END CATCH	
				END
			IF @SendSMSUnVerifiedLead = 1  AND NOT EXISTS (SELECT Id FROM TC_MappingDealerFeatures WITH(NOLOCK) WHERE BranchId = @BranchId AND TC_DealerFeatureId =  5)
				BEGIN
					INSERT INTO TC_MappingDealerFeatures (BranchId, TC_DealerFeatureId)
					VALUES (@BranchId, 5)
				END
			ELSE  IF @SendSMSUnVerifiedLead = 0
				BEGIN
					 DELETE FROM TC_MappingDealerFeatures 
					 WHERE BranchId = @BranchId AND TC_DealerFeatureId = 5
				END
			IF @SendEmailUnVerifiedLead = 1  AND NOT EXISTS (SELECT Id FROM TC_MappingDealerFeatures WITH(NOLOCK) WHERE BranchId = @BranchId AND TC_DealerFeatureId =  6)
				BEGIN
					INSERT INTO TC_MappingDealerFeatures (BranchId, TC_DealerFeatureId)
					VALUES (@BranchId, 6)
				END
			ELSE IF @SendEmailUnVerifiedLead = 0
			  BEGIN
				 DELETE FROM TC_MappingDealerFeatures 
				 WHERE BranchId = @BranchId AND TC_DealerFeatureId = 6
			  END
		END
END

