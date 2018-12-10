IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_EagernessChange]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_EagernessChange]
GO
	

-- =============================================
-- Author:		<Author,Nilesh Utture>
-- Create date: <Create Date,08/1/2013>
-- Description:	<Description, Sets Eagerness for Lead>
-- DECLARE @RetVal SMALLINT EXEC TC_EagernessChange 1,1,1, @RetVal
-- Modified By: Nilesh Utture on 19th Feb, 2013 Added  "IsActive = 1 in WHERE CLAUSE OF TC_InquiriesLead TABLE"
-- Modified by: Nilesh Utture on 24th April,2013 Removed check for userId
-- =============================================
CREATE PROCEDURE [dbo].[TC_EagernessChange]
	-- Add the parameters for the stored procedure here
		@LeadId BIGINT,
		@Eagerness SMALLINT,
		@UserId BIGINT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here 
	UPDATE TC_InquiriesLead SET TC_InquiryStatusId = @Eagerness, ModifiedBy = @UserId, ModifiedDate = GETDATE() WHERE TC_LeadId = @LeadId AND IsActive = 1 --AND TC_UserId = @UserId -- Modified by: Nilesh Utture on 24th April,2013 
	RETURN 1
END


