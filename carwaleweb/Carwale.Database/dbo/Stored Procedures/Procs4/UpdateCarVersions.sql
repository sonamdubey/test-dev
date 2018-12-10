IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UpdateCarVersions]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UpdateCarVersions]
GO

	-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[UpdateCarVersions]
	-- Add the parameters for the stored procedure here
	@ID numeric(18, 0),
	@Name varchar(50) ,
	@SegmentId numeric(18, 0) ,
	@BodyStyleId numeric(18, 0) ,
	@Used bit ,
	@New bit ,
	@IsDeleted bit ,
	@Indian bit ,
	@Imported bit ,
	@Futuristic bit ,
	@Classic bit ,
	@Modified bit ,		
	@SubSegmentId numeric,
	@VUpdatedOn datetime,
	@CarFuelType tinyint,
	@CarTransmission tinyint,
	@VUpdatedBy numeric
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    UPDATE CarVersions SET 
				Name=@Name,
				SegmentId=@SegmentId,
				SubSegmentId=@SubSegmentId,
				BodyStyleId=@BodyStyleId,
                CarFuelType=@CarFuelType,
                CarTransmission=@CarTransmission,
				Used=@Used,
				New=@New,
				Indian=@Indian,
				Imported=@Imported,
				Classic=@Classic,
				Modified=@Modified,
                Futuristic=@Futuristic,
                VUpdatedOn=GETDATE(),
                VUpdatedBy=@VUpdatedBy
				WHERE Id=@Id
-- mysql sync start	
declare  @Discontinuation_date datetime =null,	 @MaskingName varchar(50)  = null, @LaunchDate datetime = null, @DiscontinuitionId numeric = null, @ReplacedByVersionId smallint = null, @Comment varchar(5000) = null,  @HostUrl varchar(100) = null, @UpdateType int = null, @Environment varchar(150) =null , @OriginalImgPath varchar(150) = null, @LargePic varchar(150) = null, @SmallPic varchar(150) = null 
set @UpdateType =10
set @VUpdatedOn=GETDATE()
begin try
exec [dbo].[SyncCarVersionsWithMysqlUpdate] 
	@ID , @Name , @SegmentId , @BodyStyleId , @Used , @New , @IsDeleted , @Indian , @Imported , @Futuristic , @Classic , @Modified ,		 @Discontinuation_date ,	 @MaskingName , @SubSegmentId , @LaunchDate , @DiscontinuitionId , @ReplacedByVersionId , @Comment , @VUpdatedBy , @VUpdatedOn , @CarFuelType , @CarTransmission , @HostUrl , @UpdateType , @Environment , @OriginalImgPath , @SmallPic , @LargePic 	
	
end try
BEGIN CATCH
	INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
	VALUES('MysqlSync','UpdateCarVersions',ERROR_MESSAGE(),'SyncCarVersionsWithMysqlUpdate',@Id,GETDATE(),@UpdateType)
END CATCH	
--mysql sync end
END

