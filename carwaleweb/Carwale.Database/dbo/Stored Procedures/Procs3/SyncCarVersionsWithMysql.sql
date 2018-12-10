IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[SyncCarVersionsWithMysql]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[SyncCarVersionsWithMysql]
GO

	-- =============================================
-- Author:		Pratik Vasa
-- Create date: 23/9/16
-- Description:	added to sync Carwale sqlserver with Mysql
-- =============================================
CREATE PROCEDURE  [dbo].[SyncCarVersionsWithMysql] --12345,'new car version', 55555,1,1,1,1,1,1,1,1,1,1,1,1,'10-10-2016','12-10-2016','mname',1,12,'10-10-2016'
	-- Add the parameters for the stored procedure here
	@ID numeric(18, 0),
	@Name varchar(50) ,
	@CarModelId numeric(18, 0) ,
	@SegmentId numeric(18, 0) ,
	@BodyStyleId numeric(18, 0) ,
	@IsDeleted bit ,
	@Used bit ,
	@New bit ,
	@Indian bit ,
	@Imported bit ,
	@Futuristic bit ,
	@Classic bit ,
	@Modified bit ,	
	@CarFuelType tinyint ,
	@CarTransmission tinyint ,
	@VCreatedOn datetime ,		
	@Discontinuation_date datetime ,	
	@MaskingName varchar(50) ,
	@SubSegmentId numeric,
	@vcreatedby numeric(18, 0) ,
	@LaunchDate datetime 
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	BEGIN TRY
	INSERT INTO mysql_test...carversions(ID ,
	Name ,
	CarModelId ,
	SegmentId ,
	BodyStyleId ,
	IsDeleted  ,
	Used  ,
	New  ,
	Indian  ,
	Imported  ,
	Futuristic  ,
	Classic  ,
	Modified,
	SubSegmentId ,
	CarFuelType  ,
	CarTransmission  ,
	VCreatedOn  ,
	Discontinuation_date  ,
	MaskingName  ,
	vcreatedby  ,
	LaunchDate ) 
	VALUES (
	@ID ,
	@Name ,
	@CarModelId ,
	@SegmentId ,
	@BodyStyleId ,
	@IsDeleted  ,
	@Used  ,
	@New  ,
	@Indian  ,
	@Imported  ,
	@Futuristic  ,
	@Classic  ,
	@Modified  ,
	@SubSegmentId ,
	@CarFuelType  ,
	@CarTransmission  ,
	@VCreatedOn  ,
	@Discontinuation_date,
	@MaskingName  ,
	@vcreatedby  ,
	@LaunchDate);
    				end try
	BEGIN CATCH
		INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
		VALUES('MysqlSync','SyncCarVersionsWithMysql',ERROR_MESSAGE(),'CarVersions',@Id,GETDATE(),null)
	END CATCH
END

