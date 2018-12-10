IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetSavedPriceQuotes]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetSavedPriceQuotes]
GO

	/*
	Author  :   Vishal Srivastava AE1830
    Date    :   09-10-2013
    Description:Get Previous Models price quotes Detail for PriceQuotes.aspx page
*/
CREATE PROCEDURE [dbo].[TC_GetSavedPriceQuotes]
@dealerId INT,
@userId INT,
@versionId INT
AS
BEGIN

	SELECT V.TC_PQFieldMasterId AS Id,V.Field AS Value,Value AS txtValue,CVPQ.TC_CarVersionPQId AS Name
	 FROM TC_CarVersionPQ AS CVPQ WITH(NOLOCK)
	INNER JOIN TC_PQFieldMaster AS V WITH(NOLOCK)
	ON CVPQ.TC_PQFieldMasterId=V.TC_PQFieldMasterId
	AND CVPQ.DealerId=@dealerId
--	AND CVPQ.TC_UserID=@userId
	AND CVPQ.VersionId=@versionId
	AND CVPQ.IsActive=1
	AND  V.IsActive=1

END




