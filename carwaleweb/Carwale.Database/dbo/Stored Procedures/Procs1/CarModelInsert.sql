IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CarModelInsert]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CarModelInsert]
GO

	-- =============================================
-- Author:		Pratik Vasa
-- Create date: 17/10/2016
-- Description:	added to sync Carwale sqlserver with Mysql
-- =============================================
CREATE PROCEDURE [dbo].[CarModelInsert] 
	-- Add the parameters for the stored procedure here
	@name varchar(30),
	@carMakeId int,
	@IsDeleted bit,
	@moCreatedOn datetime,
	@moUpdatedBy int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	declare @ID int;
     INSERT INTO CarModels( Name, CarMakeId, IsDeleted,MoCreatedOn,MoUpdatedBy )  
                 VALUES (@name,@carMakeId,@isDeleted,@moCreatedOn,@moUpdatedBy) 
	
		set @Id = scope_identity()
		declare 
		@Used bit =null,
		@New bit  =null,
		@Indian bit =null,
		@Imported bit =null,
		@Futuristic bit =null,
		@Classic bit =null,
		@Modified bit =null,	
		@comment varchar(5000) =null,
		@ReplacedByModelId smallint =null,
		@MaskingName varchar(50) =null,
		@RootId smallint =null,
		@Platform varchar(500) =null,
		@Generation tinyint =null,
		@Upgrade tinyint =null,
		@ModelLaunchDate datetime =null,
		@InsertType int =2
		begin try
		
			exec [dbo].[SyncCarModelsWithMysql] 
		-- Add the parameters for the stored procedure here
			@ID,
			@Name,
			@CarMakeId ,
			@IsDeleted ,
			@Used  ,
			@New  ,
			@Indian  ,
			@Imported  ,
			@Futuristic  ,
			@Classic  ,
			@Modified  ,	
			@MoUpdatedBy  ,
			@comment ,
			@ReplacedByModelId ,
			@MaskingName,
			@RootId,
			@Platform ,
			@Generation ,
			@Upgrade,
			@ModelLaunchDate,
			@moCreatedOn,
			@InsertType
	end try
	BEGIN CATCH
		INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
		VALUES('MysqlSync','CarModelInsert',ERROR_MESSAGE(),'SyncCarModelsWithMysql',@Id,GETDATE(),null)
	END CATCH			
END

