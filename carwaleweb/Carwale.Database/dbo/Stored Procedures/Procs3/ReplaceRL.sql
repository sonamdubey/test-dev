IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[ReplaceRL]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[ReplaceRL]
GO

	CREATE procedure [dbo].[ReplaceRL]
as
begin
--Declare necessary variables 
DECLARE @searchstring1 VARCHAR(20) 
DECLARE @searchstring2 VARCHAR(20) 
DECLARE @lensearch smallint 

--Set values for search  
SELECT @searchstring1 = 'View Specifications'
--SELECT @lensearch = LEN(@searchstring) 
SELECT @searchstring2 = '..'
    
Select PC.Data,REPLACE(PC.Data,@searchstring2,'http://www.carwale.com/research')
From Con_EditCms_Basic CB
Inner Join Con_EditCms_Pages CP On CP.BasicId = CB.Id
Inner Join Con_EditCms_PageContent PC On PC.PageId = CP.Id
Where PC.Data  LIKE '%'+@searchstring1 + '%' 
and PC.Data  LIKE '%'+@searchstring2 + '%' 
end