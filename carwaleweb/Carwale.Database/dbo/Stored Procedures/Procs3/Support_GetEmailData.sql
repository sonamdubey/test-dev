IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Support_GetEmailData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Support_GetEmailData]
GO

	-- =============================================
-- Author:	Komal Manjare 
-- Create date: 28 Sept 2016
-- Description:	Get email data to report problem
-- =============================================
CREATE PROCEDURE [dbo].[Support_GetEmailData] 
@SubCategoryId INT,
@CurrentUserId INT
AS
BEGIN
	SELECT SSC.AssignedTo,OU.LoginId AS LogInId,OU.UserName,
	REPLACE(STUFF((
				SELECT ', ' + LoginId + '@carwale.com' 
				FROM OprUsers (NOLOCK) 
				WHERE Id IN(SELECT ListMember FROM fnSplitCSV(SSC.SubAssignee)) FOR XML PATH('')), 1, 2, ''),' ', '') AS EMailData,
	SSC.Name AS SubCategoryName,SC.Name AS CategoryName
	FROM Support_SubCategory SSC (NOLOCK) 
	INNER JOIN OprUsers OU (NOLOCK) ON OU.Id=SSC.AssignedTo 
	INNER JOIN Support_Category  SC (NOLOCK) ON SC.Id=SSC.CategoryId
	WHERE SSC.Id = @SubCategoryId
	SELECT OU.LoginId AS LoginId , OU.UserName AS Name FROM OprUsers OU (NOLOCK) WHERE OU.Id = @CurrentUserId 
END

