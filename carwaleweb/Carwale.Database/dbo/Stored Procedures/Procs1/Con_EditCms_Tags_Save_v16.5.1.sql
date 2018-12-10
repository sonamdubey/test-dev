IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Con_EditCms_Tags_Save_v16]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Con_EditCms_Tags_Save_v16]
GO

	-- =============================================
-- Description:	To Update the tags of CMS category
-- Modified By : Jitendra on 13 May 2016, Save url in basic table for expert review and videos
-- =============================================
CREATE PROCEDURE [dbo].[Con_EditCms_Tags_Save_v16.5.1]      
@BasicId Numeric(18,0),      
@String VarChar(8000),      
@Delimiter Char(1),
@ApplicationId tinyint,      
@LastUpdatedBy Numeric(18,0)          
AS       
      
Begin      
      
 Declare @idx int             
 Declare @slice varchar(8000)       
 Declare @slug varchar(8000)      
 Declare @validChars varchar(100)            
 Declare @rowCount Int = 0
       
 Set @validChars = '0-9a-zA-Z '         
       
 Select @idx = 1             
  if Len( @String )<1 or @String is Null        
   Return 
 
 -- Jitendra on 13 May 2016, Save url in basic table for expert review and videos
 IF @ApplicationId = 1
 BEGIN
	 DECLARE @CategoryId INT,
			@CategoryMaskingName VARCHAR(150),
			@ExistingUrl VARCHAR(200)

	 SELECT @CategoryId = ceb.CategoryId,
			@CategoryMaskingName = cec.CategoryMaskingName,
			@ExistingUrl = ceb.Url
	 FROM Con_EditCms_Basic ceb WITH(NOLOCK) 
	 INNER JOIN 
	 Con_EditCms_Category cec WITH(NOLOCK)  ON ceb.CategoryId = cec.Id 
	 WHERE ceb.Id = @BasicId

	 IF @CategoryId IN (8,13)
	 BEGIN
		 DECLARE @TagCount INT = 0         
		 SELECT  @TagCount = COUNT(TagId) FROM Con_EditCms_BasicTags WITH(NOLOCK) Where BasicId = @BasicId

		 IF @TagCount = 0
		 BEGIN

			DECLARE @ModelId INT,
					@ModelMaskingName VARCHAR(50),
					@MakeMakingName VARCHAR(50),
					@_Url VARCHAR(200)		

			SELECT TOP 1 @ModelId = ModelId FROM Con_EditCms_Cars WITH(NOLOCK) WHERE BasicId = @BasicId ORDER BY LastUpdatedTime

			SELECT @ModelMaskingName = cmd.MaskingName,@MakeMakingName = cmk.Name  
			FROM 
			CarModels cmd WITH(NOLOCK) 
			INNER JOIN 
			CarMakes cmk WITH(NOLOCK) 
			ON cmk.ID = cmd.CarMakeId and cmd.ID = @ModelId

			SET @MakeMakingName = LOWER(REPLACE(REPLACE(REPLACE(REPLACE(@MakeMakingName,'[^/\-0-9a-zA-Z\s]*',''),' ', ''),'-',''),'/',''))
			SET @_Url = '/' +@MakeMakingName+'-cars/'+@ModelMaskingName+'/'+@CategoryMaskingName
		
			SET @_Url = CASE 
							WHEN @CategoryId = 8 THEN @_Url+'-'+CAST(@BasicId AS VARCHAR(18))+'/'
							ELSE @_Url +'/'+@ExistingUrl+'-'+CAST(@BasicId AS VARCHAR(18))+'/'
						END

			UPDATE  Con_EditCms_Basic
			SET Url = @_Url					
			WHERE
			Id = @BasicId
		 END 
	 END
 END  

 Delete From Con_EditCms_BasicTags Where BasicId = @BasicId      
       
 While @idx != 0             
  Begin             
   Set @idx = CharIndex( @Delimiter, @String )             
   if @idx != 0             
    set @slice = Left( @String, @idx - 1)             
   else             
    set @slice = @String       
           
   if( Len( @slice ) > 0 )        
    Begin        
     -- The Below function remove all spl chars and replaces white space with hyphen      
     Set @slug = dbo.GetCharacters(@slice, @validChars)           
     Set @slug = Replace(LTRIM(RTRIM(LOWER(@slug))), ' ', '-')      
           
     Declare @tagId Int      
          
     Set @tagId = IsNull( ( Select Id From Con_EditCms_Tags WITH(NOLOCK) Where Slug =  @slug ), 0 )      
           
     --Select @slug, @slice      
                
     if @tagId > 0          
      Begin      
       --If IsNull( ( Select Id From Con_EditCms_BasicTags Where BasicId = @BasicId And TagId = @tagId ), 0 ) = 0           
	   Select @rowCount = COUNT(BasicId) From Con_EditCms_BasicTags WITH(NOLOCK) Where BasicId = @BasicId And TagId = @tagId 
	   If( @rowCount = 0 )
	   Begin
		Insert Into Con_EditCms_BasicTags ( BasicId, TagId ) Values ( @BasicId, @tagId )
	   End	
      End      
     else      
      Begin      
       Insert Into Con_EditCms_Tags ( Tag, Slug, LastUpdatedBy ) Values ( LTRIM(RTRIM(@slice)), @slug, @LastUpdatedBy )      
       Set @tagId = SCOPE_IDENTITY()      
       if @tagId Is Not Null --And IsNull( ( Select Id From Con_EditCms_BasicTags Where BasicId = @BasicId And TagId = @tagId ), 0 ) = 0      
       Begin      
        Insert Into Con_EditCms_BasicTags ( BasicId, TagId ) Values ( @BasicId, @tagId )      
       End        
      End       
    End      
         
   Set @String = Right( @String, Len( @String ) - @idx )             
         
   if Len( @String ) = 0       
    Break        
  End      
End   
