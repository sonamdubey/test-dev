IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[SyncClassifiedCertifiedOrgWithMysql]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[SyncClassifiedCertifiedOrgWithMysql]
GO

	-- =============================================
-- Author:		<Author,,Prasad Gawde>
-- Create date: <Create Date,,9/11/2016>
-- Description:	<Description,,to sync with mysql>
-- =============================================
CREATE PROCEDURE [dbo].[SyncClassifiedCertifiedOrgWithMysql]
	(
	@Id int,
	@CertifiedOrgName VARCHAR(100),
	@LogoURL VARCHAR(100),
	@Description VARCHAR(MAX),
	@HostURL varchar(100),
	@DirectoryPath varchar(100),
	@Advantages varchar(max),
	@Criteria varchar(max),
	@CoreBenefits varchar(max),
	@CheckPoints varchar(max),
	@WarrantyServices varchar(max),
	@OriginalImgPath varchar(250)
	)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
begin try	
INSERT INTO mysql_test...classified_certifiedorg (Id, CertifiedOrgName,LogoURL,Description,
				IsActive,HostURL,IsReplicated,DirectoryPath,Advantages,Criteria,CoreBenefits,
				CheckPoints,WarrantyServices, OriginalImgPath)
		    VALUES
				(@Id, @CertifiedOrgName,@LogoUrl,@Description,1,@HostUrl,0,@DirectoryPath,@Advantages,
				 @Criteria,@CoreBenefits,@CheckPoints,@WarrantyServices, @OriginalImgPath)
 	end try
	BEGIN CATCH
		INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
		VALUES('MysqlSync','SyncClassifiedCertifiedOrgWithMysql',ERROR_MESSAGE(),'classified_certifiedorg',@Id,GETDATE(),null)
	END CATCH	
END

