IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[NCSDealers_Info_v16_8_5]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[NCSDealers_Info_v16_8_5]
GO

	
-- =============================================
-- Author:		sanjay soni
-- Create date: 07/03/2016
-- Description:	Return Dealer Name based on NCS Dealer Id. 
-- Modified by: Chetan on 17/08/2016 - Fetching Address (Pincode) of dealers
-- =============================================
CREATE PROCEDURE [dbo].[NCSDealers_Info_v16_8_5] @DealerID INT
AS
BEGIN
	SELECT NCSD.NAME AS DealerName
		,NCSD.DealerCode
		,NCSD.Mobile
		,NCSD.[Address] AS PinCode
		,S.StateCode
	FROM NCS_Dealers NCSD WITH (NOLOCK)
	INNER JOIN Cities C WITH (NOLOCK) ON NCSD.CityId = C.ID
	INNER JOIN States S WITH (NOLOCK) ON S.ID = C.StateId
	WHERE NCSD.ID = @DealerID
END

