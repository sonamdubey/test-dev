IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_VisitAppointmentsLoad]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_VisitAppointmentsLoad]
GO

	-- =============================================
-- Author:		Nilesh Utture
-- Create date: 30 Jan 2013
-- Description:	Load details for schedule Appointments
-- =============================================
CREATE PROCEDURE [dbo].[TC_VisitAppointmentsLoad]
	-- Add the parameters for the stored procedure here
	@LeadId BIGINT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT S.Purpose,S.VisitDate,S.VisitTime 
	FROM TC_Appointments S WITH(NOLOCK)
	WHERE TC_LeadId=@LeadId
	
END
