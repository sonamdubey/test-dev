IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetClosedLeadDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetClosedLeadDetails]
GO
	-- =============================================
-- Author:		Ashwini Dhamankar
-- Create date: November 30,2015
-- Description:	Get Closed lead details
-- =============================================
CREATE PROCEDURE [dbo].[TC_GetClosedLeadDetails] 
	@BranchId BIGINT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	SELECT			C.CustomerName,C.Mobile AS CustomerMobile,TL.LeadClosedDate AS ClosingDate,TCLD.Name AS Reason
	FROM			TC_CustomerDetails C WITH(NOLOCK)
	INNER JOIN		TC_Lead TL WITH(NOLOCK) ON TL.TC_CustomerId = c.Id
	LEFT JOIN		TC_LeadDisposition AS TCLD WITH(NOLOCK) ON TL.TC_LeadDispositionId = TCLD.TC_LeadDispositionId
	WHERE			TL.TC_LeadStageId = 3 AND TL.BranchId = @BranchId
END

---------------------------------------------------------------------------------------

/****** Object:  StoredProcedure [dbo].[TC_TaskListLeadCount]    Script Date: 12/9/2015 7:01:26 PM ******/
SET ANSI_NULLS ON
