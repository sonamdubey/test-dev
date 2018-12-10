IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[SyncCarModelRootsWithMysql]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[SyncCarModelRootsWithMysql]
GO

	-- =============================================
-- Author:		Pratik Vasa
-- Create date: 23/9/16
-- Description:	added to sync Carwale sqlserver with Mysql
-- =============================================
CREATE PROCEDURE  [dbo].[SyncCarModelRootsWithMysql] --1,'name',1,2
	-- Add the parameters for the stored procedure here
	@RootId int,
	@RootName varchar(80) ,
	@MakeId smallint,
	@IsInsert int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	BEGIN TRY
	if(@IsInsert=1)
	begin
		 INSERT INTO mysql_test...carmodelroots(RootId ,
		RootName ,
		MakeId)  
		VALUES (@RootId ,
		@RootName ,
		@MakeId
		);
	end;
	else
	begin
		UPDATE mysql_test...carmodelroots 
            SET    rootname = @RootName, 
                   makeid = @MakeId 
            WHERE  rootid = @RootId 
	end;
	end try
	BEGIN CATCH
		INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
		VALUES('MysqlSync','SyncCarModelRootsWithMysql',ERROR_MESSAGE(),'CarModelRoots',@RootId,GETDATE(),@IsInsert)
	END CATCH
END

