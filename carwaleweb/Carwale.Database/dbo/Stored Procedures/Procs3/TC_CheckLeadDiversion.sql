IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_CheckLeadDiversion]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_CheckLeadDiversion]
GO

	-- =============================================
-- Author:		<Author,Nilesh Utture>
-- Create date: <Create Date,20th August, 2013>
-- Description:	<Description, Check lead diversion and get the new lead owner Id>
-- =============================================
CREATE PROCEDURE [dbo].[TC_CheckLeadDiversion]
	-- Add the parameters for the stored procedure here
	@LeadId INT,
	@LeadOwnerId INT OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @OldLeadOwnerId INT 
	SET @OldLeadOwnerId = @LeadOwnerId
    -- Insert statements for procedure here
	SELECT TOP(1) @LeadOwnerId = TC_UsersId FROM TC_ActiveCalls WHERE TC_LeadId = @LeadId
	
	IF @OldLeadOwnerId <> @LeadOwnerId 
	BEGIN
		RETURN 1
	END
	ELSE
	BEGIN
		RETURN 0
	END
END







/****** Object:  StoredProcedure [dbo].[TC_CustomerDetailSave]    Script Date: 08/21/2013 15:59:15 ******/
SET ANSI_NULLS ON
