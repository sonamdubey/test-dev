IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CarModelMemberUpdate]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CarModelMemberUpdate]
GO

	-- =============================================
-- Author:		Pratik Vasa
-- Create date: 17/10/2016
-- Description:	added to sync Carwale sqlserver with Mysql
-- =============================================
CREATE PROCEDURE [dbo].[CarModelMemberUpdate]
	-- Add the parameters for the stored procedure here
	@Name varchar(30),
	@Used bit,
	@New bit,
	@Indian bit,
	@Imported bit, 
	@Classic bit,
	@Modified bit, 
	@Futuristic bit,
	@MoUpdatedOn datetime,
	@MoUpdatedBy int,
	@Id int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
   UPDATE CarModels SET 
                Name=@Name,
                Used=@Used,
                New=@New,
                Indian=@Indian,
                Imported=@Imported,
                Classic=@Classic,
                Modified=@Modified,
                Futuristic=@Futuristic,
                MoUpdatedOn=@MoUpdatedOn,
                MoUpdatedBy=@MoUpdatedBy
                WHERE Id=@Id
	begin try
		exec SyncCarModelsWithMysqlUpdate @Name  ,
		null  ,
		null  ,
		null  ,
		@MoUpdatedOn ,
		@MoUpdatedBy ,
		@Used ,
		@New ,
		@Indian ,
		@Imported ,
		@Classic ,
		@Futuristic ,
		@Modified ,
		@Id ,
		null ,
		null ,
		null ,
		1
	end try
	BEGIN CATCH
		INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
		VALUES('MysqlSync','CarModelMemberUpdate',ERROR_MESSAGE(),'SyncCarModelsWithMysqlUpdate',@Id,GETDATE(),1)
	END CATCH			
END

