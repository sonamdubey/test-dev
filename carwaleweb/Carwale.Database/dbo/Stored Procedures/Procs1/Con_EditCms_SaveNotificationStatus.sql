IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Con_EditCms_SaveNotificationStatus]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Con_EditCms_SaveNotificationStatus]
GO

	
-- Author		:	Meet Shah
-- Create date	:	01/09/2016 18:58:26 
-- Description	:	This SP used to save notification message_id  for andriod and iOS
-- =============================================  
create PROCEDURE [dbo].[Con_EditCms_SaveNotificationStatus]
@BasicId INT,
@AndroidMessageId VARCHAR(50) = NULL,
@IOSMessageId VARCHAR(50) = NULL

AS
BEGIN

UPDATE  Con_EditCms_Basic
SET 
IsNotified = 1
WHERE
ID = @BasicId

INSERT INTO Con_EditCms_Notifications
(BasicId,AndroidMessageId,IOSMessageId) 
VALUES 
(@BasicId,@AndroidMessageId,@IOSMessageId)

END