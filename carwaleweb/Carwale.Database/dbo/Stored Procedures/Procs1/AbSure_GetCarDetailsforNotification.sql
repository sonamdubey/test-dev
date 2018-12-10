IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AbSure_GetCarDetailsforNotification]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AbSure_GetCarDetailsforNotification]
GO

	-- =============================================
-- Author:		Ruchira Patil
-- Create date: 24th Jun 2015
-- Description:	to get car details for the mobile notificationwhich has to be send to the surveyor on every assignment
-- =============================================
CREATE PROCEDURE [dbo].[AbSure_GetCarDetailsforNotification]
	@Absure_CarDetailsId INT
AS
BEGIN
	SELECT		CD.OwnerName CustomerName,ACD.Name CustomerArea ,D.Organization DealerName,AD.Name DealerArea,CD.DealerId DealerId
	FROM		AbSure_CarDetails CD WITH(NOLOCK)
	INNER JOIN	DEALERS D WITH(NOLOCK) ON CD.DealerId = D.ID
	INNER JOIN	Areas ACD WITH(NOLOCK) ON ACD.ID=CD.OwnerAreaId
	INNER JOIN	Areas AD WITH(NOLOCK) ON AD.ID=D.AreaId
	WHERE		CD.Id=@Absure_CarDetailsId

END
-----------------------------------------------------------------------------------------------------------------------------------------------

