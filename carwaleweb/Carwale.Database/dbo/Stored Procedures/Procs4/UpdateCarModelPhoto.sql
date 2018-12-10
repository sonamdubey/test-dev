IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UpdateCarModelPhoto]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UpdateCarModelPhoto]
GO

	-- =============================================
-- Author:		Pratik Vasa
-- Create date: 23/9/16
-- Description:	added to sync Carwale sqlserver with Mysql
-- =============================================
CREATE PROCEDURE [dbo].[UpdateCarModelPhoto] 
	-- Add the parameters for the stored procedure here
	@Id int,
	@SmallPic varchar(200),
	@LargePic varchar(200),
	@HostUrl varchar(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    UPDATE CarModels SET IsReplicated = 0,
				 SmallPic=@SmallPic,
				 LargePic=@LargePic,
				 HostURL =@HostUrl
				 WHERE ID=@Id
begin try
 exec SyncCarModelsWithMysqlUpdate null  ,
	null  ,
	null  ,
	null  ,
	null ,
	null ,
	
	null ,
	null ,
	null ,
	null ,
	null ,
	null ,
	null ,
	@Id ,
	@SmallPic ,
	@LargePic ,
	@HostUrl ,
	4
		end try
			BEGIN CATCH
				INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
				VALUES('MysqlSync','[UpdateCarModelPhoto]',ERROR_MESSAGE(),'[SyncCarModelsWithMysqlUpdate]',@Id,GETDATE(),4)
			END CATCH	
END

