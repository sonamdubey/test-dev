IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[SyncCarVersionsWithMysqlUpdate]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[SyncCarVersionsWithMysqlUpdate]
GO

	-- =============================================
-- Author:		Prasad Gawde
-- Create date: 19/10/16
-- Description:	added to sync Carwale sqlserver with Mysql
-- =============================================
CREATE PROCEDURE  [dbo].[SyncCarVersionsWithMysqlUpdate] --12345,'unknown',1,1,1,1,1,1,1,1,1,1,'10-10-2016','mname',1,'10-10-2016',1,1,'comment',12,'12-12-2016',1,1,'hosturl',1,'env','orgpath','smallpic','largepic'
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
	@Discontinuation_date datetime ,	
	@MaskingName varchar(50) ,
	@SubSegmentId numeric,
	@LaunchDate datetime,
	@DiscontinuitionId numeric,
	@ReplacedByVersionId smallint,
	@Comment varchar(5000),
	@VUpdatedBy numeric,
	@VUpdatedOn datetime,
	@CarFuelType tinyint,
	@CarTransmission tinyint,
	@HostUrl varchar(100),
	@UpdateType int,
	@Environment varchar(150) =null,
	@OriginalImgPath varchar(150) = null,
	@SmallPic varchar(150) = null,
	@LargePic varchar(150) = null
