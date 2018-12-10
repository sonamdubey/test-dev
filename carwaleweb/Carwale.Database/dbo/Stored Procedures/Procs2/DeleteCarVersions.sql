IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DeleteCarVersions]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DeleteCarVersions]
GO

	-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[DeleteCarVersions] 
	-- Add the parameters for the stored procedure here
	@Id numeric,
	@VUpdatedBy numeric
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
   UPDATE CarVersions SET IsDeleted=1,VUpdatedOn=GETDATE(),VUpdatedBy=@VUpdatedBy WHERE Id=@Id
   -- mysql sync start	
declare   @Name varchar(50)  = null, @SegmentId numeric(18, 0)  = null, @BodyStyleId numeric(18, 0)  = null, @Used bit  = null, @New bit  = null, @IsDeleted bit  = null, @Indian bit  = null, @Imported bit  = null, @Futuristic bit  = null, @Classic bit  = null, @Modified bit =null,		 @Discontinuation_date datetime =null,	 @MaskingName varchar(50)  = null, @SubSegmentId numeric = null, @LaunchDate datetime = null, @DiscontinuitionId numeric = null, @ReplacedByVersionId smallint = null, @Comment varchar(5000) = null,  @VUpdatedOn datetime = null, @CarFuelType tinyint = null, @CarTransmission tinyint = null, @HostUrl varchar(100) = null, @UpdateType int = null, @Environment varchar(150) =null , @OriginalImgPath varchar(150) = null, @LargePic varchar(150) = null, @SmallPic varchar(150) = null 
set @UpdateType=11
	begin try
		exec [dbo].[SyncCarVersionsWithMysqlUpdate] 
		@ID , @Name , @SegmentId , @BodyStyleId , @Used , @New , @IsDeleted , @Indian , @Imported , @Futuristic , @Classic , @Modified ,		 @Discontinuation_date ,	 @MaskingName , @SubSegmentId , @LaunchDate , @DiscontinuitionId , @ReplacedByVersionId , @Comment , @VUpdatedBy , @VUpdatedOn , @CarFuelType , @CarTransmission , @HostUrl , @UpdateType , @Environment , @OriginalImgPath , @SmallPic , @LargePic 	
 	end try
	BEGIN CATCH
		INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
		VALUES('MysqlSync','DeleteCarVersions',ERROR_MESSAGE(),'SyncCarVersionsWithMysqlUpdate',@Id,GETDATE(),@UpdateType)
	END CATCH			
	
--mysql sync end
END

