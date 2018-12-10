IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[SyncCityWithMysqlUpdate]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[SyncCityWithMysqlUpdate]
GO

	-- =============================================
-- Author:		Pratik Vasa
-- Create date: 21/10/2016
-- Description:	added to sync Carwale sqlserver with Mysql
-- =============================================
CREATE PROCEDURE  [dbo].[SyncCityWithMysqlUpdate]
	-- Add the parameters for the stored procedure here
	@CityId INT,
	@CityName VARCHAR(50),
	@StateId INT ,
	@Lattitude DECIMAL(18,4),
	@Longitude DECIMAL(18,4),
	@PinCode VARCHAR(10),
	@StdCode numeric,
	@UpdateType int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	BEGIN TRY
    if @UpdateType = 1
	begin
	UPDATE mysql_test...cities SET 
                    Name = @CityName,
                    Lattitude = @Lattitude,
                    Longitude = @Longitude,
                    DefaultPinCode = @PinCode
                    WHERE Id=@CityId
	end
	else if @UpdateType =2 
	begin
		UPDATE mysql_test...cities SET IsDeleted=1 WHERE Id=@CityId;
	end
	else if @UpdateType = 3
	begin
		 UPDATE mysql_test...cities SET StdCode = @StdCode WHERE Id = @CityId;
	end
		 				end try
	BEGIN CATCH
		INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
		VALUES('MysqlSync','SyncCityWithMysqlUpdate',ERROR_MESSAGE(),'Cities',@CityId,GETDATE(),@UpdateType)
	END CATCH
END

