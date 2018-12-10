IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_PreviousPriceQuotes]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_PreviousPriceQuotes]
GO

	/*
	Author  :   Vishal Srivastava AE1830
    Date    :   09-10-2013
    Description:Get Previous Models name for PriceQuotes.aspx page
*/
CREATE PROCEDURE [dbo].[TC_PreviousPriceQuotes]
@dealerId INT,
@userId INT,
@modelId int
AS
BEGIN

	SELECT DISTINCT CVPQ.VersionId AS [Value],V.[Version] AS [Text] FROM TC_CarVersionPQ AS CVPQ WITH(NOLOCK)
	INNER JOIN vwMMV AS V WITH(NOLOCK)
	ON CVPQ.DealerId=@dealerId
	--AND CVPQ.TC_UserID=@userId
	AND CVPQ.VersionId=V.VersionId
	AND V.ModelId=@modelId
	AND CVPQ.IsActive=1

END



/****** Object:  StoredProcedure [dbo].[TC_GetFieldForPriceQuotes]    Script Date: 10/15/2013 7:08:29 PM ******/
SET ANSI_NULLS ON
