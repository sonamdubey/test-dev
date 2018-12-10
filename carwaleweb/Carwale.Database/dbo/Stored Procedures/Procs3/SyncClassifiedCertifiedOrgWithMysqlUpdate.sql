IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[SyncClassifiedCertifiedOrgWithMysqlUpdate]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[SyncClassifiedCertifiedOrgWithMysqlUpdate]
GO

	-- =============================================
-- Author:		<Author,,Prasad Gawde>
-- Create date: <Create Date,,9/11/2016>
-- Description:	<Description,,to sync with mysql>
-- =============================================
CREATE PROCEDURE [dbo].[SyncClassifiedCertifiedOrgWithMysqlUpdate]
	(
	@Id int,
	@LogoURL VARCHAR(100),
	@Description VARCHAR(MAX),
	@HostURL varchar(100),
	@Advantages varchar(max),
	@Criteria varchar(max),
	@CoreBenefits varchar(max),
	@CheckPoints varchar(max),
	@WarrantyServices varchar(max),
	@OriginalImgPath varchar(250),
	@Environment VARCHAR(50) = '',
	@UpdateType int 
	)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	begin try
	if @UpdateType=1
		UPDATE mysql_test...classified_certifiedorg SET HostURL=@HostUrl, 
											   IsReplicated =1,
											   DirectoryPath = '',
											   LogoURL = '/0X0/' + @Environment + OriginalImgPath,
											   OriginalImgPath = @Environment + OriginalImgPath
				WHERE Id=@Id
	else if @UpdateType=2						   
											   
		UPDATE mysql_test...classified_certifiedorg SET
				Description = @Description,
				Advantages = @Advantages,
				Criteria = @Criteria,
				CoreBenefits = @CoreBenefits,
				CheckPoints = @CheckPoints,
				WarrantyServices = @WarrantyServices,
				LogoURL         = @LogoUrl,
				OriginalImgPath = @OriginalImgPath
			WHERE Id = @Id
 	end try
	BEGIN CATCH
		INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
		VALUES('MysqlSync','SyncClassifiedCertifiedOrgWithMysqlUpdate',ERROR_MESSAGE(),'classified_certifiedorg',@Id,GETDATE(),@UpdateType)
	END CATCH	
END

