IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DeleteVideos]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DeleteVideos]
GO

	-- =============================================
-- Author:		Prashant Vishe	
-- Create date: 22 Dec 2013
-- Description:	For deleting video data...
-- =============================================
CREATE PROCEDURE [dbo].[DeleteVideos] 
	-- Add the parameters for the stored procedure here
	@BasicId numeric
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	--SET NOCOUNT ON;


    -- Insert statements for procedure here
	--Delete from Con_EditCms_Videos where BasicId=@BasicId

	Update Con_EditCms_Videos set IsActive=0 where BasicId=@BasicId


		-- Added by Ravi to update the video count in the models table.
			DECLARE @ModelId bigint
			select @ModelId = modelid from Con_EditCms_Cars WITH(NOLOCK) where BasicId = @BasicId
         EXECUTE [dbo].[UpdateVideoCount] @ModelId



END
/****** Object:  StoredProcedure [dbo].[ModifyVideos]    Script Date: 5/6/2014 8:04:41 PM ******/
SET ANSI_NULLS ON
