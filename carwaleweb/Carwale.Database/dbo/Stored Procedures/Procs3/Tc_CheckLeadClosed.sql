IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Tc_CheckLeadClosed]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Tc_CheckLeadClosed]
GO

	
-- =============================================
-- Author:		Nilima More 
-- Create date: 16 December 2015
-- Description:	To check lead is closed or not.
-- Avishkar Commented 28-06-2016
-- =============================================
CREATE PROCEDURE [dbo].[Tc_CheckLeadClosed] --16064,NULL
	@TC_LeadId BIGINT
	,@isclosed BIT OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT TC_LeadId
	FROM TC_Lead L WITH(NOLOCK)
	-- Avishkar Commented 28-06-2016
	--JOIN TC_InquiriesLead IL WITH (NOLOCK) ON N.TC_InquiriesLeadId = IL.TC_InquiriesLeadId
	--JOIN TC_Lead L WITH (NOLOCK) ON L.TC_LeadId = IL.TC_LeadId
	WHERE L.TC_LeadStageId = 3
		AND L.TC_LeadId = @TC_LeadId

	IF (@@ROWCOUNT > 0)
		SET @isclosed = 1
	ELSE
		SET @isclosed = 0
END
