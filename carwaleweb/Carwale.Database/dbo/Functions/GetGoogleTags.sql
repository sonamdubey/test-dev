IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetGoogleTags]') 
    AND xtype IN (N'FN', N'IF', N'TF')
)
    DROP FUNCTION [dbo].[GetGoogleTags]
GO

	-- =============================================
-- Author:		<Ravi Koshal>
-- Create date: <8/1/2013>
-- Description:	<Returns the tags as comma separated values>
-- =============================================
CREATE FUNCTION [dbo].[GetGoogleTags] 
(
	-- Add the parameters for the function here
	@Basic INT
)
RETURNS VARCHAR(MAX)
AS
BEGIN
DECLARE @Tags VARCHAR(MAX)
select @Basic=BasicId,@Tags = COALESCE(@Tags+', ','') + CONVERT(VARCHAR,CT.Tag )
from Con_EditCms_BasicTags as CB
join Con_EditCms_Tags as CT on CB.TagId=CT.Id
where BasicId=@Basic
return @Tags

END
