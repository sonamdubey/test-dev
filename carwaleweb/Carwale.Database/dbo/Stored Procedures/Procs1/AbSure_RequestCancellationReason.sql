IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AbSure_RequestCancellationReason]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AbSure_RequestCancellationReason]
GO

	-- =============================================
-- Author:		Yuga Hatolkar
-- Create date: 26th Feb, 2015
-- Description:	Get Cancellation reason.
-- Modified By: Ashwini Dhamankar on March 17,2015, Added IsActive condition in where clause
-- =============================================
CREATE PROCEDURE [dbo].[AbSure_RequestCancellationReason]
AS
	BEGIN
		SELECT		Id, Reason, IsActive 
		FROM		AbSure_ReqCancellationReason
		WHERE       IsActive = 1 
		ORDER BY	Reason
	END