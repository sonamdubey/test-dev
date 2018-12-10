IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UpdateVideoCount]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UpdateVideoCount]
GO

	-- =============================================
-- Author:		Ravi Koshal	
-- Create date: 5/6/2014
-- Description:	Update Video Count
-- =============================================
CREATE PROCEDURE [dbo].[UpdateVideoCount] 
	-- Add the parameters for the stored procedure here
	@ModelId bigint
AS
BEGIN

	with CTE
as
(
select count(CV.Id) as VideoCnt,ModelId
from Con_EditCms_Videos CV
inner join Con_EditCms_Cars AS CC ON CC.BasicId = CV.BasicId
where CV.Isactive=1

Group by ModelId
) 


update CM
set videocount=VideoCnt
from CTE as CE
inner join carmodels as CM on CM.ID=CE.ModelId
where CM.ID = @ModelId


END