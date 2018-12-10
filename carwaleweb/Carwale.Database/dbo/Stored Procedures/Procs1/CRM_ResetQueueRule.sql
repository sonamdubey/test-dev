IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_ResetQueueRule]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_ResetQueueRule]
GO

	-- =============================================
-- Author:		Ruchira Patil
-- Create date: 21 JAN 2013
-- Description: To activate Queue Rule for the dealers
-- =============================================
CREATE PROCEDURE [dbo].[CRM_ResetQueueRule]
AS
BEGIN
	UPDATE CRM_ADM_QueueRuleParams SET IsActive = 1 
	WHERE DealerId IN (SELECT DealerId FROM CRM_ADM_QueueRuleParams WHERE IsActive = 0)
END
