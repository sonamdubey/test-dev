IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[SyncCarModelsWithMysql]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[SyncCarModelsWithMysql]
GO

	-- =============================================
-- Author:		Pratik Vasa
-- Create date: 23/9/16
-- Description:	added to sync Carwale sqlserver with Mysql
-- =============================================
CREATE PROCEDURE  [dbo].[SyncCarModelsWithMysql] 
	-- Add the parameters for the stored procedure here
	@ID numeric(18, 0) ,
	@Name varchar(30) ,
	@CarMakeId numeric(18, 0) ,
	@IsDeleted bit ,
	@Used bit ,
	@New bit ,
	@Indian bit ,
	@Imported bit ,
	@Futuristic bit ,
	@Classic bit ,
	@Modified bit ,	
	@MoUpdatedBy numeric(18, 0) ,
	@comment varchar(5000) ,
	@ReplacedByModelId smallint ,
	@MaskingName varchar(50) ,
	@RootId smallint ,
	@Platform varchar(500) ,
	@Generation tinyint ,
	@Upgrade tinyint ,
	@ModelLaunchDate datetime,
	@moCreatedOn datetime =null,
	@InsertType int =1
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	BEGIN TRY
	--insert into prasadtemp values('reached [[SyncCarModelsWithMysql]]');
	
	if @inserttype=1 
		INSERT INTO mysql_test...carmodels 
				   (Id,
					name, 
					carmakeid, 
					isdeleted, 
					mocreatedon, 
					moupdatedby, 
					used, 
					new, 
					indian, 
					imported, 
					classic, 
					modified, 
					futuristic, 
					maskingname, 
					rootid, 
					platform, 
					generation, 
					upgrade, 
					ModelLaunchDate, 
					comment,
					ReplacedByModelId) --added by Khushaboo Patil on 2 Apr 
		VALUES     (@Id,
					@Name, 
					@CarMakeId, 
					@IsDeleted, 
					Getdate(), 
					@MoUpdatedBy, 
					@Used, 
					@New, 
					@Indian, 
					@Imported, 
					@Classic, 
					@Modified, 
					@Futuristic, 
					@maskingname, 
					@RootId, 
					@Platform, 
					@Generation, 
					@Upgrade, 
					@ModelLaunchDate, 
					@comment,
					@ReplacedByModelId)
	else if @InsertType=2
					 INSERT INTO  mysql_test...carmodels (Id, Name, CarMakeId, IsDeleted,MoCreatedOn,MoUpdatedBy )  
                 VALUES (@Id,@name,@carMakeId,@isDeleted,@moCreatedOn,@moUpdatedBy) 
		
	end try
	BEGIN CATCH
		INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
		VALUES('MysqlSync','SyncCarModelsWithMysql',ERROR_MESSAGE(),'CarModels',@Id,GETDATE(),@InsertType)
	END CATCH
END

