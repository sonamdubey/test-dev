IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Support_GenerateTicket]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Support_GenerateTicket]
GO

	-- =============================================
-- Author:		<Amit Kumar>
-- Create date: <18th may 2013>
-- Description:	<To Save the ticket generated from user>
-- Modified By :Komal Manjare on 27-09-2016
-- Desc : Reduced parameters and use ticketlogid to save data in Support_AttachedFileDetails
-- =============================================
CREATE PROCEDURE [dbo].[Support_GenerateTicket]

@SubcategoryId  NUMERIC(18,0),
@InitiatedBy	NUMERIC(18,0),
@TicketOwner	NUMERIC(18,0),
@Status			SMALLINT,
@Comment		VARCHAR(1500),
@TktId			INT=0 OUT,
@AttachFileName VARCHAR(MAX)=NULL

AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @TicketId NUMERIC(18,0),
	@CurrentDate DATETIME=GETDATE(), -- Komal Manjare on 27-09-2016
	@TicketLogId INT 

	INSERT INTO Support_Tickets (SubcategoryId,InitiatedBy,TicketOwner,TicketDate,Status,LastActionDate,Comment)
		VALUES(@SubcategoryId,@InitiatedBy,@TicketOwner,@CurrentDate,@Status,@CurrentDate,@Comment) -- Komal Manjare on 27-09-2016
	
	SET @TicketId = SCOPE_IDENTITY()
	SET @TktId = @TicketId
		
	INSERT INTO Support_TicketLog(TicketId,CreatedOn,Status,Comment,CreatedBy) 
		VALUES(@TicketId,@CurrentDate,@Status,@Comment,@InitiatedBy) -- Komal Manjare on 27-09-2016
		SET @TicketLogId=SCOPE_IDENTITY()

	IF @AttachFileName IS NOT NULL AND @AttachFileName != ''
	BEGIN
		-- insert image data in Support_AttachedFileDetails
		EXEC [dbo].[Support_SaveTicketImages] @AttachFileName,@InitiatedBy,@TicketLogId
	END
END