AS
BEGIN
	DECLARE @CarVersionId int, @OriginalImagePath VARCHAR(150), @UpdateHostURL VARCHAR(50), @SmallPhoto VARCHAR(150), @LargePhoto VARCHAR(150)
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	  BEGIN TRY
  if @UpdateType =1
	  update mysql_test...carversions 
	  set 
		New=@New,
		DiscontinuationId=@DiscontinuitionId,
		ReplacedByVersionId=@ReplacedByVersionId,
		comment=@Comment,
		Discontinuation_date=@Discontinuation_date 
	  where Id=@Id;    
  else if @UpdateType =2
	  UPDATE mysql_test...carversions 
	  SET 
		IsDeleted = @IsDeleted,
		VUpdatedOn = @VUpdatedOn,
		VUpdatedBy = @VUpdatedBy 
	  WHERE ID = @ID
  else if @UpdateType =3  
	  UPDATE mysql_test...carversions
		SET NAME = @Name
			,segmentid = @SegmentId
			,subsegmentid = @SubSegmentId
			,bodystyleid = @BodyStyleId
			,carfueltype = @CarFuelType
			,cartransmission = 	@CarTransmission
			,used = @Used
			,new = @New
			,indian = @Indian
			,imported = @Imported
			,classic = @Classic
			,modified = @Modified
			,futuristic = @Futuristic
			,vupdatedon = @VUpdatedOn
			,vupdatedby = @VUpdatedBy
			,maskingname = @MaskingName
			,Discontinuation_date = @Discontinuation_date
			,LaunchDate = @LaunchDate
		WHERE id = @Id
	else if @UpdateType =4
		UPDATE mysql_test...carversions 
		SET 
			New=@New,
			Futuristic=@Futuristic 
		WHERE Id=(SELECT CarVersionId FROM  expectedcarlaunches WITH(NOLOCK) WHERE Id=@Id)    
		
	else if @UpdateType =5  	
	begin
		CREATE table #TempCarVersions (CarVersionId int,HostURL varchar(100),OriginalImgPath varchar(150));
		insert into #TempCarVersions(CarVersionId ,HostURL ,OriginalImgPath) 
		select
			CV.ID,
			EI.HostURL,
			EI.OriginalImgPath  --Added By Ashwini Todkar on 9 July 2015, saved small image path to mysql_test...carversions			
			FROM CarVersions CV WITH (NOLOCK)
			JOIN Con_EditCms_Images EI WITH (NOLOCK) ON CV.CarModelId = EI.ModelId
			INNER JOIN Con_EditCms_Basic EB WITH(NOLOCK) ON EB.Id = EI.BasicId
			WHERE EI.Id = @ID AND EI.IsMainImage = 1  AND EB.CategoryId = 10 AND EB.ApplicationID = 1 
			AND CV.SpecialVersion = 0 --Special version having different image than model image				
	
		SELECT  
			TOP 1 @OriginalImagePath  = EI.OriginalImgPath  
			,@HostURL = EI.HostURL
			FROM #TempCarVersions EI
		UPDATE mysql_test...carversions SET IsReplicated = 1 , 
					   HostUrl = @UpdateHostURL,
					   OriginalImgPath = @OriginalImagePath 
				WHERE Id in (select CarVersionId from #TempCarVersions)	
	end
	else if @UpdateType =6  						
		UPDATE mysql_test...carversions SET IsReplicated = 1 , 
				   HostUrl = @HostUrl,
				   OriginalImgPath = @Environment + OriginalImgPath
			WHERE Id=@ID 
											
	else if @UpdateType =7		
	begin
	
	CREATE table #TempCarVersionsSmallLargePic (CarVersionId int,HostURL varchar(100),SmallPic varchar(150),LargePic varchar(150));
	insert into #TempCarVersionsSmallLargePic(CarVersionId ,HostURL ,SmallPic,LargePic) 
	select
		CV.ID,
		EI.HostURL,
		EI.ImagePathSmall,
		EI.ImagePathCustom
		FROM CarVersions CV WITH (NOLOCK)
		JOIN Con_EditCms_Images EI WITH (NOLOCK) ON CV.CarModelId = EI.ModelId
		INNER JOIN Con_EditCms_Basic EB WITH(NOLOCK) ON EB.Id = EI.BasicId
		WHERE EI.Id = @ID AND EI.IsMainImage = 1  AND EB.CategoryId = 10 AND EB.ApplicationID = 1 
		AND CV.SpecialVersion = 0 --Special version having different image than model image				
	
	SELECT  
		TOP 1 @SmallPhoto = EI.SmallPic  ,
		@LargePhoto = LargePic
		,@HostURL = EI.HostURL
		FROM #TempCarVersionsSmallLargePic EI
	UPDATE mysql_test...carversions SET IsReplicated = 1 , 
				   HostUrl = @UpdateHostURL,
				   largePic = @LargePhoto,
				   smallPic=@SmallPhoto
			WHERE Id in (select CarVersionId from #TempCarVersionsSmallLargePic)
	end
	else if @UpdateType =8
		
		UPDATE mysql_test...carversions 
			SET IsReplicated = 1 , 
				HostUrl = @HostUrl		
			WHERE Id=@ID    
			
	else if @UpdateType = 9
	begin
		UPDATE mysql_test...carversions SET New=1, Comment=NULL, Discontinuation_date=NULL, ReplacedByVersionId=NULL, DiscontinuationId=NULL WHERE Id=@Id
	end
	else if @UpdateType = 10
	begin 
		UPDATE mysql_test...carversions SET 
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
	end
	else if @UpdateType = 11
	begin
		UPDATE mysql_test...carversions SET IsDeleted=1,VUpdatedOn=GETDATE(),VUpdatedBy=@VUpdatedBy WHERE Id=@Id
	end
	else if @UpdateType = 12
	begin
		UPDATE mysql_test...carversions SET IsReplicated = 0,
                 OriginalImgPath = @OriginalImgPath,
                 HostURL = @HostURL,
                 SpecialVersion = 1
                 WHERE ID=@Id
	end
	else if @UpdateType = 13
	begin
	UPDATE mysql_test...carversions SET IsReplicated = 0,
				SmallPic= @SmallPic,
				LargePic=@LargePic,
				HostURL = @HostURL
				WHERE ID=@Id
	end 
	 				end try
	BEGIN CATCH
		INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
		VALUES('MysqlSync','SyncCarVersionsWithMysqlUpdate',ERROR_MESSAGE(),'CarVersions',@Id,GETDATE(),@UpdateType)
	END CATCH
END

