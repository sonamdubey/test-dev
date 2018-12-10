IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetPQIDandFieldId]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetPQIDandFieldId]
GO
	/*
	Author  :   Vishal Srivastava AE1830
    Date    :   09-10-2013
    Description:Get ID FOR SAVED DATA for PriceQuotes.aspx page
*/
CREATE PROCEDURE [dbo].[TC_GetPQIDandFieldId]
@dealerId INT,
@versionId INT
AS
BEGIN
	SELECT TC_CarVersionPQId AS Id,TC_PQFieldMasterId AS FieldId 
	FROM TC_CarVersionPQ AS tc WITH(NOLOCK) 
	WHERE tc.DealerId=@dealerId 
	AND tc.VersionId=@versionId 
	AND tc.IsActive=1

END
