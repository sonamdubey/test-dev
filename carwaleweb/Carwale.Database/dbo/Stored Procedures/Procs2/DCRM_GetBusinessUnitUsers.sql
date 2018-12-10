IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_GetBusinessUnitUsers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_GetBusinessUnitUsers]
GO

	

-- =============================================
-- Author	:	Sachin Bharti(14th May 2015)
-- Description	:	Get all  opr users based on business unit
-- execute [dbo].[DCRM_GetBusinessUnitUsers] 1
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_GetBusinessUnitUsers] 
	@BusinessUnitId	INT,
	@OprUserId INT = NULL --to get user above and at the same level
AS
	BEGIN
		IF @OprUserId IS NULL
			BEGIN
				SELECT
					DISTINCT OU.Id AS Value,
					OU.UserName AS Text
				FROM
					DCRM_ADM_MappedUsers AMU(NOLOCK)
					INNER JOIN OprUsers OU(NOLOCK) ON OU.Id= AMU.OprUserId
				WHERE
					AMU.BusinessUnitId IN (@BusinessUnitId,4)
					AND AMU.IsActive = 1
				ORDER BY
					OU.UserName
			END
		ELSE IF @OprUserId IS NOT NULL
			BEGIN
				SELECT 
					DISTINCT OU.Id AS Value, 
					OU.UserName  AS Text
				FROM
					DCRM_ADM_MappedUsers AMU(NOLOCK) 
					INNER JOIN OprUsers OU(NOLOCK) ON AMU.OprUserId = OU.Id
				WHERE 
					AMU.BusinessUnitId IN (@BusinessUnitId,4)
					AND NodeRec.GetLevel() <= (SELECT NodeRec.GetLevel() FROM DCRM_ADM_MappedUsers(NOLOCK) where OprUserId = @OprUserId AND IsActive = 1) and OprUserId <> @OprUserId
					AND AMU.IsActive = 1
				ORDER BY
					OU.UserName
			END
	END

