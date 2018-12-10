IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UpdateArea]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UpdateArea]
GO

	-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[UpdateArea]
	-- Add the parameters for the stored procedure here
	@AreaId INT, 
	@AreaName VARCHAR(50),
	@Lattitude DECIMAL(18,4),
	@Longitude DECIMAL(18,4),
	@PinCode VARCHAR(10)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	 UPDATE Areas SET 
				 Name = @AreaName,
				 PinCode = @PinCode,
				 Lattitude = @Lattitude,
				 Longitude = @Longitude
				 WHERE ID = @AreaId;
	begin try
		exec SyncAreasWithMysqlUpdate @AreaId, @AreaName, @Lattitude, @Longitude, @PinCode, 1
	end try
	BEGIN CATCH
		INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
		VALUES('MysqlSync','UpdateArea',ERROR_MESSAGE(),'SyncCarMoSyncAreasWithMysqlUpdatedelsWithMysql',@AreaId,GETDATE(),1)
	END CATCH	
END

