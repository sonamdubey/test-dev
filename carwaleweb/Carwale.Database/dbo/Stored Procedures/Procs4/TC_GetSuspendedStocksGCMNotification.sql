IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetSuspendedStocksGCMNotification]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetSuspendedStocksGCMNotification]
GO

	
-- =============================================
-- Author:		<Khushaboo Patil>
-- Create date: <31/03/2016>
-- Description:	<Send Suspended Stock Push Notification to app>
-- =============================================
Create PROCEDURE [dbo].[TC_GetSuspendedStocksGCMNotification]
AS
BEGIN
	
		WITH CTE AS(
		SELECT DISTINCT AN.Id NotificationId,TU.Id UserId,TU.UniqueId,AN.NotificationDateTime,
		STUFF
		(
			(
				SELECT ',' + convert(varchar,S.Id)
				FROM TC_Stock S WITH(NOLOCK) 
				WHERE BranchId  = TU.BranchId AND S.StatusId = 4  AND S.IsActive = 1 
				AND CONVERT(DATE,S.SuspendedDate) = CONVERT(DATE,GETDATE())
				FOR XML PATH('')
			), 1, 1, ''
		) AS SuspendedStockIds

		FROM TC_AppNotification AN WITH(NOLOCK)
		INNER JOIN TC_Users TU WITH(NOLOCK) ON TU.Id = AN.TC_UserId
		INNER JOIN TC_Stock S WITH(NOLOCK) ON S.BranchId = TU.BranchId
		WHERE RecordType = 4 AND CONVERT(DATE,S.SuspendedDate) = CONVERT(DATE,GETDATE()) AND
		CONVERT(DATE,NotificationDateTime) = CONVERT(DATE,GETDATE()) AND S.StatusId = 4 AND S.IsActive = 1
		)
		SELECT *,CASE WHEN SuspendedStockIds IS NOT NULL THEN (LEN(SuspendedStockIds) - LEN(REPLACE(SuspendedStockIds, ',', ''))+1) 
		ELSE 0 END AS SuspendedStockCount FROM CTE WITH(NOLOCK)
END
----------------------

