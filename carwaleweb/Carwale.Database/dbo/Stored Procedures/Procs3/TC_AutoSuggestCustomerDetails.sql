IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_AutoSuggestCustomerDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_AutoSuggestCustomerDetails]
GO

	-- =============================================
-- Author:		<Khushaboo Patil>
-- Create date: <19/08/2015>
-- Description:	<Fetch Customer Name,Email,Mobile for autosuggest>
-- Modified By : Nilima More On 29th 2016(Added Condition to fetch only active leads with TC_LeadStageId <> 3 and ActiveLeadId IS NOT NULL).
-- =============================================
CREATE PROCEDURE [dbo].[TC_AutoSuggestCustomerDetails] 
	@BranchId        INT,
	@SearchText      VARCHAR(50)
AS
BEGIN
	SELECT DISTINCT TOP 20 C.Id,C.CustomerName , C.Email , C.Mobile
	FROM TC_CustomerDetails  AS C WITH (NOLOCK)
	INNER JOIN TC_Lead L WITH(NOLOCK) ON L.TC_CustomerId = C.Id 		
	WHERE C.BranchId=@BranchId 
	AND C.IsleadActive = 1   
	AND (
			(CHARINDEX('@',@SearchText) > 0 AND LOWER(Email) LIKE @SearchText + '%') OR 			  
			(ISNUMERIC(@SearchText) = 1 AND LOWER(Mobile) LIKE @SearchText + '%')OR  
			(LOWER(C.CustomerName) LIKE @SearchText + '%')
		)
	AND C.ActiveLeadId IS NOT NULL
	AND L.TC_LeadStageId <> 3
	ORDER BY C.CustomerName ASC	  
END
