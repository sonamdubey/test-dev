IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Con_EditCms_Tags_Save]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Con_EditCms_Tags_Save]
GO

	CREATE PROCEDURE [dbo].[Con_EditCms_Tags_Save]      
@BasicId Numeric(18,0),      
@String VarChar(8000),      
@Delimiter Char(1),      
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
          
     Set @tagId = IsNull( ( Select Id From Con_EditCms_Tags Where Slug =  @slug ), 0 )      
           
     --Select @slug, @slice      
                
     if @tagId > 0          
      Begin      
       --If IsNull( ( Select Id From Con_EditCms_BasicTags Where BasicId = @BasicId And TagId = @tagId ), 0 ) = 0           
	   Select @rowCount = COUNT(BasicId) From Con_EditCms_BasicTags Where BasicId = @BasicId And TagId = @tagId 
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