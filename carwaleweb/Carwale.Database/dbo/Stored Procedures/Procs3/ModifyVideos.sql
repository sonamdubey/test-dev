IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[ModifyVideos]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[ModifyVideos]
GO

	
-- ============================================= 
-- Author: Prashant Vishe	    
-- Create date:22 dec 2013  
-- Description: for inserting and updating video data...  
-- Modifed On:3 march 2013 by Prashant
-- Modification:added VideoId in query
-- ============================================= 
CREATE PROCEDURE [dbo].[ModifyVideos] 
  -- Add the parameters for the stored procedure here 
  @BasicId  NUMERIC, 
  @VideoUrl VARCHAR(100), 
  @Views    INT = 0, 
  @Likes    INT = 0,
  @Duration numeric = 0,
  @VideoId varchar(100)
AS 
  BEGIN 
      -- SET NOCOUNT ON added to prevent extra result sets from 
      -- interfering with SELECT statements. 
      --SET nocount ON; 

      DECLARE @newcount INT; 

      SET @newcount=(SELECT Count(*) 
                     FROM   con_editcms_videos WITH (NOLOCK)
                     WHERE  basicid = @BasicId) 

      -- Insert statements for procedure here 
      IF( @newcount = 0 ) 
        BEGIN 
            INSERT INTO con_editcms_videos 
                        (basicid, 
                         videourl, 
                         views, 
                         likes,
						 duration,
						 videoId) 
            VALUES      (@BasicId, 
                         @VideoUrl, 
                         @Views, 
                         @Likes,
						 @Duration,
						 @VideoId) 

			-- Added by Ravi to update the video count in the models table.
			DECLARE @ModelId bigint
			select @ModelId = modelid from Con_EditCms_Cars where BasicId = @BasicId
         EXECUTE [dbo].[UpdateVideoCount] @ModelId

        END 
      ELSE 
        BEGIN 
			 IF @VideoUrl='' and @Views=0 and @likes <> 0 and @Duration=0
				 BEGIN
					UPDATE con_editcms_videos 
					SET   likes = @Likes 
					WHERE  basicid = @BasicId 
				 END

		   else IF @VideoUrl='' and @Likes=0 and @Views<> 0 and @Duration=0
				BEGIN
					UPDATE con_editcms_videos 
					SET   Views=@Views
					WHERE  basicid = @BasicId 
				END
		
		    else IF 	@VideoUrl='' and @Views <> 0 and @likes <> 0 and @Duration=0
				BEGIN
					UPDATE con_editcms_videos 
					SET   Views=@Views,
						  likes = @Likes 
					WHERE  basicid = @BasicId 
				END
			else
				begin
					UPDATE con_editcms_videos 
					SET    videourl = @VideoUrl, 
						   views = @Views, 
						   likes = @Likes ,
						   duration=@Duration,
						   videoId=@VideoId
					WHERE  basicid = @BasicId 
				end
        END
  END 


