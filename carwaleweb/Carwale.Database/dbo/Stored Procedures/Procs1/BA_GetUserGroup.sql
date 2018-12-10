IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BA_GetUserGroup]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BA_GetUserGroup]
GO

	-- =============================================
-- Author:		Ranjeet Kumar
-- Create date: 07-06-2014
-- Description:	Get the groupId for the Broker
-- =============================================
CREATE PROCEDURE [dbo].[BA_GetUserGroup]
@BrokerId INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT BG.ID AS ID, BG.GroupName AS Name FROM BA_Groups AS BG WITH (NOLOCK) WHERE BG.BrokerID = @BrokerId AND IsActive = 1
END
