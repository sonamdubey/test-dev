IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetTopNews]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetTopNews]
GO

	-- =============================================
-- Author:		<Reshma Shetty>
-- Create date: <24/12/2012>
-- Description:	<Fetches the top news as per keyword specified>
-- Changes BY: Satish Sharma
-- Size of input variable @Keyword was not there.

-- Altered By Satish Sharma On 18-Mar-2013
-- Removed hard coded image path 'ImagePath'
-- Altered By Akansha On 26-Sep-2013
-- Commented SET @RecordCount and added the value directly in @RecordCount initialization
-- Altered By Akansha On 30-11-2013 Added column ImagePathLarge
-- Modified by Akansha on 05-02-2014
-- Modified by Manish on 15-07-2014 added with (nolock) keyword
-- Modified by: Natesh on 20-7-2014 Added Application id flag for CMS merging
-- Modified by : Supriya on 8/8/2014 added applicationid as input parameter
-- =============================================
-- =============================================
CREATE PROCEDURE [dbo].[GetTopNews] 

	-- Add the parameters for the stored procedure here
	@RecordCount INT = 3,
	@Keyword VARCHAR(150)=NULL
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;	
    -- Insert statements for procedure here
    
    --SET  @RecordCount=3 //Commented because it overrides the actual @RecordCount passed
    
	-- Altered By Akansha On 30-11-2013 Added column ImagePathLarge
    IF(@Keyword IS NULL)
	BEGIN	
		SELECT TOP (@RecordCount) CB.Id AS BasicId, CB.AuthorName, CB.Description, CB.DisplayDate, 
		CB.Views, CB.Title, CB.Url, (ECI.HostUrl+ ImagePathThumbnail) AS ImagePath, ECI.HostUrl+ECI.ImagePathCustom ImagePathCustom,ECI.HostUrl+ECI.ImagePathLarge ImagePathLarge,
		--ROW_NUMBER()OVER(ORDER BY CB.IsSticky DESC) Row_No
		ROW_NUMBER()OVER(ORDER BY DisplayDate DESC) Row_No
		FROM Con_EditCms_Basic AS CB  WITH (NOLOCK)
		LEFT JOIN Con_EditCms_Images ECI WITH (NOLOCK)  ON ECI.BasicId = CB.Id AND ECI.IsMainImage = 1
		WHERE CB.CategoryId in (1,9) AND CB.IsActive = 1 AND CB.IsPublished = 1 AND CB.ApplicationID = 1 AND ImagePathCustom IS NOT NULL
		--ORDER BY CB.IsSticky desc , DisplayDate DESC 
		ORDER BY DisplayDate DESC 
	END
	ELSE IF(LEN(@Keyword)>0)
	BEGIN		
			SELECT TOP (@RecordCount) CB.Id AS BasicId, CB.AuthorName, CB.Description, CB.DisplayDate, 
			CB.Views, CB.Title, CB.Url, (ECI.HostUrl+ ImagePathThumbnail) AS ImagePath, ECI.HostUrl+ECI.ImagePathCustom ImagePathCustom, 
			ROW_NUMBER()OVER(ORDER BY DisplayDate DESC) Row_No
			FROM Con_EditCms_Basic AS CB WITH (NOLOCK)
			LEFT JOIN Con_EditCms_Images ECI WITH (NOLOCK) ON ECI.BasicId = CB.Id AND ECI.IsMainImage = 1
			LEFT JOIN Con_EditCms_BasicTags BT WITH (NOLOCK)  ON BT.BasicId = CB.Id 
			LEFT JOIN Con_EditCms_Tags T WITH (NOLOCK) ON T.Id = BT.TagId 
			WHERE CB.CategoryId in (1,9) AND CB.IsActive = 1 AND CB.IsPublished = 1 AND CB.ApplicationID = 1
			 AND T.Slug = @Keyword
			ORDER BY DisplayDate DESC 
	END
END


