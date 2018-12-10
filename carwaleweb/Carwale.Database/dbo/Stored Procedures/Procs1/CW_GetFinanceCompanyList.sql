IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CW_GetFinanceCompanyList]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CW_GetFinanceCompanyList]
GO

	-- =============================================
-- Author:		Piyush Sahu
-- Create date: 9/2/2016
-- Description:	Bind list of company provided by HDFC
-- =============================================
CREATE PROCEDURE [dbo].[CW_GetFinanceCompanyList] @ClientId INT
AS
BEGIN
	SELECT Id
		,CompanyName AS NAME
	FROM CW_CompanyList WITH (NOLOCK)
	WHERE IsActive = 1
		AND ClientId = @ClientId
	ORDER BY CompanyName
END
