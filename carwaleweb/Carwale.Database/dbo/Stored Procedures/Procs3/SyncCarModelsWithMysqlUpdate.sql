IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[SyncCarModelsWithMysqlUpdate]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[SyncCarModelsWithMysqlUpdate]
GO

	-- =============================================
-- Author:		Pratik Vasa
-- Create date: 23/9/16
-- Description:	added to sync Carwale sqlserver with Mysql
-- =============================================
create PROCEDURE  [dbo].[SyncCarModelsWithMysqlUpdate]
	-- Add the parameters for the stored procedure here
	@Name varchar(30) ,
	@CarMakeId numeric(18, 0) ,
	@IsDeleted bit ,
	@MoCreatedOn datetime ,
	@MoUpdatedOn datetime,
	@MoUpdatedBy numeric(18, 0),
	@Used bit,
	@New bit,
	@Indian bit,
	@Imported bit,
	@Classic bit,
	@Futuristic bit,
	@Modified bit,
	@Id int,
	@SmallPic varchar(200),
	@LargePic varchar(200),
	@HostUrl varchar(100),
	@UpdateType INT,
	@DiscontinuitionId int =null,
	@ReplacedByModelId smallint =null,
	@comment Varchar(max) = null,
	@Discontinuition_date datetime = null,
	@Maskingname varchar(50) = null,
	@RootId smallint =null,
	@Platform varchar(50)=null, 
	@Generation tinyint=null, 
	@Upgrade tinyint =null, 
	@ModelLaunchDate datetime =null,
	@SubSegmentId int = null,
	@CarVersionID_Top int =null,
	@IsSolidColor bit =null,
	@IsMetallicColor bit =null,
	@MinPrice int =null, 
	@MaxPrice int =null,
	@ReviewRate decimal = null,   
	@Looks decimal = null,   
	@Comfort decimal = null,   
	@Performance decimal = null,   
	@ValueForMoney decimal = null,   
	@FuelEconomy decimal = null,   
	@ReviewCount decimal = null,
	@Summary text = null,
	@OriginalImgPath varchar(150)= null,
	@XLargePic varchar(150)=null,
	@CV_ID int = null,
	@SelId varchar(8000)=null,
	@CarModelId numeric = null,
	@PriceCityId INT = 10,  
	@PriceCityName VARCHAR(25) =null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	BEGIN TRY
	declare 
			@CMXLargePic varchar(150)
			,@CMSmallPic varchar(200)
			,@CMLargePic varchar(200)
			,@CMHostURL varchar(100)
			,@CMID numeric(18,0)
			,@CMOriginalImgPath varchar(150)
	if @UpdateType = 1
	begin
    UPDATE mysql_test...carmodels SET Name=@Name, Used=@Used, New=@New, Indian=@Indian,
                Imported=@Imported,
                Classic=@Classic,
                Modified=@Modified,
                Futuristic=@Futuristic,
                MoUpdatedOn=@MoUpdatedOn,
                MoUpdatedBy=@MoUpdatedBy
                WHERE Id=@Id
				end
	else if @UpdateType = 2
	begin
		UPDATE mysql_test...carmodels SET
		 IsDeleted=1,MoUpdatedOn=GETDATE(),MoUpdatedBy=@MoUpdatedBy WHERE Id=@Id;
	end
	else if @UpdateType = 3
	begin
		UPDATE mysql_test...carmodels SET New=1, Comment=NULL, Discontinuation_date=NULL, ReplacedByModelId=NULL, DiscontinuationId=NULL WHERE Id=@Id
	end
	else if @UpdateType = 4
	begin
		UPDATE mysql_test...carmodels SET IsReplicated = 0, SmallPic=@SmallPic, 
                                 LargePic=@LargePic,
                                 HostURL = @HostUrl
                                 WHERE ID=@Id
	end
	else if @UpdateType = 5
		update mysql_test...carmodels 
		set New=0,
			DiscontinuationId=@DiscontinuitionId,
			ReplacedByModelId=@ReplacedByModelId,
			comment=@comment,
			Discontinuation_date=@Discontinuition_date 
		where Id=@Id;  
	else if @UpdateType = 6  
		update mysql_test...carmodels SET New=1,Futuristic=0 WHERE Id In (SELECT CarModelId FROM  ExpectedCarLaunches WHERE Id IN(select * from dbo.SplitTextRS(@SelId,',')) )  
	--else if @UpdateType = 7   
		-- following will happen via code
		--UPDATE MO SET MO.CarVersionID_Top = VC.VersionId , MO.SubSegmentID = CV.SubSegmentID , MO.ModelBodyStyle=CV.BodyStyleId
		--	FROM mysql_test...carmodels AS MO
		--	INNER JOIN TopVersionCar VC WITH(NOLOCK) ON VC.Modelid = MO.ID
		--	INNER JOIN CarVersions CV WITH(NOLOCK) ON CV.Id = VC.VersionId
	else if @UpdateType = 8
		update mysql_test...carmodels 
			SET    name = @Name,						   
					maskingname = @Maskingname, 
					used = @Used, 
					new = @New, 
					indian = @Indian, 
					imported = @Imported, 
					classic = @Classic, 
					modified = @Modified, 
					futuristic = @Futuristic, 
					moupdatedon = Getdate(), 
					moupdatedby = @MoUpdatedBy, 
					rootid = @RootId, 
					platform = @Platform, 
					generation = @Generation, 
					upgrade = @Upgrade, 
					ModelLaunchDate = @ModelLaunchDate, 
					comment = @comment ,
					ReplacedByModelId = @ReplacedByModelId
						   
			WHERE  id = @Id
	else if @UpdateType = 9					
		update mysql_test...carmodels 
			SET    new = @new, 
					discontinuationid = @DiscontinuitionId, 
					discontinuation_date = @Discontinuition_date 
			WHERE  id = @Id; 
					
	else if @UpdateType = 10				
		update mysql_test...carmodels
				SET carversionid_top = (
						CASE @ID
							WHEN - 1
								THEN @CV_ID
							ELSE @ID
							END
						)
					,subsegmentid = @SubSegmentId
				WHERE id = @CarModelId				
	else if @UpdateType = 11
		update mysql_test...carmodels SET Futuristic=@Futuristic WHERE Id=(SELECT CarModelId FROM  ExpectedCarLaunches WHERE Id=@Id)     
	else if @UpdateType = 12		   
		update mysql_test...carmodels 
		SET SmallPic = @SmallPic , 
			LargePic = @LargePic , 
			HostURL = @HostURL ,
			New= @New,
			Futuristic=@Futuristic, 
			IsReplicated = 0 
		WHERE ID = @Id          
		 
	else if @UpdateType = 13					
		update mysql_test...carmodels
			SET CarVersionID_Top = @CarVersionID_Top, SubSegmentID = @SubSegmentId WHERE ID = @Id
	else if @UpdateType = 14
		update mysql_test...carmodels SET isSolidColor=@IsSolidColor,isMetallicColor=@IsMetallicColor WHERE ID=@Id
	else if @UpdateType = 15
		update mysql_test...carmodels
			SET MinAvgPrice = tbl.AvgPrice, MinPrice = @MinPrice , MaxPrice=@MaxPrice 
			FROM (	SELECT MIN(a.AvgPrice)as AvgPrice from Con_NewCarNationalPrices a 
					join CarVersions b on a.VersionId = b.id and b.CarModelId = @Id
					where b.New = 1 and b.IsDeleted = 0 and a.IsActive =1 ) AS tbl	
		WHERE ID = @Id
	else if @UpdateType = 16	
		update mysql_test...carmodels   
			SET   
			ReviewRate = @ReviewRate,   
			Looks = @Looks,   
			Comfort = @Comfort,   
			Performance = @Performance,   
			ValueForMoney = @ValueForMoney,   
			FuelEconomy = @FuelEconomy,   
			ReviewCount = @ReviewCount  
			WHERE ID = @Id 
	else if @UpdateType = 17
		update mysql_test...carmodels SET Summary = @Summary WHERE ID = @ID
	else if @UpdateType = 18
		update mysql_test...carmodels
				SET XLargePic = @XLargePic
				WHERE carmodels.ID = @Id
	else if @UpdateType = 19			
	begin	
		select 
			@CMOriginalImgPath= EI.OriginalImgPath --Added By Ashwini Todkar on 9 July 2015, updated image path to carmodels
			,@CMHostURL = EI.HostURL
			,@CMID=CM.ID
			FROM carmodels CM   WITH(NOLOCK)
			INNER JOIN Con_EditCms_Images EI WITH(NOLOCK) ON CM.ID = EI.ModelId
			INNER JOIN Con_EditCms_Basic EB WITH(NOLOCK) ON EB.Id = EI.BasicId
			WHERE EI.IsMainImage = 1  AND EB.CategoryId = 10 AND EB.ApplicationID = 1 
				AND EI.Id = @Id 
		UPDATE CM 
			SET CM.OriginalImgPath = @CMOriginalImgPath --Added By Ashwini Todkar on 9 July 2015, updated image path to carmodels
			,CM.HostURL = @CMHostURL
			,CM.IsReplicated = 1
			FROM mysql_test...carmodels CM   WITH(NOLOCK)
			where cm.ID=@CMID
	end
	else if @UpdateType = 20				
		update mysql_test...carmodels SET New=@New,Futuristic=@Futuristic WHERE Id=(SELECT CarModelId FROM  ExpectedCarLaunches WITH(NOLOCK) WHERE Id=@Id) 
	else if @UpdateType = 21
		update mysql_test...carmodels SET OriginalImgPath = @OriginalImgPath, HostURL = @HostUrl ,
		New= @New,Futuristic=@Futuristic, IsReplicated = 0 WHERE ID = @Id   								
			
	else if @UpdateType = 22
		update mysql_test...carmodels SET HostURL = @HostUrl , 
                     IsReplicated =1
			WHERE ID = @Id
	else if @UpdateType = 23
		begin
			
			select @CMXLargePic = @HostUrl + EI.ImagePathLarge
			,@CMSmallPic = EI.ImagePathSmall 
			,@CMLargePic = EI.ImagePathCustom
			,@CMHostURL = EI.HostURL
			,@CMID=CM.ID
			FROM carmodels CM   WITH(NOLOCK)
			INNER JOIN Con_EditCms_Images EI WITH(NOLOCK) ON CM.ID = EI.ModelId
			INNER JOIN Con_EditCms_Basic EB WITH(NOLOCK) ON EB.Id = EI.BasicId
			WHERE EI.IsMainImage = 1  AND EB.CategoryId = 10 AND EB.ApplicationID = 1 
				AND EI.Id = @Id 
			UPDATE CM 
			SET CM.XLargePic = @CMXLargePic
			,CM.SmallPic = @CMSmallPic
			,CM.LargePic = @CMLargePic
			,CM.HostURL = @CMHostURL
			,CM.IsReplicated = 1
			FROM mysql_test...carmodels CM 			
			WHERE CM.ID = @CMID
		end
	else if @UpdateType = 24
	begin
		select 
			@CMOriginalImgPath= EI.OriginalImgPath --Added By Ashwini Todkar on 9 July 2015, updated image path to carmodels
			,@CMHostURL = EI.HostURL
			,@CMID=CM.ID
			FROM carmodels CM   WITH(NOLOCK)
			INNER JOIN Con_EditCms_Images EI WITH(NOLOCK) ON CM.ID = EI.ModelId
			INNER JOIN Con_EditCms_Basic EB WITH(NOLOCK) ON EB.Id = EI.BasicId
			WHERE EI.IsMainImage = 1  AND EB.CategoryId = 10 AND EB.ApplicationID = 1 
				AND EI.Id = @Id 
		UPDATE CM 
			SET CM.OriginalImgPath = @CMOriginalImgPath --Added By Ashwini Todkar on 9 July 2015, updated image path to carmodels
			,CM.HostURL = @CMHostURL
			,CM.IsReplicated = 1
			FROM mysql_test...carmodels CM   WITH(NOLOCK)
			where cm.ID=@CMID
	end
	else if @UpdateType = 25
		update mysql_test...carmodels SET HostURL = @HostUrl , 
								 IsReplicated =1,
								 OriginalImgPath = @OriginalImgPath + OriginalImgPath
			WHERE ID = @Id			
	
	else if @UpdateType = 26
	
		WITH CTE AS(
		SELECT * FROM (
		SELECT CarModelId,ID,SubSegmentId,CNT,Price,ROW_NUMBER()OVER(PARTITION BY CarModelId ORDER BY CNT DESC,Price DESC) MCNT FROM(
		SELECT CM.ID CarModelId,CV.ID,CV.SubSegmentId,Price,COUNT(NCP.Id)CNT
		FROM CarModels CM WITH (NOLOCK)
		LEFT JOIN CarVersions CV WITH (NOLOCK) ON CM.ID = CV.CarModelId AND CV.IsDeleted = 0 AND CV.New = 1 
		LEFT JOIN NewCarShowroomPrices NCS WITH (NOLOCK) ON NCS.CarVersionId=CV.ID AND CityId=10
		LEFT JOIN NewCarPurchaseInquiries NCP WITH (NOLOCK) ON CV.ID = NCP.CarVersionId
		WHERE CM.New = 1 AND CM.IsDeleted = 0 AND (CM.ID = @CarModelId OR @CarModelId IS NULL)
		GROUP BY CM.ID,CV.ID,Price,CV.SubSegmentId)AS Tab)AS Tab2 WHERE MCNT=1)
		UPDATE mysql_test...carmodels
		SET CarVersionID_Top = CTE.ID, SubsegmentId = CTE.SubSegmentId
		FROM CTE
		WHERE CarModelId=CarModels.ID			
	else if @UpdateType = 27
		UPDATE mysql_test...carmodels
		SET MinPrice = @MinPrice ,MaxPrice=@MaxPrice , PriceCityId = @PriceCityId , PriceCityName = @PriceCityName
	WHERE  Id=@Id			
	else if @UpdateType = 28		
		SELECT @MinPrice=MIN(Price) ,
			@MaxPrice=MAX(Price) 
		FROM NewCarShowroomPrices NCP WITH(NOLOCK)
			INNER JOIN CarVersions CV WITH(NOLOCK) ON CV.ID = NCP.CarVersionId
		WHERE NCP.CityId = 10   --- Need to consider delhi price only
			AND CV.New=1 
			AND CV.IsDeleted=0
			AND CV.CarModelId=@CarModelId  ---Condition added by Manish on 24-04-2014 
		GROUP BY cv.CarModelId
		UPDATE mysql_test...carmodels
			SET MinPrice = @MinPrice ,MaxPrice=@MaxPrice WHERE  Id=@Id;
	end try
	BEGIN CATCH
		INSERT INTO CarWaleMyslSyncExceptions(ModuleName,SPName,ErrorMsg,TableName,FailedId,CreatedOn,InputParameter)
		VALUES('MysqlSync','SyncCarModelsWithMysqlUpdate',ERROR_MESSAGE(),'CarModels',@Id,GETDATE(),@UpdateType)
	END CATCH
END

