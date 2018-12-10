IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_UpdateTaskListDetails_V16]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_UpdateTaskListDetails_V16]
GO

	
-- =============================================
-- Author:		Ashwini Dhamankar
-- Create date: September 22,2016
-- Description:	To update LeadDipositionId and NextActionDate
-- =============================================
CREATE PROCEDURE [dbo].[TC_UpdateTaskListDetails_V16.9.1]
@TC_LeadId INT,
@TC_LeadDispositionId INT,
@TC_NextActionDate DATETIME
AS
BEGIN
	UPDATE TC_TaskLists SET TC_LeadDispositionId = @TC_LeadDispositionId,TC_NextActionDate = @TC_NextActionDate WHERE TC_LeadId = @TC_LeadId
END



