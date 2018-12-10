IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CT_UpdateAddOnPackageDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CT_UpdateAddOnPackageDetails]
GO

	-- =============================================
-- Author:		Mihir A Chheda
-- Create date: 02-08-2016
-- Description:	used to add/update addonpackages details comes form cartrade
-- =============================================
CREATE PROCEDURE [dbo].[CT_UpdateAddOnPackageDetails] 
@CWDealerId			INT,
@AddOnPackageId		INT,
@StartDate			DATETIME,
@EndDate			DATETIME,
@IsActive           BIT
AS
BEGIN
	declare @curDate datetime= Getdate();
  IF NOT EXISTS (SELECT CAP.Id FROM CT_AddOnPackages(NOLOCK) CAP WHERE CAP.CWDealerId=@CWDealerId AND CAP.AddOnPackageId=@AddOnPackageId )
  BEGIN
      INSERT INTO  CT_AddOnPackages(CWDealerId,AddOnPackageId,StartDate,EndDate,IsActive)
	  VALUES(@CWDealerId,@AddOnPackageId,@StartDate,@EndDate,@IsActive)	 
	  declare @lastRowId int =SCOPE_IDENTITY();
	  begin try
		exec SyncCTAddOnPackagesWithMysql @lastRowId,@CWDealerId,@AddOnPackageId,@StartDate,@EndDate,@curDate,@IsActive;
	end try
	BEGIN CATCH
		INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
		VALUES('MysqlSync','CT_UpdateAddOnPackageDetails',ERROR_MESSAGE(),'SyncCTAddOnPackagesWithMysql',@lastRowId,GETDATE(),null)
	END CATCH			
  END
  ELSE
  BEGIN      
	
      UPDATE CT_AddOnPackages
	  SET    AddOnPackageId=ISNULL(@AddOnPackageId,AddOnPackageId),
	         StartDate=ISNULL(@StartDate,StartDate),
			 EndDate=ISNULL(@EndDate,EndDate),
			 IsActive=ISNULL(@IsActive,IsActive),
			 UpdatedOn=@curDate
	  WHERE	 CWDealerId=@CWDealerId AND AddOnPackageId=@AddOnPackageId
	  begin try
		exec SyncCTAddOnPackagesWithMysqlUpdate @CWDealerId,@AddOnPackageId,@StartDate,@EndDate,@curDate,@IsActive,null,1;
	end try
	BEGIN CATCH
		INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
		VALUES('MysqlSync','CT_UpdateAddOnPackageDetails',ERROR_MESSAGE(),'SyncCTAddOnPackagesWithMysqlUpdate',@CWDealerId,GETDATE(),1)
	END CATCH		
  END
    
   --Log Package Details On New Insert / Update Request 
   INSERT INTO  CT_AddOnPackagesLog(CT_AddOnPackagesId,CWDealerId,AddOnPackageId,StartDate,EndDate,IsActive,CreatedOn,UpdatedOn)
   SELECT	CAP.Id,CAP.CWDealerId,CAP.AddOnPackageId,CAP.StartDate,CAP.EndDate,CAP.IsActive,CAP.CreatedOn,CAP.UpdatedOn
   FROM		CT_AddOnPackages(NOLOCK) CAP 
   WHERE	CAP.CWDealerId=@CWDealerId AND CAP.AddOnPackageId=@AddOnPackageId
END

