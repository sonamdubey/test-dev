IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[ESM_GetAccountUsers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[ESM_GetAccountUsers]
GO

	-- =============================================
-- Author	:	Sachin Bharti(5th Aug 2015)
-- Description	:	Get account manager users for ESM Agency Or Client
-- =============================================
CREATE PROCEDURE [dbo].[ESM_GetAccountUsers]
	
AS
BEGIN
	SELECT 
		DISTINCT OU.Id ,
		OU.UserName 
	FROM
		OprUsers OU(NOLOCK) 
		INNER JOIN ESM_Users EU(NOLOCK) ON OU.Id = EU.UserId
	WHERE
		OU.IsActive = 1
	ORDER BY
		OU.UserName
END


