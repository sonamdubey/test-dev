IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UpdatePQDealerInquiryDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UpdatePQDealerInquiryDetails]
GO

	

-- =============================================
-- Author:		Ashish Verma
-- Create date: 07/08/2014
-- Description:	To Update response after PQ dealer add inquiry details before calling to api
-- exec [dbo].[SavePQDealerInquiryDetails] 35,1,""
-- =============================================
CREATE PROCEDURE [dbo].[UpdatePQDealerInquiryDetails] 
	-- Add the parameters for the stored procedure here
	@Id NUMERIC
	,@PushStatus INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here

	UPDATE PQDealerAdLeads
	SET PushStatus =@PushStatus
	WHERE  Id = @Id
	RETURN
END

