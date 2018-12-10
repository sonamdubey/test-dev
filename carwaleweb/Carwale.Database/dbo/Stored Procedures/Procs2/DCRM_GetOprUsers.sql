IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_GetOprUsers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_GetOprUsers]
GO

	--==============================================================
-- Author		: Suresh Prajapati
-- Created On	: 20th Nov, 2015
-- Description	: To Get data for binding Opr users dropdown list
-- Modified By	: Suresh Prajapati on 11th Dec, 2015
-- Description	: To Get Login Id of Opr User
-- EXEC DCRM_GetOprUsers
--==============================================================
CREATE PROCEDURE [dbo].[DCRM_GetOprUsers]
AS
BEGIN
	SELECT OU.Id AS Value
		,OU.UserName AS [Text]
		,OU.Address AS Address
		,OU.PhoneNo AS PhoneNo
		,OU.LoginId + '@carwale.com' AS LoginId
	FROM OprUsers AS OU WITH (NOLOCK)
	INNER JOIN DCRM_ADM_Users AS DAU WITH (NOLOCK) ON DAU.UserId = OU.Id
	LEFT JOIN TC_CWExecutiveMapping AS EM WITH (NOLOCK) ON EM.OprUserId = OU.Id
	WHERE OU.Id = DAU.UserId
		AND DAU.IsActive = 1
		AND OU.IsActive = 1
		AND EM.OprUserId IS NULL
	ORDER BY UserName
END


