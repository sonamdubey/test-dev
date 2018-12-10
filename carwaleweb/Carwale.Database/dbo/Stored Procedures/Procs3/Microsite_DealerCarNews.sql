IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Microsite_DealerCarNews]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Microsite_DealerCarNews]
GO

	
-- =============================================
-- Author:		Umesh Ojha
-- Create date: 20/06/2012
-- Description:	This Sp returns car news according to dealer website and also using for 
---             repeater pager with SP.
-- =============================================
CREATE PROCEDURE [dbo].[Microsite_DealerCarNews] 
	-- Add the parameters for the stored procedure here
	@MakeId INT,
	@CategoryId INT, 
	@StartIndex INT = null,
	@EndIndex INT = null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	with DealerCarNews as
	(
		select CarNews.*, row_number() over(order by CarNews.DisplayDate desc) as displayRowNum from
		(
			Select CB.Id AS BasicId, CB.AuthorName, CB.Description, CB.DisplayDate,CB.Title, CB.Url, CB.MainImageSet, CB.HostURL 
			from Con_EditCms_Basic CB 
			JOIN Con_EditCms_Cars CEC On CEC.BasicId = CB.Id
			Where CB.IsActive =1 And CB.IsPublished =1 And CB.CategoryId=@CategoryId and CB.IsDealerFriendly = 1  AND CEC.MakeId = @MakeId And CEC.IsActive = 1
			UNION
			Select CB.Id AS BasicId, CB.AuthorName, CB.Description, CB.DisplayDate,CB.Title, CB.Url, CB.MainImageSet, CB.HostURL 
			from Con_EditCms_Basic CB
			Where CB.IsActive =1 And CB.IsPublished =1 And CB.IsDealerFriendly = 1 And CB.CategoryId=@CategoryId
			AND CB.Id not in (select BasicId from Con_EditCms_Cars)
		)CarNews
	)
	select * from DealerCarNews where displayRowNum between @StartIndex and @EndIndex	 
	
	-- now fetching no of count from the abve recordes
	Select count(RecordCount.Id) AS RecordCount From
	(
		Select CB.Id from Con_EditCms_Basic CB JOIN Con_EditCms_Cars CEC On CEC.BasicId = CB.Id
		Where CB.IsActive =1 And CB.IsPublished =1 And CB.CategoryId=@CategoryId 
		and CB.IsDealerFriendly = 1  AND CEC.MakeId = @MakeId And CEC.IsActive = 1
		UNION
		Select CB.Id from Con_EditCms_Basic CB Where CB.IsActive =1 And 
		CB.IsPublished =1 And CB.IsDealerFriendly = 1 And CB.CategoryId=@CategoryId
		AND CB.Id not in (select BasicId from Con_EditCms_Cars) 
	)RecordCount
END

