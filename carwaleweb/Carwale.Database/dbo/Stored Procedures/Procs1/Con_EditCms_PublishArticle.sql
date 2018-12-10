IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Con_EditCms_PublishArticle]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Con_EditCms_PublishArticle]
GO

	-- Modified by:Rakesh yadav On 7 jan 2016, added output parameter @ModelId and @MakeId
CREATE PROCEDURE [dbo].[Con_EditCms_PublishArticle]  
@BasicId INT,  
@Path VARCHAR(50),  
@AddToForum BIT,  
@CustomerId INT,  
@AlertType INT = 2,  
@Message VARCHAR(500),  
@ArticleType INT,  
@TitleText VarChar(100) ,
@IsDealerFriendly Bit,
@ModelId INT= 0 OUTPUT,
@MakeId INT= 0 OUTPUT
--,@URL VARCHAR(200) OUTPUT
AS  
BEGIN  
  
  --DECLARE @ModelId NUMERIC(18,0)  
  DECLARE @MakeName VARCHAR(50)  
  DECLARE @ModelName VARCHAR(50)  
  DECLARE @VersionName VARCHAR(50)  
  DECLARE @CarName VARCHAR(150)  
  DECLARE @Title VARCHAR(170)  
  DECLARE @ForumCategoryId INT 
  DECLARE @ThreadId INT
  DECLARE @PostId AS INT
  DECLARE @StartDateTime DATETIME  
  
  --Update road test to publish in basic table  
  UPDATE Con_EditCms_Basic  
  SET  
  IsPublished = 1, PublishedDate = GETDATE(), IsDealerFriendly =  @IsDealerFriendly
  WHERE  
  ID = @BasicId  
  
  --If successfully published  
  IF @@ROWCOUNT > 0  
  BEGIN  
  
  --Select ModelId ,car name, forum category id for road test  
  SELECT  
     @ModelId = CEC.ModelId,  
     @MakeName = CMA.Name,  
     @ModelName = CMO.Name,  
     @VersionName = CV.Name,  
     @ForumCategoryId = CECAT.ForumCategoryId,
	 @MakeId=CMA.ID
     --,@URL = 'http://www.carwale.com/' + CECAT.CategoryMaskingName + '/' + CEB.Url + '-' + CAST(CEB.Id AS VARCHAR) + '/'
  FROM  
  Con_EditCms_Basic  CEB   WITH(NOLOCK)
     LEFT JOIN Con_EditCms_Category CECAT WITH(NOLOCK) ON CEB.CategoryId = CECAT.Id  
     LEFT JOIN Con_EditCms_Cars CEC WITH(NOLOCK) On CEB.Id = CEC.BasicId  
     LEFT JOIN CarMakes CMA WITH(NOLOCK) ON CEC.MakeId = CMA.ID  
     LEFT JOIN CarModels CMO WITH(NOLOCK) ON CEC.ModelId = CMO.ID  
     LEFT JOIN CarVersions CV WITH(NOLOCK) ON CEC.VersionId = CV.ID  
  WHERE  
  CEB.Id = @BasicId  
  
  SET @Path = @Path + CONVERT(VARCHAR,@BasicId)  
  
     --insert modelId and path to RoadTests table  
    /* INSERT INTO RoadTests (ModelId,Path)  
     VALUES (@ModelId, @Path)         */  
  
     --If it is to be added to forums  
     IF @AddToForum = 1  
     BEGIN  
  
     IF @VersionName IS NOT NULL  
     BEGIN  SET @CarName = @MakeName + ' ' + @ModelName + ' ' + @VersionName  END  
     ELSE  
     BEGIN  SET @CarName = @MakeName + ' ' + @ModelName  END  
  
     SET @Title = @CarName + @TitleText  
     SET @StartDateTime = GETDATE()  
  
     --Add it to forums table to create the thread  
     INSERT INTO Forums  
     (ForumSubCategoryId,  CustomerId,  Topic,   StartDateTime,   IsActive, IsApproved)  
     VALUES  
     (@ForumCategoryId,  @CustomerId,  @Title,  @StartDateTime,  1,  0)  
  
     SET @ThreadId = SCOPE_IDENTITY()  
           
     INSERT INTO Forum_ArticleAssociation (ArticleType,ThreadId,ArticleId,CreateDate)  
     VALUES ( @ArticleType, @ThreadId, @BasicId , @StartDateTime)  
  
     EXEC EnterForumMessage @ThreadId, @CustomerId, @Message, @StartDateTime, @AlertType, @PostId  
  
  END  
  
 END   
	--SELECT  'http://www.carwale.com/' + ISNULL(CECAT.CategoryMaskingName,'') + '/' + ISNULL(CEB.Url,'') + '-' + CAST(CEB.Id AS VARCHAR) + '/'
	--FROM  Con_EditCms_Basic CEB INNER JOIN Con_EditCms_Category CECAT ON CEB.CategoryId = CECAT.Id  
	--WHERE  CEB.Id = 21036   
          
END  
  