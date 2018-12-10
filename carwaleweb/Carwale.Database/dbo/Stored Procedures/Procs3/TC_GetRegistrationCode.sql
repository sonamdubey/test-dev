IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetRegistrationCode]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetRegistrationCode]
GO
	/*
	Author		: Vivek Rajak
	Date		: 06/06/2015
	Description : SP to Get State's rto_code and RTOno
	Modified By : Suresh Prajapati on 07th Sept, 2015
	Description : Changed the process of appending '0' to single digit RtoNum
	Modified By : Chetan Navin on 19th Dec 2016 (Added condition for Delhi in query to get rtono)
*/
CREATE PROCEDURE [dbo].[TC_GetRegistrationCode] @StateRtoCodeId INT = NULL
AS
BEGIN
	DECLARE @RTONum VARCHAR(5)

	--Gets State RTO Code
	IF (@StateRtoCodeId IS NULL)
	BEGIN
		SELECT StateRTOCode
			,StateRTOCodeId
		FROM StatesRTOCode WITH (NOLOCK)
		ORDER BY StateRTOCode
	END
			--Gets RTO No of cities
	ELSE
	BEGIN
		SELECT CASE 
				--WHEN (CAST(RTONo AS INT)) <= 9
				--	THEN '0'+RTONo
				--ELSE RTONo
				--END AS RTONo
				WHEN LEN(RTONo) < 2 AND CityName NOT LIKE '%Delhi%'
					THEN '0' + RTONo
				ELSE RTONo
				END AS RTONo
			,CityName
		FROM StateRTOCities WITH (NOLOCK)
		WHERE StateRTOCodeId = @StateRtoCodeId --AND RTONo IN (SELECT RTONo FROM StateRTOCities WITH(NOLOCK)
			--WHERE StateRTOCodeId = @StateRtoCodeId)
	END
END

-----------------------------------------------------------------------------
