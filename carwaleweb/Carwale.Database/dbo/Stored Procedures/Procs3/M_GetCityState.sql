IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[M_GetCityState]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[M_GetCityState]
GO

	-- =============================================
-- Author	:	Sachin Bharti(3rd Jan 2013)
-- Description	:	Get City and State for opr mobile website
-- =============================================
CREATE PROCEDURE [dbo].[M_GetCityState]
AS
	BEGIN
		SELECT C.ID , C.Name +'-'+S.Name AS CityState  
		FROM Cities C(NOLOCK) 
		INNER JOIN States S(NOLOCK) ON S.ID = C.StateId
		ORDER BY C.Name
	END




/****** Object:  StoredProcedure [dbo].[NCS_AddNCDCampaign]    Script Date: 01/07/2014 13:11:11 ******/
SET ANSI_NULLS ON
